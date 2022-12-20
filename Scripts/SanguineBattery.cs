using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuneLib;
using UnityEngine;

namespace JuneGunJamSubmission
{
    public class SanguineBatteryItem : PassiveItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(SanguineBatteryItem))
        {
            Name = "Red Ice",
            Description = "Bloody Cooldowns",
            LongDescription = "Allows you to use an item regardless of it's cooldown, for a blood price",
            SpriteResource = "JuneGunJamSubmission/Resources/redice",
            Quality = ItemQuality.B,
        };

        public override void Pickup(PlayerController player)
        {
            ItemsCore.OnPreUseItem += OnPreUse;
            base.Pickup(player);
        }

        public override void DisableEffect(PlayerController player)
        {
            ItemsCore.OnPreUseItem -= OnPreUse;
            base.DisableEffect(player);
        }

        private void OnPreUse(PlayerController arg1, PlayerItem arg2, OverrideItemCanBeUsed.ValidOverrideArgs arg3)
        {
            if (arg1.healthHaver && arg1.healthHaver.IsVulnerable)
            {
                arg3.AddActionWithPriority(OverrideItemCanBeUsed.ValidOverrideArgs.Priority.PASSIVE_EFFECT_REGULAR_PRIORITY, OnUse);
                arg3.ShouldBeUseable = true;
            }
        }

        private void OnUse(PlayerController controller, PlayerItem item, OverrideItemCanBeUsed.OnUseOverrideArgs args)
        {
            if (controller && controller.healthHaver)
            {
                HealthHaver haver = controller.healthHaver;
                haver.ApplyDamage(1f, Vector2.zero, "Blood sacrifice", damageCategory: DamageCategory.BlackBullet);
            }
        }
    }
}
