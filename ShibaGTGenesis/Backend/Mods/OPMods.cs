using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class OPMods
    {
        private static bool antibanfinished = false;
        public static void AntiBan()
        {
            if (PhotonNetwork.InRoom)
            {
                Hashtable hash = new Hashtable();
                hash.Add("gameMode", "MODDED");
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
                GorillaNot.instance.reportedPlayers.Add(PhotonNetwork.LocalPlayer.UserId);
                GorillaNot.instance.rpcErrorMax = int.MaxValue;
                GorillaNot.instance.rpcCallLimit = int.MaxValue;
                GorillaNot.instance.logErrorMax = int.MaxValue;
                antibanfinished = true;
                NotificationManager.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban enabled!");
            }
            else
            {
                antibanfinished = false;
            }
        }

        public static void ClearPrefabs()
        {
            SetMaster();
            PhotonNetwork.DestroyAll();
        }

        public static void DisableNetworkTriggersSS()
        {
            SetMaster();
            Hashtable hash = new Hashtable();
            hash.Add("gameMode", "forestcitycanyonscavesmountainsDEFAULTFORESTINFECTIONINFECTION");
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public static void ChangeNameGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    ChangePlrName(Menu.Menu.lockTarget.photonView.Owner, "<color=blue>SHIBAGT GENESIS ON TOP BY NOVA</color>\ndiscord.gg/dtQdz59FJG");
                }

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        Menu.Menu.lockTarget = rig;
                        Menu.Menu.gunLocked = true;
                    }
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void ChangeNameAll()
        {
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                ChangePlrName(p, "<color=blue>SHIBAGT GENESIS ON TOP BY NOVA</color>\ndiscord.gg/dtQdz59FJG");
            }
        }

        public static void ChangePlrName(Photon.Realtime.Player plr, string name)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            plr.NickName = name;
            Type typetyetpy = typeof(Photon.Realtime.Player);
            MethodInfo meto = typetyetpy.GetMethod("SetPlayerNameProperty", BindingFlags.Instance | BindingFlags.Public);
            if (meto != null)
            {
                meto.Invoke(plr, new object[0]);
            }
        }

        public static void ChangeGamemode(string gamemode)
        {
            SetMaster();
            Hashtable gamemodehash = new Hashtable();
            gamemodehash.Add("gameMode", gamemode);
            PhotonNetwork.CurrentRoom.SetCustomProperties(gamemodehash);
            if (gamemode == "HUNT")
            {
                PhotonNetwork.Instantiate("Gorilla Hunt Manager", new Vector3(0, 0, 0), Quaternion.identity);
            }
            if (gamemode == "BATTLE")
            {
                PhotonNetwork.Instantiate("Gorilla Battle Manager", new Vector3(0, 0, 0), Quaternion.identity);
            }
        }

        public static void BreakGamemode()
        {
            SetMaster();
            GameObject.FindObjectsOfType<GorillaTagManager>().FirstOrDefault().ClearInfectionState();
        }

        public static void MatSpamGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                    {
                        if (tagman.currentInfected.Contains(Menu.Menu.lockTarget.photonView.Owner))
                        {
                            tagman.currentInfected.Remove(Menu.Menu.lockTarget.photonView.Owner);
                            tagman.UpdateInfectionState();
                        }
                        tagman.EndInfectionGame();
                        tagman.AddInfectedPlayer(Menu.Menu.lockTarget.photonView.Owner);
                        tagman.UpdateInfectionState();
                    }
                }

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        Menu.Menu.lockTarget = rig;
                        Menu.Menu.gunLocked = true;
                    }
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void MatSpamAll()
        {
            SetMaster();
            foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        if (tagman.currentInfected.Contains(rig.photonView.Owner))
                        {
                            tagman.currentInfected.Remove(rig.photonView.Owner);
                            tagman.UpdateInfectionState();
                        }
                        tagman.EndInfectionGame();
                        tagman.AddInfectedPlayer(rig.photonView.Owner);
                        tagman.UpdateInfectionState();
                    }
                }
            }
        }

        public static void AntiBanStatus()
        {
            if (antibanfinished)
            {
                NotificationManager.SendNotification("<color=green>[ANTIBAN STATUS]</color> Antiban is enabled, and you're master!");
            }
            else
            {
                NotificationManager.SendNotification("<color=red>[ANTIBAN STATUS]</color> Antiban is not enabled, and you're not master!");
            }
        }

        public static void SetMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            //NotificationManager.SendNotification("<color=red>[SET MASTER]</color> Set Master enabled!");
        }
        public static void AutoSetMaster()
        {
            if (antibanfinished)
                SetMaster();
            else
            {
                AntiBan();
                SetMaster();
            }
        }

        public static void CrashGun()
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
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                        FLushRPCS();
                    }
                }
            }
        }

        public static void LagAura()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 them = rig.headMesh.transform.position;
                    if (Vector3.Distance(them, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 3.75f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", rig.photonView.Owner, null);
                            GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", rig.photonView.Owner, null);
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }

        public static void CrashAura()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 them = rig.headMesh.transform.position;
                    if (Vector3.Distance(them, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 3.75f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }

        public static void LagOnTouch()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 rhand = rig.rightHandTransform.position;
                    Vector3 lhand = rig.leftHandTransform.position;
                    if (Vector3.Distance(rhand, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", rig.photonView.Owner, null);
                            GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", rig.photonView.Owner, null);
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    if (Vector3.Distance(lhand, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", rig.photonView.Owner, null);
                            GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", rig.photonView.Owner, null);
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }

        public static void CrashOnTouch()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 rhand = rig.rightHandTransform.position;
                    Vector3 lhand = rig.leftHandTransform.position;
                    if (Vector3.Distance(rhand, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    if (Vector3.Distance(lhand, GorillaTagger.Instance.myVRRig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }
        public static void BanAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    BetaBanAll(rig.photonView.Owner.UserId);
                }
            }
        }

        public static void BanGun()
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
                        BetaBanAll(rig.photonView.Owner.UserId);
                    }
                }
            }
        }

        public static void BetaBanAll(string userid)
        {
            /*WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            string url = "https://api-nova-two.vercel.app/banusingcloudscript";
            string useragent = "banneratqolossallol";
            string titleId = PlayFabSettings.TitleId;
            client.Headers.Add("User-Agent", useragent);
            string playerId = userid;
            var payload = new
            {
                titleId = titleId,
                playerId = userid
            };
            string json = JsonConvert.SerializeObject(payload);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] response = client.UploadData(url, "POST", data);
            string responseString = Encoding.UTF8.GetString(response);
            NotificationManager.SendNotification($"<color=cyan>[INFO]</color> Success {responseString}");
            client.Dispose();*/
        }

        public static void FLushRPCS()
        {
            GorillaNot.instance.rpcErrorMax = int.MaxValue;
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.logErrorMax = int.MaxValue;

            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;

            PhotonNetwork.SendAllOutgoingCommands();
        }

        public static void LagOnYouTouch()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 rhand = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 lhand = GorillaTagger.Instance.leftHandTransform.position;
                    if (Vector3.Distance(rhand, rig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", rig.photonView.Owner, null);
                            GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", rig.photonView.Owner, null);
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    if (Vector3.Distance(lhand, rig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", rig.photonView.Owner, null);
                            GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", rig.photonView.Owner, null);
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }

        public static void CrashOnYouTouch()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    Vector3 rhand = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 lhand = GorillaTagger.Instance.leftHandTransform.position;
                    if (Vector3.Distance(rhand, rig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    if (Vector3.Distance(lhand, rig.headMesh.transform.position) <= 0.55f)
                    {
                        for (int i = 0; i < 150; i++)
                        {
                            PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            PhotonNetwork.RaiseEvent(3, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                        }
                    }
                    FLushRPCS();
                }
            }
        }

        public static void CrashAll()
        {
            for (int i = 0; i < 150; i++)
            {
                PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
            }
            PhotonNetwork.SendAllOutgoingCommands();
        }

        public static void LagAll()
        {
            foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
            {
                for (int i = 0; i < 150; i++)
                {
                    GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", plr, null);
                    GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", plr, null);
                    PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others }, SendOptions.SendUnreliable);
                }
                FLushRPCS();
            }
        }

        public static void LagGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        GorillaTagger.Instance.myVRRig.photonView.RPC("UpdatePlayerCosmetic", Menu.Menu.lockTarget.photonView.Owner, null);
                        GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", Menu.Menu.lockTarget.photonView.Owner, null);
                        PhotonNetwork.RaiseEvent(2, null, new Photon.Realtime.RaiseEventOptions { TargetActors = new int[] { Menu.Menu.lockTarget.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                    }
                    FLushRPCS();
                }

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        Menu.Menu.lockTarget = rig;
                        Menu.Menu.gunLocked = true;
                    }
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }
    }
}