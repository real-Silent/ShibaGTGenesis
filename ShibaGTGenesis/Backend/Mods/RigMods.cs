using easyInputs;
using Photon.Pun;
using ShibaGTGenesis.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class RigMods
    {
        public static void GhostMonkey()
        {
            if (EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand))
            {
                if (PhotonNetwork.InRoom)
                    GorillaTagger.Instance.myVRRig.enabled = false;
                else
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                if (PhotonNetwork.InRoom)
                    GorillaTagger.Instance.myVRRig.enabled = true;
                else
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void ResetRig()
        {
            if (PhotonNetwork.InRoom)
                GorillaTagger.Instance.myVRRig.enabled = true;
            else
                GorillaTagger.Instance.offlineVRRig.enabled = true;
        }

        public static Photon.Realtime.Player lucyTarget;

        public static float lucyspeed = 0.1f;

        private static bool Spin;
        private static bool Roll;

        public static void HeadSpin()
        {
            Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
            rot.y += 15f;
            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;

            Spin = true;
        }

        public static void OffHeadSpin()
        {
            if (Spin)
            {
                Spin = false;

                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.y = 0f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;
            }
        }

        public static void HeadRoll()
        {
            Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
            rot.x += 15f;
            GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;

            Roll = true;
        }

        public static void SpazMonk()
        {
            System.Random random = new System.Random();
            if (PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
            }
        }

        static bool upside;
        static bool back;

        public static void OFFHeadBackwards()
        {
            if (back)
            {
                back = false;

                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.y = 0f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;
            }
        }

        public static void HeadBackwards()
        {
            if (!back)
            {
                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.y = 180f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;

                back = true;
            }
        }

        public static void HeadUpsidedown()
        {
            if (!upside)
            {
                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.x = 180f;
                rot.y = 180f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;

                upside = true;
            }
        }

        public static void OFFHeadUpsidedown()
        {
            if (upside)
            {
                upside = false;

                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.x = 0f;
                rot.y = 0f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;
            }
        }

        public static void HeadSpaz()
        {
            System.Random random = new System.Random();
            if (PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
            }
        }

        public static void HandSpaz()
        {
            System.Random random = new System.Random();
            if (PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
            }
        }

        public static void OffHeadRoll()
        {
            if (Roll)
            {
                Roll = false;
                Vector3 rot = GorillaTagger.Instance.myVRRig.head.trackingRotationOffset;
                rot.x = 0f;
                GorillaTagger.Instance.myVRRig.head.trackingRotationOffset = rot;
            }
        }


        public static void SpinBot()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position + new Vector3(0, 0.3f, 0);
                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * -1f;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * 1f;
                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
                GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.myVRRig.transform.rotation.eulerAngles + new Vector3(4f, 7f, 0f));
            }
            else {
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }

        public static void Tpose()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;

                GorillaTagger.Instance.myVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position + new Vector3(0, 0.3f, 0);

                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * -1f;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * 1f;

                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
            }
            else
            {
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }

        public static void Helicopter()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;

                GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.bodyCollider.transform.position + new Vector3(0, 0.3f, 0);

                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * -1f;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.myVRRig.transform.position + GorillaTagger.Instance.myVRRig.transform.right * 1f;

                GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
                GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.myVRRig.transform.rotation;
                GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.myVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
            }
            else
            {
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }


        public static void LucyGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    VRRig lucyTargetRig = Menu.Menu.lockTarget;
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position += GorillaTagger.Instance.myVRRig.transform.forward * lucyspeed * Time.deltaTime;
                    GorillaTagger.Instance.myVRRig.transform.LookAt(lucyTargetRig.transform);
                    lucyspeed += 0.005f;
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                    lucyspeed = 0.1f;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void LucyRandom()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                if (lucyTarget == null)
                {
                    lucyTarget = RigManager.GetRandomPlayer(false);
                }
                VRRig lucyTargetRig = RigManager.GetVRRigFromPlayer(lucyTarget);
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position += GorillaTagger.Instance.myVRRig.transform.forward * lucyspeed * Time.deltaTime;
                GorillaTagger.Instance.myVRRig.transform.LookAt(lucyTargetRig.transform);
                lucyspeed += 0.005f;
            }
            else
            {
                GorillaTagger.Instance.myVRRig.enabled = true;
                lucyspeed = 0.1f;
            }
        }


        public static void RigGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (Menu.Menu.GetGunInput(true))
                {
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.enabled = false;
                        GorillaTagger.Instance.myVRRig.transform.position = Pointer.transform.position;
                    }
                    else
                    {
                        GorillaTagger.Instance.offlineVRRig.enabled = false;
                        GorillaTagger.Instance.offlineVRRig.transform.position = Pointer.transform.position;
                    }
                }
                else
                {
                    if (PhotonNetwork.InRoom)
                        GorillaTagger.Instance.myVRRig.enabled = true;
                    else
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
        }

        public static void FollowPlayerGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    Vector3 direction = (GorillaTagger.Instance.myVRRig.transform.position - Menu.Menu.lockTarget.headConstraint.transform.position).normalized;
                    Vector3 newPosition = Menu.Menu.lockTarget.headConstraint.transform.position + direction * 2f;
                    GorillaTagger.Instance.myVRRig.transform.LookAt(Menu.Menu.lockTarget.headConstraint.transform.position);
                    GorillaTagger.Instance.myVRRig.transform.position = newPosition;
                    System.Random random = new System.Random();
                    if (PhotonNetwork.InRoom)
                    {
                        GorillaTagger.Instance.myVRRig.head.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.myVRRig.leftHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
                        GorillaTagger.Instance.myVRRig.rightHand.rigTarget.eulerAngles = new Vector3(random.Next(0, 360), random.Next(0, 360), random.Next(0, 360));
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
                else
                {
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void AnnoyGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = Menu.Menu.lockTarget.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 3f), UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(-2f, 3f));
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void JumpscareGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position += GorillaTagger.Instance.myVRRig.transform.forward * 20f * Time.deltaTime;
                    GorillaTagger.Instance.myVRRig.transform.LookAt(Menu.Menu.lockTarget.transform);
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        private static float angle;
        private static float orbitSpeed = 8f;
        public static void HaloGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    angle += orbitSpeed * Time.deltaTime;
                    float x = Menu.Menu.lockTarget.transform.position.x + 1f * Mathf.Cos(angle);
                    float y = Menu.Menu.lockTarget.transform.position.y + 1f;
                    float z = Menu.Menu.lockTarget.transform.position.z + 1f * Mathf.Sin(angle);
                    Vector3 funny = new Vector3(x, y, z);
                    GorillaTagger.Instance.myVRRig.transform.position = funny;
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void CopyGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = Menu.Menu.lockTarget.transform.position;
                    GorillaTagger.Instance.myVRRig.transform.rotation = Menu.Menu.lockTarget.transform.rotation;
                    GorillaTagger.Instance.myVRRig.rightHandPlayer.transform.position = Menu.Menu.lockTarget.rightHandPlayer.transform.position;
                    GorillaTagger.Instance.myVRRig.rightHandPlayer.transform.rotation = Menu.Menu.lockTarget.rightHandPlayer.transform.rotation;
                    GorillaTagger.Instance.myVRRig.leftHandPlayer.transform.position = Menu.Menu.lockTarget.leftHandPlayer.transform.position;
                    GorillaTagger.Instance.myVRRig.leftHandPlayer.transform.rotation = Menu.Menu.lockTarget.leftHandPlayer.transform.rotation;
                    GorillaTagger.Instance.myVRRig.head.headTransform.transform.rotation = Menu.Menu.lockTarget.head.headTransform.transform.rotation;
                    GorillaTagger.Instance.myVRRig.head.headTransform.transform.position = Menu.Menu.lockTarget.head.headTransform.transform.position;
                    GorillaTagger.Instance.myVRRig.headConstraint.rotation = Menu.Menu.lockTarget.headConstraint.rotation;
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                    GorillaTagger.Instance.myVRRig.headConstraint.rotation = GorillaLocomotion.Player.Instance.headCollider.transform.rotation;

                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void SexGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = Menu.Menu.lockTarget.transform.position + (Menu.Menu.lockTarget.transform.forward * -(0.2f + (Mathf.Sin(Time.frameCount / 8f) * 0.1f)));
                    GorillaTagger.Instance.myVRRig.transform.rotation = Menu.Menu.lockTarget.transform.rotation;
                    GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.position = (Menu.Menu.lockTarget.transform.position + Menu.Menu.lockTarget.transform.right * -0.2f) + Menu.Menu.lockTarget.transform.up * -0.4f;
                    GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.position = (Menu.Menu.lockTarget.transform.position + Menu.Menu.lockTarget.transform.right * 0.2f) + Menu.Menu.lockTarget.transform.up * -0.4f;
                    GorillaTagger.Instance.myVRRig.leftHand.rigTarget.transform.rotation = Menu.Menu.lockTarget.transform.rotation;
                    GorillaTagger.Instance.myVRRig.rightHand.rigTarget.transform.rotation = Menu.Menu.lockTarget.transform.rotation;
                    GorillaTagger.Instance.myVRRig.head.rigTarget.transform.rotation = Menu.Menu.lockTarget.transform.rotation;
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }


        public static List<VRRig> validRigs = new List<VRRig>();
        public static RaycastHit[] rayResults = new RaycastHit[1];
        public static LayerMask layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("GorillaObject"));

        public static float Distance2D(Vector3 a, Vector3 b)
        {
            Vector2 a2 = new Vector2(a.x, a.z);
            Vector2 b2 = new Vector2(b.x, b.z);
            return Vector2.Distance(a2, b2);
        }
        public static bool PlayerNear(VRRig rig, float dist, out float playerDist)
        {
            if (rig == null)
            {
                playerDist = float.PositiveInfinity;
                return false;
            }
            playerDist = Distance2D(rig.transform.position, GorillaTagger.Instance.myVRRig.transform.position);
            return playerDist < dist && Physics.RaycastNonAlloc(new Ray(GorillaTagger.Instance.myVRRig.transform.position, rig.transform.position - GorillaTagger.Instance.myVRRig.transform.position), rayResults, playerDist, layerMask) <= 0;
        }

        public static List<VRRig> GetValidChoosableRigs()
        {
            validRigs.Clear();
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (PhotonNetwork.InRoom && vrrig != null && vrrig != GorillaTagger.Instance.myVRRig)
                {
                    validRigs.Add(vrrig);
                }
            }
            return validRigs;
        }
        public static bool ClosestPlayer(in Vector3 myPos, out VRRig outRig)
        {
            float num = float.MaxValue;
            outRig = null;
            foreach (VRRig vrrig in GetValidChoosableRigs())
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.myVRRig)
                {
                    float num2 = 0f;
                    if (PlayerNear(vrrig, 3f, out num2) && num2 < num)
                    {
                        num = num2;
                        outRig = vrrig;
                    }
                }
            }
            return num != float.MaxValue;
        }

        public static void LookAtClosest()
        {
            VRRig closestRig = GorillaTagger.Instance.myVRRig;
            ClosestPlayer(GorillaTagger.Instance.myVRRig.transform.position, out closestRig);
            GorillaTagger.Instance.myVRRig.headConstraint.LookAt(closestRig.transform.position + new Vector3(0, 0.4f, 0));
        }

        public static void LookAtGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Menu.Menu.lockTarget && Menu.Menu.gunLocked)
                {
                    ClosestPlayer(GorillaTagger.Instance.myVRRig.transform.position, out Menu.Menu.lockTarget);
                    GorillaTagger.Instance.myVRRig.headConstraint.LookAt(Menu.Menu.lockTarget.headMesh.transform.position + new Vector3(0, 0.4f, 0));
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
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
            }
            else
            {
                Menu.Menu.lockTarget = null;
                Menu.Menu.gunLocked = false;
            }
        }

        public static void HoldRig()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.leftHandFollower.position;
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = GorillaLocomotion.Player.Instance.rightHandFollower.position;
            }
            if (!EasyInputs.GetGripButtonDown(EasyHand.RightHand) && !EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }


        public static void InvisMonkey()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                if (PhotonNetwork.InRoom)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                    GorillaTagger.Instance.myVRRig.transform.position = new Vector3(435345f, 3323f, 32432f);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(435345f, 3323f, 32432f);
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                    GorillaTagger.Instance.myVRRig.enabled = true;
                else
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
    }
}