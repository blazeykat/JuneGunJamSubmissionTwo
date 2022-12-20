using JuneLib.Items;
using JuneLib.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JuneGunJamSubmission
{
    public class BloodsplatBlindfoldItem : PassiveItem
    {
        public static ItemTemplate template = new ItemTemplate(typeof(BloodsplatBlindfoldItem))
        {
            Name = "A blindfold",
            Description = "Say... \"Shuzo Masima\"",
            LongDescription = "Damaging non-boss gundead has a chance to instantly kill them, exploding into a geyser of blood.",
            SpriteResource = "JuneGunJamSubmission/Resources/bloodsplatblindfold",
            Quality = ItemQuality.B,
        };

        public override void Pickup(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage += OnDamagedEnemy;
            base.Pickup(player);
        }
        public override void DisableEffect(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage -= OnDamagedEnemy;
            base.DisableEffect(player);
        }

        private void OnDamagedEnemy(float arg1, bool arg2, HealthHaver arg3)
        {
            if (!arg2 && UnityEngine.Random.value < 0.04f
                && arg3 && arg3.specRigidbody && !arg3.IsBoss && arg3.aiActor && arg3.aiActor.IsNormalEnemy)
            {

                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(EasyGoopDefinitions.BlobulonGoopDef).AddGoopCircle(arg3.specRigidbody.UnitCenter, 4);

                PixelCollider pixelCollider = arg3.specRigidbody.HitboxPixelCollider;
                Vector3 vector = pixelCollider.UnitBottomLeft.ToVector3ZisY(0f);
                Vector3 vector2 = pixelCollider.UnitTopRight.ToVector3ZisY(0f);
                GlobalSparksDoer.DoRandomParticleBurst(UnityEngine.Random.Range(8, 12), vector, vector2, Vector3.down, 90f, 0.5f, systemType: GlobalSparksDoer.SparksType.BLOODY_BLOOD);

                arg3.ApplyDamage(2761616f, Vector2.zero, "Bang!");
            }
        }
    }
}
