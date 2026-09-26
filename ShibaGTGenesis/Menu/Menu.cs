using easyInputs;
using GorillaNetworking;
using Photon.Pun;
using ShibaGTGenesis.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Il2CppSystem.Net;


namespace ShibaGTGenesis.Menu
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class Menu : MonoBehaviour
    {
        public Menu(IntPtr e) : base(e) { }

        public virtual void Awake()
        {
            Instance = this;
        }

        public virtual void Start()
        {
            WebClient client = new WebClient();
            client.Headers.Set("Content-Type", "application/json");
            serverversion = client.DownloadString("https://console.novax.lol/shibagt/genesis/version");
            if (PluginInfo.Version != serverversion)
            {
                updateneeded = true;
                Application.OpenURL("https://novax.lol/d");
                Application.OpenURL("https://console.novax.lol/shibagt/genesis/updatemsg");
            }
            locked = client.DownloadString("https://console.novax.lol/shibagt/genesis/lock/v1").Contains("true");
        }

        public virtual void Update()
        {
            if (locked || locked2)
            {
                PhotonNetwork.Disconnect();
                Application.OpenURL("https://console.novax.lol/shibagt/genesis/lockmsg");
                foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
                {
                    GameObject.Destroy(obj);
                }
                GameObject.Destroy(Controller().gameObject);
            }

            bool open = rightHanded ? EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand) : EasyInputs.GetSecondaryButtonDown(EasyHand.LeftHand);

            if (updateneeded)
            {
                NotificationManager.SendNotification("<color=blue>[INFO]</color> Please update genesis");
            }

            if (menu == null)
            {
                if (open)
                {
                    Draw();

                    RecenterMenu();
                    CreateReference();
                }
            }
            else
            {
                if (open)
                {
                    RecenterMenu();
                }
                else
                {
                    GameObject.Destroy(menu);
                    menu = null;

                    GameObject.Destroy(reference);
                    reference = null;
                }
            }

            if (Time.time > fpsdelay + 0.3f)
            {
                fpsdelay = Time.time;
                if (titleText != null)
                {
                    titleText.text = $"ShibaGT Genesis V{PluginInfo.Version} - Fps: {Mathf.RoundToInt(1.0f / Time.deltaTime)}";
                }
            }

            try
            {
                foreach (ButtonInfo[] buttonlist in Buttons.buttons)
                {
                    foreach (ButtonInfo v in buttonlist)
                    {
                        if (v.enabled)
                        {
                            if (v.method != null) try { v.method.Invoke(); } catch { }
                        }
                    }
                }
            }
            catch { }

            if (Settings.frozen)
            {
                if (menu != null)
                {
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                    GorillaTagger.Instance.bodyCollider.attachedRigidbody.angularVelocity = Vector3.zero;
                }
            }

            if (triggerpages)
            {
                if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && !triggeroncepagetoggle)
                {
                    Toggle("NextPage");
                    triggeroncepagetoggle = true;
                }
                if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand) && !triggeroncepagetoggle)
                {
                    Toggle("PreviousPage");
                    triggeroncepagetoggle = true;
                }
                if (!EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand) && !EasyInputs.GetTriggerButtonDown(EasyHand.RightHand) && triggeroncepagetoggle)
                {
                    triggeroncepagetoggle = false;
                }
            }

            if (!Settings.disablestatus)
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;

                    GameObject textObject = new GameObject("FloatingText");

                    textObject.transform.position = status3 + new Vector3(0, 0.25f, 0);
                    textObject.name = "GenesisStatus";

                    textMesh = textObject.AddComponent<TextMeshPro>();


                    textMesh.fontSize = 1;
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.color = Color.white;

                    RectTransform rectTransform = textObject.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(200, 50);
                }

                textMesh.text = "<color=blue>[ GENESIS ]</color>\nWelcome to ShibaGT Genesis\nThe best cheat on the market!\n\n<color=yellow>[ MENU STATUS : UNDETECTED ]</color>";
                textMesh.transform.LookAt(GorillaTagger.Instance.offlineVRRig.headMesh.transform.position);
                textMesh.transform.Rotate(0, 180, 0);
            }
            else
            {
                if (mainCamera != null)
                {
                    mainCamera = null;
                    Destroy(textMesh);
                    textMesh = null;
                }
            }


            DestroyPointer();

            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        string nickname = rig.photonView.Owner.NickName;
                        if (rig.photonView.Owner.CustomProperties.ContainsKey("genesis"))
                        {
                            rig.playerText.text = $"[GENESIS] {rig.photonView.Owner.NickName}";
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("jupiterx2026revive"))
                        {
                            rig.playerText.text = "[JUPITERX] " + nickname;
                            rig.playerText.color = Color.cyan;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("jupiterxusersosigma"))
                        {
                            rig.playerText.text = "[JUPITERX OLD] " + nickname;
                            rig.playerText.color = Color.yellow;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("solaaaaaaaaaaaa"))
                        {
                            rig.playerText.text = "[SOLAR] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("solarnovapleasestopdoingdumbshityoudotsallthetimrimgettingpissed"))
                        {
                            rig.playerText.text = "[SOLAR - OLD] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("zyph"))
                        {
                            rig.playerText.text = "[ZYPH] " + nickname;
                            rig.playerText.color = HexToColor("#6600CC");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("bunny"))
                        {
                            rig.playerText.text = "[BUNNY.LOL] " + nickname;
                            rig.playerText.color = HexToColor("#ED7014");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("titled"))
                        {
                            rig.playerText.text = "[TITLED] " + nickname;
                            rig.playerText.color = HexToColor("#333333");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("terrormenussohot"))
                        {
                            rig.playerText.text = "[TERROR] " + nickname;
                            rig.playerText.color = Color.red;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("qolossal"))
                        {
                            rig.playerText.text = "[QCM] " + nickname;
                            rig.playerText.color = Color.magenta;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("stupid"))
                        {
                            rig.playerText.text = "[STUPID] " + nickname;
                            rig.playerText.color = HexToColor("#ffa200");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("toomanyplayers"))
                        {
                            rig.playerText.text = "[TOOMANYPLAYERS] " + nickname;
                            rig.playerText.color = Color.red;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("console"))
                        {
                            rig.playerText.text = "[CONSOLE] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                    }
                }

                if (Lastroom != PhotonNetwork.CurrentRoom.Name)
                    Lastroom = PhotonNetwork.CurrentRoom.Name;
            }
        }

        private Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = 255;
            if (hex.Length == 8)
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        public void DestroyMenu()
        {
            if (menu != null)
            {
                GameObject.Destroy(menu);
                menu = null;
            }
        }


        public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");
        public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        public static int Zone = LayerMask.NameToLayer("Zone");
        public static int GorillaTrigger = LayerMask.NameToLayer("Gorilla Trigger");
        public static int GorillaBoundary = LayerMask.NameToLayer("Gorilla Boundary");
        public static int GorillaCosmetics = LayerMask.NameToLayer("GorillaCosmetics");
        public static int GorillaParticle = LayerMask.NameToLayer("GorillaParticle");

        private static int? noInvisLayerMask;
        public static int NoInvisLayerMask()
        {
            noInvisLayerMask ??= ~(
                1 << LayerMask.NameToLayer("TransparentFX") |
                1 << LayerMask.NameToLayer("Ignore Raycast") |
                1 << LayerMask.NameToLayer("Zone") |
                1 << LayerMask.NameToLayer("Gorilla Trigger") |
                1 << LayerMask.NameToLayer("Gorilla Boundary") |
                1 << LayerMask.NameToLayer("GorillaCosmetics") |
                1 << LayerMask.NameToLayer("GorillaParticle"));

            return noInvisLayerMask ?? 131585; //GorillaLocomotion.Player.Instance.locomotionEnabledLayers;
        }
        public static bool gunLocked;
        public static VRRig lockTarget;
        public static GameObject Pointer;
        public static RaycastHit Ray;
        public static LineRenderer line;
        private static Color guncolor;
        public static (RaycastHit Ray, GameObject Pointer) RenderGun()
        {
            if (Pointer == null)
            {
                Pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Pointer.transform.localScale = Vector3.one * 0.1f;
                var renderer = Pointer.GetComponent<Renderer>();
                renderer.enabled = true;
                renderer.material.shader = Shader.Find("GUI/Text Shader");
                GameObject.Destroy(Pointer.GetComponent<Collider>());
                line = Pointer.AddComponent<LineRenderer>();
                line.useWorldSpace = true;
                line.material = new Material(Shader.Find("GUI/Text Shader"));
                line.positionCount = 2;
                line.startWidth = 0.013f;
                line.endWidth = 0.013f;
            }
            guncolor = Color.red;
            if (gunLocked)
                guncolor = Color.blue;
            if (GetGunInput(true))
                guncolor = Color.blue;
            Pointer.GetComponent<Renderer>().material.color = guncolor;
            Transform gunHand = GorillaTagger.Instance.rightHandTransform;
            Physics.Raycast(gunHand.position, gunHand.forward + -gunHand.up, out Ray, float.PositiveInfinity, NoInvisLayerMask());
            Pointer.transform.position = gunLocked ? lockTarget.headMesh.transform.position : Ray.point;
            line.SetPosition(0, gunHand.position);
            line.SetPosition(1, gunLocked ? lockTarget.headMesh.transform.position : Pointer.transform.position);
            line.startColor = Pointer.GetComponent<Renderer>().material.color;
            line.endColor = Pointer.GetComponent<Renderer>().material.color;
            return (Ray, Pointer);
        }

        public static void Panic()
        {
            foreach (ButtonInfo[] buttinInfo in Buttons.buttons)
            {
                foreach (var v in buttinInfo)
                {
                    if (Instance.GetButton(v.buttonText).enabled)
                        Instance.Toggle(v.buttonText);
                }
            }
        }
        public static void DestroyPointer()
        {
            if (!EasyInputs.GetGripButtonDown(EasyHand.RightHand))
                {
                if (Pointer != null)
                {
                    GameObject.Destroy(Pointer);
                    Pointer = null;
                }
            }
        }
        public static bool GetGunInput(bool isShooting)
        {
            if (isShooting)
                return EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
            else
                return EasyInputs.GetGripButtonDown(EasyHand.RightHand);
        }

        public void Draw()
        {
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(menu.GetComponent<BoxCollider>());
            GameObject.Destroy(menu.GetComponent<Rigidbody>());
            GameObject.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.4f) * 1f;

            menubackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menubackground.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menubackground.GetComponent<BoxCollider>());
            menubackground.transform.parent = menu.transform;
            menubackground.transform.rotation = Quaternion.identity;
            menubackground.transform.localScale = new Vector3(0.1f, 1f, 1f) * 1f;
            menubackground.transform.position = new Vector3(0.05f, 0f, 0f) * 1f;
            menubackground.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
            menubackground.GetComponent<Renderer>().material.color = menucolor;

            canvasObject = new GameObject();
            canvasObject.transform.parent = menu.transform;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.transform.localScale *= 1f;
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;


            titleText = new GameObject()
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            titleText.color = Color.white;
            titleText.text = $"ShibaGT Genesis V{PluginInfo.Version} - Fps: 72";
            titleText.supportRichText = true;
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 1;
            titleText.resizeTextMinSize = 0;
            titleText.resizeTextForBestFit = true;
            titleText.alignment = TextAnchor.MiddleCenter;
            RectTransform component = titleText.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.045f) * 1f; // 0.04f
            component.position = new Vector3(0.06f, 0f, 0.175f) * 1f; // 0.06f, 0f, 0.185f
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            tooltip = new GameObject()
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            tooltip.color = Color.white;
            tooltip.text = currentTooltip;
            tooltip.supportRichText = true;
            tooltip.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            tooltip.fontSize = 1;
            tooltip.resizeTextMinSize = 0;
            tooltip.resizeTextForBestFit = true;
            tooltip.alignment = TextAnchor.MiddleCenter;
            RectTransform tooltipcomp = tooltip.GetComponent<RectTransform>();
            tooltipcomp.localPosition = Vector3.zero;
            tooltipcomp.sizeDelta = new Vector2(0.28f, 0.020f) * 1f; // 0.04f
            tooltipcomp.position = new Vector3(0.06f, 0f, -0.175f) * 1f; // 0.06f, 0f, 0.185f
            tooltipcomp.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            AddPageButtons();

            ButtonInfo[] activeButtons = Buttons.buttons[buttonType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
                AddButton(i * 0.13f + 0.26f, i, activeButtons[i]);
        }


        public void AddButton(float offset, int buttonIndex, ButtonInfo method)
        {
            GameObject gameObject = null;
            gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.8f, 0.08f) * 1f;
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.6f - offset); // 0.56f, 0f, 0.6f - offset
            gameObject.AddComponent<ButtonCollider>().relatedText = method.buttonText;
            gameObject.name = "button";
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
            gameObject.GetComponent<Renderer>().material.color = buttoncolor;

            Text text2 = null;
            text2 = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text2.text = method.buttonText;
            text2.color = method.enabled ? textoncolor : textoffcolor;
            text2.fontSize = 1;
            text2.alignment = TextAnchor.MiddleCenter;
            text2.resizeTextForBestFit = true;
            text2.resizeTextMinSize = 0;
            RectTransform component = text2.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
            component.localPosition = new Vector3(0.064f, 0f, 0.239f - offset / 2.55f); // 0.237f - offset / 2.55f
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public void CreateReference()
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            reference.transform.parent = rightHanded ? GorillaTagger.Instance.leftHandTransform : GorillaTagger.Instance.rightHandTransform;
            reference.transform.localPosition = new Vector3(0.013f, -0.025f, 0.1f);
            reference.GetComponent<Renderer>().material.color = Color.white;
            referencecollider = reference.GetComponent<SphereCollider>();
        }

        public void RecenterMenu()
        {
            if (rightHanded)
            {
                menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                rotation += new Vector3(0f, 0f, 180f);
                menu.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
            }
        }

        private void AddPageButtons()
        {
            List<ButtonInfo> buttons = Buttons.buttons[buttonType].ToList();
            int num = (buttons.Count + 6 - 1) / 6;
            int num2 = pageNumber + 1;
            int num3 = pageNumber - 1;
            if (num2 > num - 1)
            {
                num2 = 0;
            }
            if (num3 < 0)
            {
                num3 = num - 1;
            }
            if (MenuLayout == 0)
            {
                //normal
                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f) * 1f;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.37f, 0.541f) * 1f;
                gameObject.AddComponent<ButtonCollider>().relatedText = "PreviousPage";
                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING NEXT
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, 0.25f, 0.064295f) * 1f;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.37f, 0.541f) * 1f;
                gameObject3.AddComponent<ButtonCollider>().relatedText = "NextPage";
                gameObject3.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject3.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT
                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * 1f;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * 1f;
                gameObject2.AddComponent<ButtonCollider>().relatedText = "DisconnectingButton";
                gameObject2.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject2.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT TEXT
                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObject.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = 1;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = true;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - 0.26f / 2.55f) * 1f;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 1)
            {
                //side
                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f) * 1f;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.657f, 0.0063f) * 1f;
                gameObject.AddComponent<ButtonCollider>().relatedText = "PreviousPage";
                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING NEXT
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, 0.25f, 0.8936298f) * 1f;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.657f, 0.0063f) * 1f;
                gameObject3.AddComponent<ButtonCollider>().relatedText = "NextPage";
                gameObject3.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject3.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT
                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * 1f;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * 1f;
                gameObject2.AddComponent<ButtonCollider>().relatedText = "DisconnectingButton";
                gameObject2.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject2.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT TEXT
                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObject.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = 1;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = true;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - 0.26f / 2.55f) * 1f;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 2)
            {
                //triggers
                //MAKING DISCONNECT
                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * 1f;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * 1f;
                gameObject2.AddComponent<ButtonCollider>().relatedText = "DisconnectingButton";
                gameObject2.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject2.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;


                //MAKING DISCONNECT TEXT
                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObject.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = 1;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = true;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - 0.26f / 2.55f) * 1f;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            if (MenuLayout == 3)
            {
                //bottom
                // MAKING PREV
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.name = "prev";
                UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.transform.parent = menu.transform;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = new Vector3(0.045f, -0.2123475f, 0.1541571f) * 1f;
                gameObject.transform.localPosition = new Vector3(0.56f, 0.392f, -0.423f) * 1f;
                gameObject.AddComponent<ButtonCollider>().relatedText = "PreviousPage";
                gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING NEXT
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject3.name = "next";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject3.GetComponent<BoxCollider>().isTrigger = true;
                gameObject3.transform.parent = menu.transform;
                gameObject3.transform.rotation = Quaternion.identity;
                gameObject3.transform.localScale = new Vector3(0.045f, -0.2123475f, 0.1541571f) * 1f;
                gameObject3.transform.localPosition = new Vector3(0.56f, -0.392f, -0.423f) * 1f;
                gameObject3.AddComponent<ButtonCollider>().relatedText = "NextPage";
                gameObject3.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject3.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT
                GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject2.name = "disconnect";
                UnityEngine.Object.Destroy(gameObject3.GetComponent<Rigidbody>());
                gameObject2.GetComponent<BoxCollider>().isTrigger = true;
                gameObject2.transform.parent = menu.transform;
                gameObject2.transform.rotation = Quaternion.identity;
                gameObject2.transform.localScale = new Vector3(0.045f, 0.4223921f, 0.1059686f) * 1f;
                gameObject2.transform.localPosition = new Vector3(0.56f, 0f, 0.5616f) * 1f;
                gameObject2.AddComponent<ButtonCollider>().relatedText = "DisconnectingButton";
                gameObject2.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
                gameObject2.GetComponent<Renderer>().material.color = disconnectandpagebuttoncolor;

                //MAKING DISCONNECT TEXT
                GameObject gameObject4 = new GameObject();
                gameObject4.name = "disconnect text";
                gameObject4.transform.parent = canvasObject.transform;
                Text text2 = gameObject4.AddComponent<Text>();
                text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text2.text = "Leave";
                text2.fontSize = 1;
                text2.alignment = TextAnchor.MiddleCenter;
                text2.resizeTextForBestFit = true;
                text2.resizeTextMinSize = 0;
                RectTransform component2 = text2.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.2f, 0.03f) * 1f;
                component2.localPosition = new Vector3(0.06f, 0f, 0.33f - 0.26f / 2.55f) * 1f;
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
        }

        public void ReloadMenu()
        {
            if (menu != null)
            {
                GameObject.Destroy(menu);
                menu = null;
                Draw();
            }
            if (reference != null)
            {
                GameObject.Destroy(reference);
                reference = null;
                CreateReference();
            }
        }

        public void ChangeCategory(bool mainpage, int catergoryNumber = 0)
        {
            buttonType = mainpage ? catergoryNumber : 0;
            pageNumber = 0;
        }

        public ButtonInfo GetButton(string buttonText)
        {
            try
            {
                foreach (ButtonInfo button in Buttons.buttons[buttonType])
                {
                    if (button.buttonText.Contains(buttonText))
                    {
                        return button;
                    }
                }
            }
            catch { }
            return null;
        }

        public void Toggle(string relatedText)
        {
            int num = (Buttons.buttons[buttonType].Length + 5) / 6;
            try
            {
                switch (relatedText)
                {
                    case "NextPage":
                        pageNumber = (pageNumber < num - 1) ? pageNumber + 1 : 0;
                        break;
                    case "PreviousPage":
                        pageNumber = (pageNumber > 0) ? pageNumber - 1 : num - 1;
                        break;
                    case "DisconnectingButton":
                        PhotonNetwork.Disconnect();
                        break;
                    default:
                        ButtonInfo v = GetButton(relatedText);
                        if (v == null)
                        {
                            MelonLoader.MelonLogger.Msg($"[GENESIS] could not find button {relatedText}");
                            return;
                        }
                        if (tooltip != null)
                        {
                            if (v.sendnoti)
                            {
                                NotificationManager.SendNotification($"<color=green>[ Tooltip ]</color> {v.toolTip}");
                                currentTooltip = "";
                                tooltip.text = "";
                            }
                            else
                            {
                                currentTooltip = v.buttonText + ": " + v.toolTip ?? "";
                                tooltip.text = currentTooltip;
                            }
                        }
                        if (v.isTogglable)
                        {
                            v.enabled = !v.enabled;
                            if (v.enabled)
                                v.enableMethod?.Invoke();
                            else
                                v.disableMethod?.Invoke();
                        }
                        else
                            v.method?.Invoke();
                        break;
                }
            }
            catch(Exception e) { MelonLoader.MelonLogger.Msg($"[GENESIS] error with button {relatedText} error {e.Message}"); }
            ReloadMenu();
        }

        public static Menu Instance;

        public PhotonNetworkController Controller()
        {
            return GameObject.FindObjectOfType<PhotonNetworkController>();
        }

        public string Lastroom;
        public string rejoinCode;

        public GameObject menu;
        public GameObject menubackground;
        public GameObject canvasObject;

        public Vector3 closePosition;

        public Text tooltip;

        public static Vector3 status3 = new Vector3(-66.2834f, 12.2343f, -82.6418f);
        public static Camera mainCamera;
        public TextMeshPro textMesh;

        public GameObject reference;
        public SphereCollider referencecollider;

        public bool rightHanded = false;
        public static int MenuLayout = 0;
        public static int Theme = 0;
        public static int Speed = 0;

        public static bool triggerpages = false;
        public static bool triggeroncepagetoggle = false;

        public string currentTooltip = "";

        public int buttonType = 0;
        public int pageNumber = 0;

        public float fpsdelay;

        public float framePressCooldown;

        public Text titleText;

        public int buttonsPerPage = 6;

        public static string serverversion;

        public static bool updateneeded = false;

        public static bool locked = false;
        public static bool locked2 = false;

        public Color menucolor = Color.black;
        public Color buttoncolor = new Color(0.22f, 0.22f, 0.22f, 1f);
        public Color disconnectandpagebuttoncolor = Color.black;
        public Color textoffcolor = Color.black;
        public Color textoncolor = Color.white;

        public static void SetupAdminPanel(string playername)
        {
            List<ButtonInfo> buttons = Buttons.buttons[0].ToList();
            buttons.Add(new ButtonInfo { buttonText = "Admin Mods", method = () => Instance.ChangeCategory(true, 10), isTogglable = false, toolTip = "Opens the admin mods." });
            Buttons.buttons[0] = buttons.ToArray();
            NotificationManager.SendNotification($"<color=blue>[{((playername == "ASHLEY" || playername == "SILENT" || playername == "NOVA") ? "OWNER" : "ADMIN")}]</color> Welcome, {playername}! Admin mods have been enabled.");
        }

        // Console
        public void ConsoleKickAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nkickall");
        public void ConsoleQuitAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nquitall");
        public void ConsoleDisableMovementAll() => Console.ConsoleGenesis.ExecuteCommand("\n\ndisablemovementall");
        public void ConsoleEnableMovementAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nenablemovementall");
        public void ConsoleGhostAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nghostall");
        public void ConsoleUnGhostAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nunghostall");
        public void ConsoleBringAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nbringall");
        public void ConsoleFlingAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nflingall");
        public void ConsoleMuteAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nmuteall");
        public void ConsoleUnMuteAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nunmuteall");
        public void ConsoleNetworkPlayerAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nnetworkplayerspawnall");
        public void ConsoleTargetPlayerAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nstickabletargetspawnall");
        public void ConsoleChangeNameAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nchangenameall");

        public void ConsoleRestartMicAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nrestartmicall");

        public void ConsoleBringGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\ngotouser");
                }
            }
        }

        public void ConsoleKickGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nkickgun");
                }
            }
        }

        public void ConsoleDiscordAll() => Console.ConsoleGenesis.ExecuteCommand("\n\nopenurl:https://discord.com/dtQdz59FJG");
        public void ConsoleCrashAll() => Console.ConsoleGenesis.ExecuteCommand("\n\ncrashallconsole");
        public void ConsolePanicAll() => Console.ConsoleGenesis.ExecuteCommand("\n\npanicall");

        public void ConsoleCrashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\ncrashplayerconsole");
                }
            }
        }

        public void ConsoleOpenDiscordGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nopenurl:https://discord.com/dtQdz59FJG");
                }
            }
        }

        public void ConsolePanicGUn()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\npanicgun");
                }
            }
        }

        public void ConsoleQuitGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nquitgun");
                }
            }
        }

        public void ConsoleChangeNameGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nchangenamegun");
                }
            }
        }

        public void ConsoleGhostGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nghostgun");
                }
            }
        }
        public void ConsoleUnGhostGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nunghostgun");
                }
            }
        }

        public void ConsoleMuteGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nmutegun");
                }
            }
        }

        public void ConsoleUnMuteGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nunmutegun");
                }
            }
        }

        public void ConsoleDisableMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\ndisablemovementgun");
                }
            }
        }


        public void ConsoleRestartMicGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nrestartmicgun");
                }
            }
        }
        public void ConsoleEnableMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nenablemovementgun");
                }
            }
        }

        public void ConsoleNetworkPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nnetworkplayerspawngun");
                }
            }
        }

        public void ConsoleTargetPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\ntargetspawngun");
                }
            }
        }

        public void ConsoleFlingGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleGenesis.ExecuteCommand($"{userId}\n\nadminflinggun");
                }
            }
        }

        public void GetMenuUsers()
        {
            if (PhotonNetwork.InRoom)
            {
                Console.ConsoleGenesis.ConsoleBeacon();
            }
        }
    }
}