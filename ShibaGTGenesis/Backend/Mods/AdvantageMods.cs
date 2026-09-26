using Photon.Pun;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class AdvantageMods
    {
        public static void TagAll()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                {
                    foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
                    {
                        tagman.AddInfectedPlayer(plr);
                    }
                }
            }
            else
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && !rig.photonView.IsMine && !rig.isMyPlayer)
                    {
                        if (GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected") && !rig.mainSkin.material.name.Contains("fected"))
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                            GorillaTagger.Instance.myVRRig.transform.position = rig.headConstraint.transform.position;
                            GorillaTagger.Instance.myVRRig.rightHandTransform.transform.position = rig.headConstraint.transform.position;
                            GorillaTagger.Instance.rightHandTransform.position = rig.headConstraint.transform.position;
                            NotificationManager.SendNotification("<color=blue>[GENESIS]</color> Tagged all!");
                        }
                        else
                        {
                            GorillaTagger.Instance.myVRRig.enabled = true;
                            NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not tagged.");
                        }
                    }
                }
            }
        }
        public static void AntiTag()
        {
            GorillaTagger.Instance.myVRRig.enabled = true;
            if (!GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected"))
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig.mainSkin.material.name.Contains("fected"))
                    {
                        if (Vector3.Distance(rig.transform.position, GorillaTagger.Instance.myVRRig.transform.position) <= 7)
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                            GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.transform.position - new Vector3(0, 7, 0);
                        }
                    }
                }
            }
        }


        public static void TagSelf()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                {
                    tagman.AddInfectedPlayer(PhotonNetwork.LocalPlayer);
                }
            }
            else
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && !rig.photonView.IsMine && !rig.isMyPlayer)
                    {
                        if (!GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected") && rig.mainSkin.material.name.Contains("fected"))
                        {
                            GorillaTagger.Instance.myVRRig.enabled = false;
                            GorillaTagger.Instance.myVRRig.transform.position = rig.headConstraint.transform.position;
                            GorillaTagger.Instance.myVRRig.transform.position = rig.rightHandTransform.position;
                            NotificationManager.SendNotification("<color=blue>[GENESIS]</color> Tagged self!");
                        }
                        else
                        {
                            GorillaTagger.Instance.myVRRig.enabled = true;
                            NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not tagged.");
                        }
                    }
                }
            }
        }

        public static void TagAura()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && !rig.photonView.IsMine && !rig.isMyPlayer)
                {
                    if (GorillaTagger.Instance.myVRRig.mainSkin.material.name.Contains("fected") && !rig.mainSkin.material.name.Contains("fected"))
                    {
                        float dis = Vector3.Distance(Camera.main.transform.position, rig.headConstraint.transform.position);
                        if (dis < 0.75f)
                        {
                            GorillaTagger.Instance.myVRRig.rightHandTransform.transform.position = rig.headConstraint.transform.position;
                            GorillaTagger.Instance.rightHandTransform.position = rig.headConstraint.transform.position;
                        }
                    }
                }
            }
        }

        public static void NoTagFreeze()
        {
            GorillaLocomotion.Player.Instance.disableMovement = false;
        }

        public static void Untagself()
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[UNTAG]</color> Become master");
                return;
            }
            foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
            {
                tagman.currentInfected.Remove(PhotonNetwork.LocalPlayer);
            }
        }

        public static void UntagGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                        {
                            NotificationManager.SendNotification("<color=red>[UNTAG]</color> Become master");
                            return;
                        }
                        foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                        {
                            tagman.currentInfected.Remove(who.photonView.Owner);
                        }
                    }
                }
            }
        }

        public static void UntagAll()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                {
                    foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                    {
                        tagman.currentInfected.Remove(p);
                    }
                }
            }
            else
            {
                NotificationManager.SendNotification("<color=red>[UNTAG]</color> Become master");
                return;
            }
        }
        public static void TagGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        GorillaGameManager.instance.GetComponent<PhotonView>().RPC(
                                "ReportTagRPC",
                                RpcTarget.MasterClient,
                                new Il2CppSystem.Object[] { who.photonView.Owner }
                            );
                    }
                }
            }
        }

        public static void TagGunRPC()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.gunLocked && Menu.Menu.lockTarget != null)
                {
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                    {
                        tagman.AddInfectedPlayer(Menu.Menu.lockTarget.photonView.Owner);
                    }
                }

                if (Menu.Menu.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Menu.Menu.gunLocked = true;
                        Menu.Menu.lockTarget = who;
                    }
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                if (Menu.Menu.gunLocked)
                    Menu.Menu.gunLocked = false;
            }
        }

        public static void FlickTagGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                if (Menu.Menu.GetGunInput(true))
                {
                    GorillaTagger.Instance.rightHandTransform.position = NewPointer.transform.position;
                }
            }
        }
    }
}