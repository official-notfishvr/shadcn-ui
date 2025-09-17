using shadcnui;
#if BEPINEX
using BepInEx;
using BepInEx.Logging;
#elif MELONLOADER
using MelonLoader;
#endif
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace shadcnui
{
#if BEPINEX
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;
        public static ManualLogSource PluginLogger => instance.Logger;
        public static bool FirstLaunch;

        private void Awake()
        {
            instance = this;
            
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<UI>();

            DontDestroyOnLoad(Loader);
        }
    }
#elif MELONLOADER
    //[MelonInfo(typeof(Plugin), PluginInfo.Name, PluginInfo.Version, "shadcnui")]
    //[MelonGame("Another Axiom", "Gorilla Tag")]
    public class Plugin : MelonMod
    {
        public static bool FirstLaunch;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<UI>();

            GameObject.DontDestroyOnLoad(Loader);
        }
    }
#endif
}
