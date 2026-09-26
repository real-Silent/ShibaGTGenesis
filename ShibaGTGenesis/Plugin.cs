using ExitGames.Client.Photon;
using Il2CppSystem.Net;
using MelonLoader;
using Photon.Pun;
using ShibaGTGenesis;
using ShibaGTGenesis.Classes;
using ShibaGTGenesis.Menu;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;

[assembly: MelonInfo(typeof(Plugin), "ShibaGTGenesis", "1.0.0", "Nova_ShibaGTGenesis")]
[assembly: MelonGame()]
namespace ShibaGTGenesis
{
    public class Plugin : MelonMod
    {
        [Obsolete]
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            ClassInjector.RegisterTypeInIl2Cpp<Menu.Menu>();
            ClassInjector.RegisterTypeInIl2Cpp<NotificationManager>();
            ClassInjector.RegisterTypeInIl2Cpp<Boards>();
            ClassInjector.RegisterTypeInIl2Cpp<SkeletonESPClass>();
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("Nova_ShibaGTGenesis");
            harmony.PatchAll();
            GameObject holder_genesis = new GameObject();
            holder_genesis.name = "ShibaGTGenesis_holder";
            holder_genesis.AddComponent<Menu.Menu>();
            holder_genesis.AddComponent<Boards>();
            holder_genesis.AddComponent<NotificationManager>();

            Console.ConsoleGenesis.LoadConsole();

            Hashtable genesishash = new Hashtable();
            genesishash.Add("genesis", "genesis");
            PhotonNetwork.LocalPlayer.SetCustomProperties(genesishash);

            ButtonCollider.CacheAudioClip("ShibaGTGenesis.Resources.steal.wav"); // why not put it hear

            Settings.LoadEnabledButtons();
            Settings.LoadPreferences();

            WebClient client = new WebClient();
            client.Headers.Set("Content-Type", "application/json");
            Menu.Menu.locked2 = client.DownloadString("https://console.novax.lol/shibagt/genesis/lock/v2").Contains("true");
        }
    }
}