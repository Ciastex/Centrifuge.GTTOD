using Reactor.API.Runtime.Patching;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace Centrifuge.GTTOD.Transpilers
{
    internal static partial class Game
    {
        private class StartGame : GameCodeTranspiler
        {
            private const int EventHookOpCodeIndex = 297;
            private const string StartGameCoroutineClassName = "<StartSceneFade>d__69";

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
            {
                var modified = new List<CodeInstruction>(instr);
                    
                var invoker = typeof(Events.Game).GetMethod(
                    nameof(Events.Game.InvokeGameModeStarted),
                    BindingFlags.NonPublic | BindingFlags.Static
                );

                modified.Insert(EventHookOpCodeIndex, new CodeInstruction(OpCodes.Call, invoker));

                return modified;
            }

            public override void Apply(Harmony harmony)
            {
                var targetMethod = typeof(global::GTTODManager).GetNestedType(
                    StartGameCoroutineClassName,
                    BindingFlags.NonPublic
                ).GetMethod(
                    "MoveNext",
                    BindingFlags.NonPublic | BindingFlags.Instance
                );

                var transpilerMethod = typeof(StartGame).GetMethod(
                    nameof(Transpiler),
                    BindingFlags.NonPublic | BindingFlags.Static
                );

                harmony.Patch(targetMethod, transpiler: new HarmonyMethod(transpilerMethod));
            }
        }
    }
}
