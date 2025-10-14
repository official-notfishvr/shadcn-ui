using System;
using System.IO;
using System.Reflection;
using shadcnui;
using shadcnui_Demo.Menu;
using UnityEngine;
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

namespace shadcnui_Demo
{
    // Embed the UI lib
    public static class AssemblyLoader
    {
        static AssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                if (args.Name.Contains("shadcnui"))
                {
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("shadcnui_Demo.shadcnui.dll"))
                    {
                        if (stream == null)
                            return null;
                        byte[] assemblyData = new byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return Assembly.Load(assemblyData);
                    }
                }
                return null;
            };
        }

        public static void Init() { }
    }

#if IL2CPP_BEPINEX
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BasePlugin
    {
        public static Plugin instance;
        public static ManualLogSource PluginLogger => instance.Log;
        public static bool FirstLaunch;

        public override void Load()
        {
            AssemblyLoader.Init();
            instance = this;
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<DemoSelector>();
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
            AssemblyLoader.Init();
            instance = this;
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<DemoSelector>();
            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
#elif MELONLOADER
    public class Plugin : MelonMod
    {
        public static bool FirstLaunch;

        public override void OnInitializeMelon()
        {
            AssemblyLoader.Init();
            base.OnInitializeMelon();
            GorillaTagger.OnPlayerSpawned(LoadMenu);
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<DemoSelector>();
            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
#endif
}
