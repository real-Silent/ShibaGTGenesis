using easyInputs;
using MelonLoader.ICSharpCode.SharpZipLib.GZip;
using Photon.Pun;
using ShibaGTGenesis.Classes;
using ShibaGTGenesis.Menu;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class PlayerMovement
    {
        private static bool SpeedBoostBool = false;
        public static void SpeedBoost()
        {
            if (!SpeedBoostBool)
            {
                foreach (GorillaSurfaceOverride surfascdse in Resources.FindObjectsOfTypeAll<GorillaSurfaceOverride>())
                {
                    if (Menu.Menu.Speed == 0)
                    {
                        surfascdse.extraVelMaxMultiplier = 1.2f;
                        surfascdse.extraVelMultiplier = 1.1f;
                    }
                    else if (Menu.Menu.Speed == 1)
                    {
                        surfascdse.extraVelMaxMultiplier = 1.5f;
                        surfascdse.extraVelMultiplier = 1.4f;
                    }
                    else if (Menu.Menu.Speed == 2)
                    {
                        surfascdse.extraVelMaxMultiplier = 10f;
                        surfascdse.extraVelMultiplier = 10f;
                    }
                }
                SpeedBoostBool = true;
            }
        }

        public static void DisableSpeedBoost()
        {
            if (SpeedBoostBool)
            {
                foreach (GorillaSurfaceOverride surfascdse in Resources.FindObjectsOfTypeAll<GorillaSurfaceOverride>())
                {
                    surfascdse.extraVelMaxMultiplier = 1f;
                    surfascdse.extraVelMultiplier = 1f;
                }
                SpeedBoostBool = false;
            }
        }
        public static void ReallyArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        public static void ResetArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static void Fly()
        {
            if (EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime) * 17f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void IronMonke()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(15f * -GorillaLocomotion.Player.Instance.leftHandTransform.right.x, 15f * -GorillaLocomotion.Player.Instance.leftHandTransform.right.y, 15f * -GorillaLocomotion.Player.Instance.leftHandTransform.right.z), ForceMode.Acceleration);
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(15f * GorillaLocomotion.Player.Instance.rightHandTransform.right.x, 15f * GorillaLocomotion.Player.Instance.rightHandTransform.right.y, 15f * GorillaLocomotion.Player.Instance.rightHandTransform.right.z), ForceMode.Acceleration);
            }
        }

        public static void SlingshotFly()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                Rigidbody rigid = GorillaLocomotion.Player.Instance.gameObject.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(GorillaLocomotion.Player.Instance.headCollider.transform.forward * 25f, ForceMode.Acceleration);
                }
            }
        }

        public static void FastSlingshotFly()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                Rigidbody rigid = GorillaLocomotion.Player.Instance.gameObject.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(GorillaLocomotion.Player.Instance.headCollider.transform.forward * 50f, ForceMode.Acceleration);
                }
            }
        }

        public static void SirenTalk()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
            {
                GorillaTagger.Instance.myVRRig.enabled = false;
                GorillaTagger.Instance.myVRRig.transform.position = new Vector3(-69.6924f, 2.648f, -70.341f);
            }
            else
            {
                GorillaTagger.Instance.myVRRig.enabled = true;
            }
        }


        public static void TeleportRandom()
        {
            MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            foreach (MeshCollider coll in meshColliders)
            {
                coll.enabled = false;
            }
            VRRig random = RigManager.GetVRRigFromPlayer(RigManager.GetRandomPlayer(false));
            GorillaLocomotion.Player.Instance.transform.position = random.transform.position;
            foreach (MeshCollider coll in meshColliders)
            {
                coll.enabled = true;
            }
        }

        public static void Bhop()
        {
            if (GorillaLocomotion.Player.Instance.IsHandTouching(false))
            {
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 220f, ForceMode.Impulse);
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(GorillaTagger.Instance.offlineVRRig.rightHandPlayer.transform.right * 170f, ForceMode.Impulse);
            }
            if (GorillaLocomotion.Player.Instance.IsHandTouching(true))
            {
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(Vector3.up * 220f, ForceMode.Impulse);
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(-GorillaTagger.Instance.offlineVRRig.leftHandPlayer.transform.right * 170f, ForceMode.Impulse);
            }
        }


        public static void AutoRun()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                float time = Time.frameCount;
                GorillaTagger.Instance.rightHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * UnityEngine.Mathf.Cos(time) / 10) + new Vector3(0, -0.5f - (Mathf.Sin(time) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * -0.05f);
                GorillaTagger.Instance.leftHandTransform.position = GorillaTagger.Instance.headCollider.transform.position + (GorillaTagger.Instance.headCollider.transform.forward * Mathf.Cos(time + 180) / 10) + new Vector3(0, -0.5f - (Mathf.Sin(time + 180) / 7), 0) + (GorillaTagger.Instance.headCollider.transform.right * 0.05f);
            }
        }

        public static void TPGun()
        {
            if (Menu.Menu.GetGunInput(false))
            {
                var GunData = Menu.Menu.RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (Menu.Menu.GetGunInput(true))
                {
                    GorillaTagger.Instance.transform.position = Pointer.transform.position;
                }
            }
        }

        public static Vector2 lerpthing;

        public static void BananaCar()
        {
            Vector2 rjoystick = EasyInputs.GetThumbStick2DAxis(EasyHand.RightHand);
            lerpthing = Vector2.Lerp(lerpthing, rjoystick, 0.05f);

            Vector3 addition = GorillaTagger.Instance.bodyCollider.transform.forward * lerpthing.y + GorillaTagger.Instance.bodyCollider.transform.right * lerpthing.x;
            Physics.Raycast(GorillaTagger.Instance.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f), Vector3.down, out var Ray, 512);

            if (Ray.distance < 0.2f && (Mathf.Abs(lerpthing.x) > 0.05f || Mathf.Abs(lerpthing.y) > 0.05f))
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = addition * 10f;
            }
        }


        public static void BarkFly()
        {
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = false;
            var rb = GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody;
            Vector2 xz = EasyInputs.GetThumbStick2DAxis(EasyHand.LeftHand);
            float y = EasyInputs.GetThumbStick2DAxis(EasyHand.RightHand).y;

            Vector3 inputDirection = new Vector3(xz.x, y, xz.y);
            var playerForward = GorillaLocomotion.Player.Instance.bodyCollider.transform.forward;
            playerForward.y = 0;
            var playerRight = GorillaLocomotion.Player.Instance.bodyCollider.transform.right;
            playerRight.y = 0;

            var velocity = inputDirection.x * playerRight + y * Vector3.up + inputDirection.z * playerForward;
            velocity *= 1f * 17f;
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.12875f);
        }
        public static void OffBarkFly()
        {
            GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
        }

        public static void TriggerFly()
        {
            if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
            {
                GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime) * 17f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        public static void NoclipTriggerFly()
        {
            foreach (MeshCollider col in GameObject.FindObjectsOfType<MeshCollider>())
            {
                if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
                {
                    col.enabled = false;
                    GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime) * 17f;
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else 
                    col.enabled = true;
            }
        }

        public static void Noclip()
        {
            foreach (MeshCollider col in GameObject.FindObjectsOfType<MeshCollider>())
            {
                if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
                    col.enabled = false;
                else
                    col.enabled = true;
            }
        }

        public static GameObject platR = null;
        public static GameObject platL = null;
        public static void Platforms()
        {
            if (EasyInputs.GetGripButtonDown(EasyHand.RightHand))
            {
                if (platR == null)
                {
                    platR = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platR.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    platR.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    platR.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
                    var rendererR = platR.GetComponent<Renderer>();
                    if (rendererR != null)
                        rendererR.material.color = Color.black;
                }
            }
            else
            {
                if (platR != null)
                {
                    GameObject.Destroy(platR);
                    platR = null;
                }
            }
            if (EasyInputs.GetGripButtonDown(EasyHand.LeftHand))
            {
                if (platL == null)
                {
                    platL = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platL.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    platL.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    platL.transform.localScale = new Vector3(0.0125f, 0.28f, 0.3825f);
                    var rendererL = platL.GetComponent<Renderer>();
                    if (rendererL != null)
                        rendererL.material.color = Color.black;
                }
            }
            else
            {
                if (platL != null)
                {
                    GameObject.Destroy(platL);
                    platL = null;
                }
            }
        }
    }
}