using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace ShibaGTGenesis.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    public class JoinPatch : MonoBehaviour
    {
        private static readonly Dictionary<int, float> JoinCooldowns = new();
        private const float CooldownTime = 5f;
        public static void Prefix(Player newPlayer)
        {
            if (newPlayer == null)
                return;
            if (PhotonNetwork.LocalPlayer != null &&
                newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                return;
            int actor = newPlayer.ActorNumber;
            float time = Time.time;
            if (JoinCooldowns.TryGetValue(actor, out float lastTime))
            {
                if (time - lastTime < CooldownTime)
                    return;
            }
            JoinCooldowns[actor] = time;
            NotificationManager.SendNotification($"<color=blue>[ROOM]</color> Player {newPlayer.NickName} Joined Lobby");
        }
        public static void RemoveCooldown(int actor)
        {
            if (JoinCooldowns.ContainsKey(actor))
                JoinCooldowns.Remove(actor);
        }
    }
}