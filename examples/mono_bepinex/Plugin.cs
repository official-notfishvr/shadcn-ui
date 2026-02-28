using System;
using System.Reflection;
using shadcnui_examples.Menu;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace shadcnui_examples
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
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("examples.Libs.shadcnui.dll"))
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
            GorillaTagger.OnPlayerSpawned(LoadMenu); // for gorilla tag
        }

        private static void LoadMenu()
        {
            GameObject Loader = new GameObject("Loader");
            Loader.AddComponent<ExamplesSelector>();
            UnityEngine.Object.DontDestroyOnLoad(Loader);
        }
    }
}
