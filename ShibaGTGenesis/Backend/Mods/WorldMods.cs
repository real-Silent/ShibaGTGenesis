using easyInputs;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class WorldMods
    {
        public static void GravitySwitcher()
        {
            //1 nothing cuz its normal
            if (SwitcherInt == 2)
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (19.62f / Time.deltaTime)), ForceMode.Acceleration);
            if (SwitcherInt == 3)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.left * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
            }
            if (SwitcherInt == 4)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.right * (Time.deltaTime * (14.81f / Time.deltaTime)), ForceMode.Acceleration);
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
            }

            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand) && Time.time > GravitySwitcherDelay + 0.5f)
            {
                GravitySwitcherDelay = Time.time;
                SwitcherInt++;
                if (SwitcherInt == 1)
                    NotificationManager.SendNotification("Set Gravity To: Normal");
                if (SwitcherInt == 2)
                    NotificationManager.SendNotification("Set Gravity To: Up");
                if (SwitcherInt == 3)
                    NotificationManager.SendNotification("Set Gravity To: Left");
                if (SwitcherInt == 4)
                    NotificationManager.SendNotification("Set Gravity To: Right");
                if (SwitcherInt == 5)
                {
                    SwitcherInt = 1;
                    NotificationManager.SendNotification("Set Gravity To: Normal");
                }
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand) && Time.time > GravitySwitcherDelay + 0.5f)
            {
                GravitySwitcherDelay = Time.time;
                SwitcherInt--;
                if (SwitcherInt == 0)
                    SwitcherInt = 4;
                if (SwitcherInt == 1)
                    NotificationManager.SendNotification("Set Gravity To: Normal");
                if (SwitcherInt == 2)
                    NotificationManager.SendNotification("Set Gravity To: Up");
                if (SwitcherInt == 3)
                    NotificationManager.SendNotification("Set Gravity To: Left");
                if (SwitcherInt == 4)
                    NotificationManager.SendNotification("Set Gravity To: Right");
                if (SwitcherInt == 5)
                {
                    SwitcherInt = 1;
                    NotificationManager.SendNotification("Set Gravity To: Normal");
                }
            }
        }

        public static bool EnemyAIBlocker;

        public static bool disableQuitbox;

        public static void DisableQuitbox()
        {
            disableQuitbox = true;
        }

        public static bool QuitBoxMOD;

        public static void OnQuitbox()
        {
            disableQuitbox = false;
        }

        public static void QuitboxMod()
        {
            QuitBoxMOD = true;
        }

        public static void OffQuitboxMod()
        {
            QuitBoxMOD = false;
        }
        public static void Lcubebending()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                PhotonNetwork.Instantiate("bulletPrefab", GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.rotation);
            }
        }
        public static void Rcubebending()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                PhotonNetwork.Instantiate("bulletPrefab", GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation);
            }
        }

        public static void CubePlayerAura()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        if (Vector3.Distance(rig.headMesh.transform.position, GorillaTagger.Instance.offlineVRRig.transform.position) <= 8)
                        {
                            PhotonNetwork.Instantiate("bulletPrefab", rig.headMesh.transform.position, UnityEngine.Random.rotation);
                        }
                    }
                }
            }
        }

        private static void CreatePrefab(string pfname, Vector3 pos, Quaternion rot)
        {
            PhotonNetwork.Instantiate(pfname, pos, rot);
        }

        public static void CubeSpam()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                CreatePrefab("bulletPrefab", GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation);
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                CreatePrefab("bulletPrefab", GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.rotation);
            }
        }

        public static void CubeGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (Menu.Menu.GetGunInput(true))
                {
                    CreatePrefab("bulletPrefab", Pointer.transform.position, Pointer.transform.rotation);
                }
            }
        }

        public static void TargetSpam()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                CreatePrefab("STICKABLE TARGET", GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.rotation);
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                CreatePrefab("STICKABLE TARGET", GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.rotation);
            }
        }

        public static void TargetGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (Menu.Menu.GetGunInput(true))
                {
                    CreatePrefab("STICKABLE TARGET", Pointer.transform.position, Pointer.transform.rotation);
                }
            }
        }

        public static void CubeAura()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                PhotonNetwork.Instantiate("bulletPrefab", GorillaTagger.Instance.headCollider.transform.position, UnityEngine.Random.rotation);
            }
        }

        public static void BlindAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    PhotonNetwork.Instantiate("STICKABLE TARGET", rig.headMesh.transform.position, Quaternion.identity);
            }
        }

        public static void BlindGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    PhotonNetwork.Instantiate("STICKABLE TARGET", Menu.Menu.lockTarget.headMesh.transform.position, Quaternion.identity);
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
                else
                {
                    Menu.Menu.lockTarget = null;
                    Menu.Menu.gunLocked = false;
                }
            }
        }

        public static float GravityWindDelay;
        public static float GravitySwitcherDelay;
        public static int WindInt;
        public static int SwitcherInt;

        public static void GravityWind()
        {
            if (WindInt == 2)
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (19.62f / Time.deltaTime)), ForceMode.Acceleration);
            if (WindInt == 3)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.left * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
            }
            if (WindInt == 4)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.right * (Time.deltaTime * (14.81f / Time.deltaTime)), ForceMode.Acceleration);
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
            }

            if (Time.time > GravityWindDelay + 4f)
            {
                GravityWindDelay = Time.time;
                int RandomInt = Random.Range(1, 5);
                WindInt = RandomInt;
                if (WindInt == 1)
                    NotificationManager.SendNotification("Set Gravity To: Normal");
                if (WindInt == 2)
                    NotificationManager.SendNotification("Set Gravity To: Up");
                if (WindInt == 3)
                    NotificationManager.SendNotification("Set Gravity To: Left");
                if (WindInt == 4)
                    NotificationManager.SendNotification("Set Gravity To: Right");
            }
        }


        public static void ReverseGravity()
        {
            GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (19.62f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void LowGravity()
        {
            GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (6.66f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void NoGravity()
        {
            GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void HighGravity()
        {
            GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * (Time.deltaTime * (6.66f / Time.deltaTime)), ForceMode.Acceleration);
        }

        public static void SpawnLucy(Color color)
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
            hgc.isSummoned = true;
            hgc.summonedColor = color;
            hgc.defaultColor = color;
            hgc.UpdateState();*/
        }

        public static void DespawnLucy()
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentState = HalloweenGhostChaser.ChaseState.Dormant;
            hgc.isSummoned = false;
            hgc.UpdateState();*/
        }
        public static void FastLucy()
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentSpeed = 25;*/
        }

        public static void SlowLucy()
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentSpeed = 5;*/
        }

        public static void FixLucy()
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentSpeed = 1;*/
        }

        public static void LucyGrabGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                    if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                        return;
                    }
                    hgc.targetPlayer = RigManager.GetPlayerFromVRRig(Menu.Menu.lockTarget);
                    hgc.followTarget = Menu.Menu.lockTarget.head.rigTarget;
                    hgc.grabSpeed = 40;
                    hgc.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                    hgc.UpdateState();
                    hgc.grabbedPlayer = RigManager.GetPlayerFromVRRig(Menu.Menu.lockTarget);
                    hgc.grabTime *= 5;*/
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

        public static void LucyGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;

                if (Menu.Menu.GetGunInput(true))
                {
                    /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
                    if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                        return;
                    }
                    hgc.currentState = HalloweenGhostChaser.ChaseState.InitialRise;
                    hgc.isSummoned = true;
                    hgc.UpdateState();
                    hgc.transform.position = Pointer.transform.position;*/
                }
            }
        }

        public static void BecomeLucy()
        {
            /*HalloweenGhostChaser hgc = GameObject.Find("Global/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Become master");
                return;
            }
            hgc.currentState = HalloweenGhostChaser.ChaseState.InitialRise;
            hgc.isSummoned = true;
            hgc.UpdateState();
            hgc.transform.position = GorillaTagger.Instance.myVRRig.transform.position;
            hgc.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;*/
        }
    }
}