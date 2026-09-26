using easyInputs;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class LegitMods
    {
        public static void SteamArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }

        public static void DisableSteamArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static void HZ()
        {
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            { }
        }

        public static void SlideControl()
        {
            GorillaLocomotion.Player.Instance.slideControl = 1f;
        }

        public static void OFFSlideControl()
        {
            GorillaLocomotion.Player.Instance.slideControl = 0.00425f;
        }

        public static float LagFloat;
        static bool LagBool;
        public static void FakeLag()
        {
            if (LagFloat < Time.time)
            {
                LagFloat = Time.time + 0.2f;
                int randomint = UnityEngine.Random.Range(1, 3);
                if (randomint == 1)
                {
                    GorillaTagger.Instance.myVRRig.enabled = false;
                }
                else
                {
                    GorillaTagger.Instance.myVRRig.enabled = true;
                }
                LagBool = true;
            }
        }


        private static GameObject leftController;
        private static GameObject rightController;
        public static void FakeOculus()
        {
            if (leftController == null)
                leftController = GameObject.Find("LeftHand Controller");
            if (rightController == null)
                rightController = GameObject.Find("RightHand Controller");
            if (leftController != null)
                leftController.SetActive(!EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand));
            if (rightController != null)
                rightController.SetActive(!EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand));
        }


        // Taking this from qolossal - nova
        static Vector3 normal2;
        static Vector3 vel1;
        static Vector3 vel2;
        static float dist2;
        static int layers;
        static bool LeftClose2;
        static bool DoOnce2;
        static float maxD2;
        public static void WallWalk()
        {
            if (GorillaLocomotion.Player.Instance == null)
                return;
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                if (!DoOnce2)
                {
                    maxD2 = 1f;
                    layers = int.MaxValue;
                    DoOnce2 = true;
                }
                RaycastHit raycastHit;
                Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, -GorillaTagger.Instance.rightHandTransform.right, out raycastHit, 1f, layers);
                RaycastHit raycastHit2;
                Physics.Raycast(GorillaTagger.Instance.leftHandTransform.position, GorillaTagger.Instance.leftHandTransform.right, out raycastHit2, 1f, layers);
                if (raycastHit2.distance > raycastHit.distance)
                {
                    normal2 = raycastHit.normal;
                    dist2 = raycastHit.distance;
                }
                else
                {
                    normal2 = raycastHit2.normal;
                    dist2 = raycastHit2.distance;
                    LeftClose2 = true;
                }
                if (dist2 < maxD2)
                {
                    vel2 = normal2 * (7.5f * Time.deltaTime);
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity -= vel2;
                }
                else
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                }
            }
            else
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
            }
        }
    }
}