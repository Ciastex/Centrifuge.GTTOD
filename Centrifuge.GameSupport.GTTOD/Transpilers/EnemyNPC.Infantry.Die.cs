using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Reactor.API.Runtime.Patching;

namespace Centrifuge.GTTOD.Transpilers
{
    internal static partial class EnemyNPC
    {
        private class InfantryDie : GameCodeTranspiler
        {
            private const int EventHookOpIndex = 81;

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
            {
                var modified = new List<CodeInstruction>(instr);

                var invoker = typeof(Events.EnemyNPC).GetMethod(
                    nameof(Events.EnemyNPC.InvokeInfantryDied),
                    BindingFlags.NonPublic | BindingFlags.Static
                );

                modified.Insert(EventHookOpIndex, new CodeInstruction(OpCodes.Call, invoker));
                modified.Insert(EventHookOpIndex, new CodeInstruction(OpCodes.Ldarg_0));

                return modified;
            }

            public override void Apply(Harmony harmony)
            {
                var targetMethod = typeof(Infantry).GetMethod(
                    nameof(Infantry.Die),
                    BindingFlags.Public | BindingFlags.Instance
                );

                var transpilerMethod = typeof(InfantryDie).GetMethod(
                    nameof(Transpiler),
                    BindingFlags.NonPublic | BindingFlags.Static
                );

                harmony.Patch(targetMethod, transpiler: new HarmonyMethod(transpilerMethod));
            }
        }
    }
}
