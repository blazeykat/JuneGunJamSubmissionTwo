using Dungeonator;
using HarmonyLib;
using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JuneGunJamSubmission
{
    public class RedLockboxItem : CustomChargeTypeItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(RedLockboxItem))
        {
            Name = "Red Lockbox",
            Description = "Box of Badness",
            LongDescription = "Full of wealth and goodies, but a sacrifice of blood must be made to open it. Holding this will prevent most healing and will instead count towards this item's cooldown.",
            SpriteResource = "JuneGunJamSubmission/Resources/redlockbox.png",
            Quality = ItemQuality.B,
            PostInitAction = item =>
            {
                CustomChargeTypeItem citem = (CustomChargeTypeItem)item;
                citem.specialCooldown = 6f;

                citem.consumable = true;
            }
        };

        public override void DoEffect(PlayerController user)
        {
            for (int i = 0; i < 4; i++)
            {
                PickupObject item = i < 2 ? LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.B, GameManager.Instance.RewardManager.ItemsLootTable, true) : PickupObjectDatabase.GetById(GlobalItemIds.FullHeart);
                Vector2 area = LastOwner.CurrentRoom.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.PlayerCenter).ToVector2();
                IntVector2 spawnPoint = LastOwner.CurrentRoom.GetBestRewardLocation(new IntVector2(1, 1), BraveUtility.RandomVector2(area - new Vector2(3, 3),
                    area + new Vector2(3, 3)));
                LootEngine.SpawnItem(item.gameObject, spawnPoint.ToVector3() + new Vector3(0.25f, 0f, 0f), Vector2.up, 1f, true, true);
            }
            base.DoEffect(user);
        }
        private void ModifyHealing(HealthHaver arg1, HealthHaver.ModifyHealingEventArgs arg2)
        {
            foreach (PlayerItem item in arg1.m_player.activeItems)
            {
                if (item is RedLockboxItem ritem
                    && ritem.RemainingSpecialCooldown > 0)
                {
                    ritem.RemainingSpecialCooldown -= arg2.ModifiedHealing;
                    arg2.ModifiedHealing = 0;
                    return;
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                ApplyCooldown(player);
            }
            base.Pickup(player);
            player.healthHaver.ModifyHealing += ModifyHealing;
        }
        public override void OnPreDrop(PlayerController user)
        {
            user.healthHaver.ModifyHealing -= ModifyHealing;
            base.OnPreDrop(user);
        }

        public override void OnDestroy()
        {
            if (LastOwner != null)
            {
                LastOwner.healthHaver.ModifyHealing -= ModifyHealing;
            }
            base.OnDestroy();
        }

        public static bool AnyRedLockboxesInChat(PlayerController player)
        {
            foreach (PlayerItem item in player.activeItems)
            {
                if (item is RedLockboxItem ritem
                    && ritem.RemainingSpecialCooldown > 0)
                {
                    return true;
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(HealthPickup), "PrePickupLogic")]
        [HarmonyPrefix]
        public static void HandlePickupLogic(HealthPickup __instance, SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody)
        {
            PlayerController playerController = otherRigidbody.GetComponent<PlayerController>();
            if (playerController == null
                || playerController.IsGhost

                || __instance.healAmount <= 0)
            {
                return;
            }

            if (AnyRedLockboxesInChat(playerController))
            {
                __instance.Pickup(playerController);
                Destroy(__instance.gameObject);
            }
        }

        [HarmonyPatch(typeof(HealthPickup), "OnEnteredRange")]
        [HarmonyPrefix]
        public static bool CantSlurp(HealthPickup __instance, PlayerController interactor)
        {
            if (!__instance
                || !AnyRedLockboxesInChat(interactor))
            {
                return true;
            }
            return false;
        }

        [HarmonyPatch(typeof(HealthPickup), nameof(HealthPickup.Interact))]
        [HarmonyPrefix]
        public static bool CantGlurp(PlayerController interactor)
        {
            if (!AnyRedLockboxesInChat(interactor))
            {
                return true;
            }
            return false;
        }
    }
}
