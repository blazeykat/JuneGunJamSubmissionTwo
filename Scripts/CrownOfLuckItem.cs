using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;

namespace JuneGunJamSubmission
{
    public class CrownOfLuckItem : PassiveItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(CrownOfLuckItem))
        {
            Name = "Crimson Crown",
            Description = "In the court",
            LongDescription = "Sets your HP to half a heart upon entering a new floor. Enemies have a random chance to start at near death health themselves.",
            SpriteResource = "JuneGunJamSubmission/Resources/anticrown",
            Quality = ItemQuality.C,
            PostInitAction = item =>
            {
                item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1f);
            }
        };
        public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += FloorLoaded;
            ETGMod.AIActor.OnPostStart += OnEnemySpawned;
            base.Pickup(player);
        }

        private void OnEnemySpawned(AIActor obj)
        {
            if (obj && obj.healthHaver && !obj.healthHaver.IsBoss && UnityEngine.Random.value <= 0.1f)
            {
                obj.healthHaver.ForceSetCurrentHealth(Math.Min(Owner.healthHaver.GetCurrentHealth(), 0.5f));
            }
        }

        private void FloorLoaded(PlayerController obj)
        {
            QueueResetHP = true;
        }

        public override void Update()
        {
            base.Update();
            if (QueueResetHP && Owner && !GameManager.Instance.IsLoadingLevel)
            {
                QueueResetHP = false;
                Owner.healthHaver.ForceSetCurrentHealth(Math.Min(Owner.healthHaver.GetCurrentHealth(), 0.5f));
            }
        }
        public bool QueueResetHP;

        public override void DisableEffect(PlayerController player)
        {
            player.OnNewFloorLoaded -= FloorLoaded;
            base.DisableEffect(player);
        }
    }

}
