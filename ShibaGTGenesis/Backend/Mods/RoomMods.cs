using easyInputs;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class RoomMods
    {
        public static void JoinGenesis()
        {
            Menu.Menu.Instance.Controller().AttemptToJoinSpecificRoom("GENESIS");
        }

        public static void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public static void BDisconnect()
        {
            if (EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand))
            {
                PhotonNetwork.Disconnect();
            }
        }
        public static void CreatePublicRoom()
        {
            Hashtable dictionaryEntries = new Hashtable();
            dictionaryEntries.Add("gameMode", "INFECTION");
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 10,
                SuppressPlayerInfo = true,
                CustomRoomProperties = dictionaryEntries,
                PublishUserId = true,
                IsOpen = true,
                IsVisible = true
            };
            PhotonNetwork.CreateRoom("HANJ", roomOptions);
        }
        public static void RejoinLastRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
            }
            Menu.Menu.Instance.Controller().AttemptToJoinSpecificRoom(Menu.Menu.Instance.Lastroom);
        }
        public static void Serverhop()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
            }
            PhotonNetwork.JoinRandomRoom();
        }

        public static void MuteGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        rig.muted = true;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().muteButton.enabled = true;
                    }
                }
            }
        }

        public static void MuteAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    rig.muted = true;
                    GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                    GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().muteButton.enabled = true;
                }
            }
        }

        public static void UnmuteGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        rig.muted = false;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().muteButton.enabled = false;
                    }
                }
            }
        }

        public static void UnmuteAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    rig.muted = false;
                    GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                    GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().Where(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).FirstOrDefault().muteButton.enabled = false;
                }
            }
        }

        public static void Rejoin()
        {
            Menu.Menu.Instance.Controller().OnDisconnected(DisconnectCause.DisconnectByClientLogic);
            Menu.Menu.Instance.rejoinCode = PhotonNetwork.CurrentRoom.Name;
            PhotonNetwork.Disconnect();
        }

        public static void CancelRejoin()
        {
            Menu.Menu.Instance.rejoinCode = null;
        }

        private static GameObject cachedTriggers;
        private static GameObject GetNetworkingTriggers()
        {
            if (cachedTriggers == null)
            {
                cachedTriggers = GameObject.Find("NetworkTriggers");
            }
            return cachedTriggers;
        }

        public static void DisableNetworkTriggers()
        {
            GameObject triggers = GetNetworkingTriggers();
            if (triggers != null)
            {
                triggers.SetActive(false);
            }
        }

        public static void EnableNetworkTriggers()
        {
            GameObject triggers = GetNetworkingTriggers();
            if (triggers != null)
            {
                triggers.SetActive(true);
            }
        }

        public static void VisibleTrigs()
        {
            GameObject triggers = GetNetworkingTriggers();
            if (triggers == null)
                return;
            foreach (Renderer g in triggers.GetComponentsInChildren<Renderer>(true))
            {
                g.material.shader = Shader.Find("Standard");
                g.enabled = true;
            }
        }
        public static void NonVisTrigs()
        {
            GameObject triggers = GetNetworkingTriggers();
            if (triggers == null)
                return;
            foreach (Renderer g in triggers.GetComponentsInChildren<Renderer>(true))
            {
                g.enabled = false;
            }
        }


        public static void AntiReport()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        foreach (GorillaPlayerScoreboardLine line in GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>())
                        {
                            if (line.linePlayer.UserId == rig.photonView.Owner.UserId)
                            {
                                Transform reportButton = line.reportButton.transform;
                                float distanceR = Vector3.Distance(reportButton.position, rig.rightHandTransform.position);
                                float distanceL = Vector3.Distance(reportButton.position, rig.leftHandTransform.position);
                                if (distanceR > 0.35f || distanceL > 0.35f)
                                {
                                    PhotonNetwork.Disconnect();
                                    NotificationManager.SendNotification($"<color=red>[AntiReport]</color>  The player {rig.photonView.Owner.NickName} almost reported you, but dont worry ShibaGT Genesis's antireport made u leave they could report u!");
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AntiModerator()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    if (rig.concatStringOfCosmeticsAllowed.Contains("LBAAK"))
                    {
                        PhotonNetwork.Disconnect();
                        NotificationManager.SendNotification("<color=red>[ANTI-MODERATOR]</color> Someone with a STICK joined, disconnected.");
                    }
                }
            }
        }

        public static void HideNameOnLeaderboard()
        {
            if (PhotonNetwork.InRoom)
            {
                GorillaComputer.instance.savedName = "GORILLA" + UnityEngine.Random.Range(0000, 9999);
                GorillaComputer.instance.currentName = "GORILLA" + UnityEngine.Random.Range(0000, 9999);
                foreach (GorillaScoreBoard line in GameObject.FindObjectsOfType<GorillaScoreBoard>())
                {
                    line.RedrawPlayerLines();
                }
            }
        }

        public static void GenesisName()
        {
            PhotonNetwork.LocalPlayer.NickName = "<color=#3B82F6>S</color><color=#4B89F2>H</color><color=#5B90EE>I</color><color=#6B97EA>B</color><color=#7B9EE6>A</color><color=#8BA5E2>G</color><color=#9CA3AF>T GENESIS</color> " 
            + "<color=#60A5FA>BY NOVA</color>\n" + "discord.gg/dtQdz59FJG";
        }

        public static void FlashGenesisName()
        {
            string[] colors = { "red", "cyan", "grey", "yellow", "black" };
            PhotonNetwork.LocalPlayer.NickName = $"<color={colors[UnityEngine.Random.Range(0, colors.Length)]}>SHIBAGT GENESIS</color> <color=#60A5FA>BY NOVA</color>\n" + "discord.gg/dtQdz59FJG";
        }
    }
}