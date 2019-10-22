using Harmony;

namespace Centrifuge.GTTOD.Infrastructure
{
    public abstract class GameCodeTranspiler
    {
        public abstract void Apply(HarmonyInstance harmony);
    }
}
