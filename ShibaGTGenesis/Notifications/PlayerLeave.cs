using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace ShibaGTGenesis.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), nameof(MonoBehaviourPunCallbacks.OnPlayerLeftRoom))]
    public class LeavePatch
    {
        private static readonly Dictionary<int, float> leaveCooldowns = new();

        private const float CooldownTime = 2f;

        public static void Prefix(Player otherPlayer)
        {
            if (otherPlayer == null)
                return;
            if (PhotonNetwork.LocalPlayer == null)
                return;
            if (otherPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            if (leaveCooldowns.TryGetValue(otherPlayer.ActorNumber, out float lastTime))
            {
                if (Time.time - lastTime < CooldownTime)
                    return;
            }
            leaveCooldowns[otherPlayer.ActorNumber] = Time.time;
            JoinPatch.RemoveCooldown(otherPlayer.ActorNumber);
            NotificationManager.SendNotification($"<color=blue>[ROOM]</color> Player {otherPlayer.NickName} Left Lobby");
        }
    }
}