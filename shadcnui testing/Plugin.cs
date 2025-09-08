using shadcnui;
using BepInEx;
using BepInEx.Logging;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace shadcnui
{
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
}
