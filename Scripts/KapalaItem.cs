using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using HarmonyLib;

namespace JuneGunJamSubmission
{
    [HarmonyPatch]
    public class KapalaItem : CustomChargeTypeItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(KapalaItem))
        {
            Name = "Kapala",
            Description = "Blood red",
            LongDescription = "Drinking from this will heal whoever drinks from it, but this only can be filled from freshly gathered gundead blood.",
            SpriteResource = "JuneGunJamSubmission/Resources/kapala",
            Quality = ItemQuality.A,
            PostInitAction = item =>
            {
                CustomChargeTypeItem citem = (CustomChargeTypeItem)item;
                citem.AddPassiveStatModifier(PlayerStats.StatType.Curse, 3f);
                citem.specialCooldown = 400f;
            }
        };
        public override void Pickup(PlayerController player)
        {
            if (!m_pickedUpThisRun)
            {
                ApplyCooldown(player);
            }
            base.Pickup(player);
        }

        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.OnDidDamage))]
        [HarmonyPostfix]
        public static void OnDamage (float damageDone, bool fatal, HealthHaver target, PlayerController __instance)
        {
            foreach (PlayerItem item in __instance.activeItems) { if (item is KapalaItem kapala) { kapala.OnDamage(damageDone, target, __instance); } }
        }

        public void OnDamage(float dmg, HealthHaver receiver, PlayerController player)
        {
            if (LastOwner == null || !PickedUp) { return; }

            PixelCollider pixelCollider = receiver.specRigidbody.HitboxPixelCollider;
            Vector3 vector = pixelCollider.UnitBottomLeft.ToVector3ZisY(0f);
            Vector3 vector2 = pixelCollider.UnitTopRight.ToVector3ZisY(0f);
            GlobalSparksDoer.DoRandomParticleBurst(UnityEngine.Random.Range(2, 3), vector, vector2, Vector3.down, 90f, 0.5f, systemType: GlobalSparksDoer.SparksType.BLOODY_BLOOD);

            if (IsActive && !PlayerItem.AllowDamageCooldownOnActive)
            {
                return;
            }

            Vector2 v = BraveMathCollege.ClosestPointOnRectangle(player.specRigidbody.UnitCenter, pixelCollider.UnitBottomLeft, pixelCollider.UnitDimensions);
            if (receiver && receiver.specRigidbody && player.specRigidbody
                && (Vector2.Distance(v, player.specRigidbody.UnitCenter) < m_distance))
            {
                float num = 1f;
                GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
                if (lastLoadedLevelDefinition != null)
                {
                    num /= lastLoadedLevelDefinition.enemyHealthMultiplier;
                }
                dmg *= num;
                this.RemainingSpecialCooldown -= dmg;
            }
        }

        public override void DoEffect(PlayerController player)
        {
            base.DoEffect(player);
            if (player.characterIdentity != PlayableCharacters.Robot) { player.healthHaver.ApplyHealing(0.5f); } else { player.healthHaver.Armor += 1; }
            AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", gameObject);
            player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero);
        }

        private float m_distance = 3.5f;
    }
}
