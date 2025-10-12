using System;
using System.IO;
using shadcnui;
using UnityEngine;
using shadcnui_testing.Menu;

#if IL2CPP_BEPINEX
using BepInEx.Unity.IL2CPP;
using BepInEx;
using BepInEx.Logging;
#elif BEPINEX
using BepInEx;
using BepInEx.Logging;
#elif MELONLOADER
using MelonLoader;
#endif

namespace shadcnui_testing
{
#if IL2CPP_BEPINEX // BepInEx6 il2cpp
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BasePlugin
    {
        public static Plugin instance;
        public static ManualLogSource PluginLogger => instance.Log;
        public static bool FirstLaunch;

        public override void Load()
        {
            instance = this;
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<UI>();

            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
#elif BEPINEX
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

            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
#elif MELONLOADER
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

            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
#endif
}
