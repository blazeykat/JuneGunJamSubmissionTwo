using JuneLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Misc;
using HarmonyLib;
using Alexandria.ItemAPI;

namespace JuneGunJamSubmission
{
    public class AntiKeyItem : PassiveItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(AntiKeyItem))
        {
            Name = "Anti-Key",
            Description = "Sakreyfice",
            LongDescription = "Destabilises all keys, converting any new keys into direct strength\n\nMade out of matter where a key is not at, at any point in time.",
            SpriteResource = "JuneGunJamSubmission/Resources/antikey",
            Quality = ItemQuality.B,
        };

        private int lastKeys = 9999999;
        public override void Update()
        {
            base.Update();
            if (Owner)
            {
                int newKeys = Owner.carriedConsumables.KeyBullets;
                if (newKeys != lastKeys)
                {
                    int diff = newKeys - lastKeys;
                    if (diff > 0)
                    {
                        Owner.carriedConsumables.KeyBullets = lastKeys;
                        for (int i = 0; i < diff; i++)
                        {
                            AddRandomStat();
                        }
                    }
                    lastKeys = newKeys;
                }
            }
        }

        public void AddRandomStat()
        {
            PlayerStats.StatType type = BraveUtility.RandomElement(increments.Keys.ToList());
            this.AddPassiveStatModifier(type, increments[type]);
        }
        public static Dictionary<PlayerStats.StatType, float> increments = new Dictionary<PlayerStats.StatType, float>()
        {
            { PlayerStats.StatType.Damage, 0.05f },
            { PlayerStats.StatType.RateOfFire, 0.15f },
            { PlayerStats.StatType.ReloadSpeed, -0.2f },
        };
    }
}
