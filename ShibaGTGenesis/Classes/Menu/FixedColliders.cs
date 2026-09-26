using ShibaGTGenesis.Menu;
using UnityEngine;

namespace JupiterX.Classes
{
    public class FixedColliders
    {
        public static void CheckButton()
        {
            float num = Vector3.Distance(FixedColliders.button.transform.position, FixedColliders.reference.transform.position);
            if (Time.frameCount >= Menu.Instance.framePressCooldown + 30 && (double)num <= 0.02)
            {
                Menu.Instance.Toggle(FixedColliders.relatedText);
                Menu.Instance.framePressCooldown = Time.frameCount;
            }
        }
        static Transform smethod_0(GameObject gameObject_0)
        {
            return gameObject_0.transform;
        }
        static Vector3 smethod_1(Transform transform_0)
        {
            return transform_0.position;
        }
        static int smethod_2()
        {
            return Time.frameCount;
        }
        public static string relatedText;
        public static GameObject reference;
        public static GameObject button;
    }
}