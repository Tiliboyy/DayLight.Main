#region

using DayLight.Stat.Stats.Components;
using Footprinting;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items.Usables.Scp330;
using JetBrains.Annotations;
using Utils;

#endregion

namespace DayLight.Stat.Stats;

public class Patches
{
    [HarmonyPatch]
    public static class CandyExplosionPatch
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CandyPink), nameof(CandyPink.ServerApplyEffects))]
        public static bool ServerApplyEffectsPatch(CandyPink __instance, ReferenceHub hub)
        {
            ServerExplode(hub);
            return false;
        }

        public static void ServerExplode(ReferenceHub hub)
        {
            if (!InventoryItemLoader.TryGetItem<ThrowableItem>(ItemType.GrenadeHE, out var result) || !(result.Projectile is ExplosionGrenade projectile))
                return;
            projectile.gameObject.AddComponent<PinkCandyComponent>();

            ExplosionUtils.ServerSpawnEffect(hub.transform.position, ItemType.GrenadeHE);
            ExplosionGrenade.Explode(new Footprint(hub), hub.transform.position, projectile);
        }
    }
}
