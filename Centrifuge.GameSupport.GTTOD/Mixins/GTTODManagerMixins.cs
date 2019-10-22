using Harmony;

namespace Centrifuge.GTTOD.Mixins
{
    [HarmonyPatch(typeof(GTTODManager), nameof(GTTODManager.PlayerDeath))]
    internal class PlayerDeathMixin
    {
        public static void Postfix()
            => Events.Player.InvokeDied();
    }
}
