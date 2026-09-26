using easyInputs;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShibaGTGenesis
{
    public class VisualMods // this whole thing is just taken from genesis src im lazy - nova
    {
        public static void ESP()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    ((Renderer)rig.mainSkin).material.shader = Shader.Find("GUI/Text Shader");
                    ((Renderer)rig.mainSkin).material.color = new Color(0f, 0f, 1f, 0.5f);
                }
            }
        }
        public static void DisableESP()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    rig.ChangeMaterialLocal(rig.currentMatIndex);
                }
            }
        }

        public static void Turning()
        {
            Vector2 axis = EasyInputs.GetThumbStick2DAxis(EasyHand.RightHand);
            if (axis.x > 0.6f)
            {
                Turn(6f);
            }
            if (axis.x < -0.6f)
            {
                Turn(-6f);
            }
        }

        private static void Turn(float degrees)
        {
            var playerType = Type.GetType("GorillaLocomotion.Player, Assembly-CSharp") ?? Type.GetType("GLocomotion.Player, Assembly-CSharp");
            if (playerType == null)
                return;
            var playerField = playerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            object playerInstance = playerField?.GetValue(null);
            if (playerInstance == null)
                return;
            var turnMethod = playerType.GetMethod("Turn", BindingFlags.Public | BindingFlags.Instance);
            if (turnMethod == null)
                return;
            turnMethod.Invoke(playerInstance, new object[] { degrees });
        }

        public static void HeadESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.myVRRig)
                {
                    GameObject o = null;
                    try
                    {
                        o = vrrig.transform.Find("headesp").gameObject;
                    }
                    catch { }
                    if (!o)
                    {
                        UnityEngine.Color thecolor = vrrig.mainSkin.material.color;
                        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        box.name = "headesp";
                        box.transform.SetParent(vrrig.transform);
                        box.transform.position = vrrig.headConstraint.position;
                        UnityEngine.Object.Destroy(box.GetComponent<BoxCollider>());
                        box.transform.localScale = new Vector3(0.2f, 0.2f, 0f);
                        box.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                        box.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                        box.GetComponent<Renderer>().material.color = thecolor;
                    }
                    else
                    {
                        o.GetComponent<Renderer>().material.color = vrrig.mainSkin.material.color;
                    }
                }
            }
        }

        public static void OffHeadESP()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    GameObject o = null;
                    try
                    {
                        o = vrrig.transform.Find("headesp").gameObject;
                    }
                    catch { }
                    if (o)
                    {
                        GameObject.Destroy(o);
                    }
                }
            }
        }


        static bool boxbool;

        public static void BoxEsp()
        {
            boxbool = true;

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.myVRRig)
                {
                    try
                    {
                        GameObject obe = vrrig.transform.Find("box")?.gameObject;

                        if (obe == null)
                        {
                            GameObject gameObject = new GameObject("box");

                            gameObject.transform.SetParent(vrrig.transform);
                            gameObject.transform.localPosition = Vector3.zero;

                            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            GameObject gameObject4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            GameObject gameObject5 = GameObject.CreatePrimitive(PrimitiveType.Cube);

                            GameObject.Destroy(gameObject2.GetComponent<BoxCollider>());
                            GameObject.Destroy(gameObject3.GetComponent<BoxCollider>());
                            GameObject.Destroy(gameObject4.GetComponent<BoxCollider>());
                            GameObject.Destroy(gameObject5.GetComponent<BoxCollider>());

                            gameObject2.transform.SetParent(gameObject.transform);
                            gameObject3.transform.SetParent(gameObject.transform);
                            gameObject4.transform.SetParent(gameObject.transform);
                            gameObject5.transform.SetParent(gameObject.transform);

                            gameObject2.transform.localPosition = new Vector3(0f, 0.49f, 0f);
                            gameObject5.transform.localPosition = new Vector3(0f, -0.49f, 0f);
                            gameObject4.transform.localPosition = new Vector3(-0.49f, 0f, 0f);
                            gameObject3.transform.localPosition = new Vector3(0.49f, 0f, 0f);

                            gameObject2.transform.localScale = new Vector3(1f, 0.04f, 0.04f);
                            gameObject5.transform.localScale = new Vector3(1f, 0.04f, 0.04f);
                            gameObject4.transform.localScale = new Vector3(0.04f, 1f, 0.04f);
                            gameObject3.transform.localScale = new Vector3(0.04f, 1f, 0.04f);

                            gameObject2.name = "rizz1";
                            gameObject3.name = "rizz2";
                            gameObject4.name = "rizz3";
                            gameObject5.name = "rizz4";

                            Renderer[] renderers =
                            {
                                gameObject2.GetComponent<Renderer>(),
                                gameObject3.GetComponent<Renderer>(),
                                gameObject4.GetComponent<Renderer>(),
                                gameObject5.GetComponent<Renderer>()
                            };

                            foreach (Renderer renderer in renderers)
                            {
                                renderer.material.shader = Shader.Find("GUI/Text Shader");
                                renderer.material.color = Color.blue;
                            }

                            obe = gameObject;
                        }

                        obe.transform.LookAt(obe.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public static void OFFBoxEsp()
        {
            if (!boxbool)
                return;

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null)
                {
                    Transform box = vrrig.transform.Find("box");

                    if (box != null)
                    {
                        GameObject.Destroy(box.gameObject);
                    }
                }
            }

            boxbool = false;
        }



        public static bool TracersBool;

        public static void Tracers()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != GorillaTagger.Instance.myVRRig)
                {
                    if (!vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>())
                    {
                        vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                        vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().startWidth = 0.0075f;
                        vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material.shader = Shader.Find("Standard");
                    }
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(0, vrrig.head.rigTarget.gameObject.transform.position);
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().material.color = vrrig.mainSkin.material.color;
                    vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>().SetPosition(1, GorillaTagger.Instance.offlineVRRig.rightHandTransform.position);
                }
            }
            TracersBool = true;
        }
        public static void OFFTracers()
        {
            if (TracersBool == true)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    GameObject.Destroy(vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>());
                }
                TracersBool = false;
            }
        }


        private static bool BoneESP;
        public static void StartSkeleEsp()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    if (!rig.gameObject.GetComponent<SkeletonESPClass>())
                    {
                        rig.gameObject.AddComponent<SkeletonESPClass>();
                    }
                }
            }
            BoneESP = true;
        }

        public static void EndSkeleEsp()
        {
            if (BoneESP)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && !rig.isOfflineVRRig)
                    {
                        GameObject.Destroy(rig.gameObject.GetComponent<SkeletonESPClass>());
                    }
                }
                BoneESP = false;
            }
        }
    }

    [MelonLoader.RegisterTypeInIl2Cpp]
    public class SkeletonESPClass : MonoBehaviour
    {
        public SkeletonESPClass(IntPtr e) : base(e) { }

        public Color lineColor = Color.blue;
        public float lineWidth = 0.02f;

        private LineRenderer lineRenderer;
        private List<GameObject> lineObjects = new List<GameObject>();
        public static bool RGBSkeletonESP = true;

        public virtual void Start()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }
        public virtual void Update()
        {
            DrawSkeleton();
        }

        public virtual void OnDestroy()
        {
            ClearLineObjects();
        }

        public void DrawSkeleton()
        {
            ClearLineObjects();

            VRRig rig = GetComponent<VRRig>();
            if (rig == null)
            {
                Debug.LogWarning("displyy made this btw so dont diss me lol");
                return;
            }

            Color colorrig = rig.mainSkin.material.color;
            if (RGBSkeletonESP)
            {
                colorrig = GetAnimatedColor();
            }
            DrawLine(rig.headMesh.transform.position - new Vector3(0, 0.35f, 0), rig.headMesh.transform.position, colorrig);
            DrawLine(rig.headMesh.transform.position - new Vector3(0, 0.05f, 0), rig.headMesh.transform.position + rig.headMesh.transform.up * +0.2f, colorrig);
            DrawLine(rig.headMesh.transform.position - new Vector3(0, 0.05f, 0), rig.headMesh.transform.position + rig.transform.right * -0.15f, colorrig);
            DrawLine(rig.headMesh.transform.position - new Vector3(0, 0.05f, 0), rig.headMesh.transform.position + rig.transform.right * +0.15f, colorrig);
            DrawLine(rig.rightHandTransform.position, rig.rightThumb.fingerBone1.position, colorrig);
            DrawLine(rig.rightThumb.fingerBone1.position, rig.rightThumb.fingerBone2.position, colorrig);
            DrawLine(rig.rightHandTransform.position, rig.rightIndex.fingerBone1.position, colorrig);
            DrawLine(rig.rightIndex.fingerBone1.position, rig.rightIndex.fingerBone2.position, colorrig);
            DrawLine(rig.rightIndex.fingerBone2.position, rig.rightIndex.fingerBone3.position, colorrig);
            DrawLine(rig.rightHandTransform.position, rig.rightMiddle.fingerBone1.position, colorrig);
            DrawLine(rig.rightMiddle.fingerBone1.position, rig.rightMiddle.fingerBone2.position, colorrig);
            DrawLine(rig.rightMiddle.fingerBone2.position, rig.rightMiddle.fingerBone3.position, colorrig);
            DrawLine(rig.leftHandTransform.position, rig.leftThumb.fingerBone1.position, colorrig);
            DrawLine(rig.leftThumb.fingerBone1.position, rig.leftThumb.fingerBone2.position, colorrig);
            DrawLine(rig.leftHandTransform.position, rig.leftIndex.fingerBone1.position, colorrig);
            DrawLine(rig.leftIndex.fingerBone1.position, rig.leftIndex.fingerBone2.position, colorrig);
            DrawLine(rig.leftIndex.fingerBone2.position, rig.leftIndex.fingerBone3.position, colorrig);
            DrawLine(rig.leftHandTransform.position, rig.leftMiddle.fingerBone1.position, colorrig);
            DrawLine(rig.leftMiddle.fingerBone1.position, rig.leftMiddle.fingerBone2.position, colorrig);
            DrawLine(rig.leftMiddle.fingerBone2.position, rig.leftMiddle.fingerBone3.position, colorrig);
        }
        private Color GetAnimatedColor()
        {
            float time = Time.time;
            float red = Mathf.Sin(time * 2f) * 0.5f + 0.5f;
            float green = Mathf.Sin(time * 1.5f) * 0.5f + 0.5f;
            float blue = Mathf.Sin(time * 2.5f) * 0.5f + 0.5f;
            return new Color(red, green, blue);
        }
        private void ClearLineObjects()
        {
            foreach (GameObject lineObj in lineObjects)
            {
                Destroy(lineObj);
            }
            lineObjects.Clear();
        }

        private GameObject CreateLineObject()
        {
            GameObject lineObj = new GameObject("LineObject");
            lineObj.transform.SetParent(transform);
            lineObjects.Add(lineObj);
            return lineObj;
        }

        private void DrawLine(Vector3 startPos, Vector3 endPos, Color color)
        {
            GameObject lineObj = CreateLineObject();
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] { startPos, endPos });
        }
    }
}