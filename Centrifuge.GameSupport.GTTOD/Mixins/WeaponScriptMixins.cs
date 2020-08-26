using Centrifuge.GTTOD.Events;
using Centrifuge.GTTOD.Events.Args;
using HarmonyLib;

namespace Centrifuge.GTTOD.Mixins
{
    [HarmonyPatch(typeof(WeaponScript), "Awake")]
    internal class WeaponScriptAwakeMixins
    {
        public static bool Prefix(WeaponScript __instance)
        {
            var eventArgs = new MethodPreviewEventArgs<WeaponScript>(__instance);
            Weapon.InvokePreviewAwake(eventArgs);

            return !eventArgs.Cancel;
        }

        public static void Postfix(WeaponScript __instance)
        {
            var eventArgs = new TypeInstanceEventArgs<WeaponScript>(__instance);
            Weapon.InvokeAwakeComplete(eventArgs);
        }
    }

    [HarmonyPatch(typeof(WeaponScript), "NewPrimaryFire")]
    internal class WeaponScriptNewPrimaryFireMixins
    {
        public static bool Prefix(WeaponScript __instance, int BurstCount)
        {
            var eventArgs = new MethodPreviewEventArgs<WeaponScript>(__instance);
            eventArgs.AdditionalData[nameof(BurstCount)] = BurstCount;

            Weapon.InvokePreviewShotFiredPrimary(eventArgs);

            return !eventArgs.Cancel;
        }

        public static void Postfix(WeaponScript __instance, int BurstCount)
        {
            Weapon.InvokeShotFiredPrimary(__instance, BurstCount);
        }
    }

    [HarmonyPatch(typeof(WeaponScript), "NewSecondaryFire")]
    internal class WeaponScriptNewSecondaryFireMixins
    {
        public static bool Prefix(WeaponScript __instance, int BurstCount)
        {
            var eventArgs = new MethodPreviewEventArgs<WeaponScript>(__instance);
            eventArgs.AdditionalData[nameof(BurstCount)] = BurstCount;

            Weapon.InvokePreviewShotFiredSecondary(eventArgs);

            return !eventArgs.Cancel;
        }

        public static void Postfix(WeaponScript __instance, int BurstCount)
        {
            Weapon.InvokeShotFiredSecondary(__instance, BurstCount);
        }
    }
}
