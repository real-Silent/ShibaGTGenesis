using UnityEngine;
using HarmonyLib;
using GorillaNetworking;

namespace ShibaGTGenesis.Patches
{
    [HarmonyPatch(typeof(GorillaQuitBox), "OnBoxTriggered")]
    internal class QuitboxPatch : MonoBehaviour
    {
        public static bool Prefix()
        {
            if (WorldMods.disableQuitbox)
                return false;
            if (WorldMods.QuitBoxMOD)
            {
                GorillaTagger.Instance.transform.position = GorillaComputer.instance.friendJoinCollider.transform.position;
                return false;
            }
            return true;
        }
    }
}