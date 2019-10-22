using Harmony;
using Centrifuge.GTTOD.Events;
using Centrifuge.GTTOD.Events.Args;

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
}
