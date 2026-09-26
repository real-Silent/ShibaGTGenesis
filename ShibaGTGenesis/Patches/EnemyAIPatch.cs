using HarmonyLib;

namespace ShibaGTGenesis.Patches
{
    [HarmonyPatch(typeof(GorillaEnemyAI), "Start")]
    internal class EnemyAIPatch
    {
        static void Prefix(GorillaEnemyAI __instance)
        {
            if (WorldMods.EnemyAIBlocker)
            {
                __instance.enabled = false;
            }
        }
    }
}
