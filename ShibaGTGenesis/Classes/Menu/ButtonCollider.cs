using ShibaGTGenesis.Menu;
using System;
using System.Reflection;
using UnityEngine;

namespace ShibaGTGenesis.Classes
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class ButtonCollider : MonoBehaviour
    {
        public ButtonCollider(IntPtr ptr) : base(ptr) { }
        public string relatedText;

        public static float buttonCooldown = 0f;

        public void OnTriggerEnter(Collider collider)
        {
            if (Time.time > buttonCooldown && collider == Menu.Menu.Instance.referencecollider && Menu.Menu.Instance.menu != null)
            {
                buttonCooldown = Time.time + 0.2f;
                if (!Settings.buttonsound)
                {
                    if (cachedAudio == null)
                        CacheAudioClip("ShibaGTGenesis.Resources.steal.wav");
                    cachedAudio.Play();
                }
                Menu.Menu.Instance.Toggle(relatedText);
            }
        }

        public static AudioSource cachedAudio;

        public static void CacheAudioClip(string resourceName)
        {
            byte[] soundBytes = LoadEmbeddedSounds(resourceName);
            if (soundBytes == null) return;
            AudioClip clip = WavToAudioClip(soundBytes);
            if (clip == null)
                return;
            cachedAudio = GorillaTagger.Instance.offlineVRRig.gameObject.AddComponent<AudioSource>();
            cachedAudio.clip = clip;
            cachedAudio.volume = 0.5f;
            cachedAudio.loop = false;
            /*if (cachedAudio != null)
            {
                cachedAudio.clip = clip;
                cachedAudio.volume = 0.5f;
                cachedAudio.loop = false;
                cachedAudio.Play();
            }*/
        }
        public static byte[] LoadEmbeddedSounds(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }
        private static AudioClip WavToAudioClip(byte[] fileBytes)
        {
            const int headerSize = 44;
            if (fileBytes.Length < headerSize) return null;
            int sampleRate = BitConverter.ToInt32(fileBytes, 24);
            int channels = BitConverter.ToInt16(fileBytes, 22);
            int dataSize = fileBytes.Length - headerSize;
            int sampleCount = dataSize / 2;
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                short sample = BitConverter.ToInt16(fileBytes, headerSize + i * 2);
                samples[i] = sample / 32768f;
            }
            AudioClip clip = AudioClip.Create("sound", sampleCount / channels, channels, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}