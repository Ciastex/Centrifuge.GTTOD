using Harmony;
using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Extensions;
using Centrifuge.GTTOD.Infrastructure;
using Centrifuge.GTTOD.Internal;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Reactor.API.Logging;
using System.Diagnostics;
using Centrifuge.GTTOD.ResourceManagement;

namespace Centrifuge.GTTOD
{
    [GameSupportLibraryEntryPoint(GttodGameNamespace)]
    internal sealed class GameAPI : MonoBehaviour
    {
        internal const string GttodGameNamespace = "com.github.ciastex/Centrifuge.GTTOD";

        internal static Log Log { get; set; }

        private Settings Settings { get; set; }
        private PrefabInitializer PrefabInitializer { get; set; }

        private HarmonyInstance HarmonyInstance { get; set; }
        private Terminal Terminal { get; set; }

        internal void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Log = new Log("gttod_gsl");

            InitializeSettings();

            PrefabInitializer = new PrefabInitializer();
            PrefabInitializer.HookUpEvent();

            try
            {
                InitializeMixins();
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize mix-ins. Mods will still be loaded, but may not function correctly.");
                Log.Exception(e, true);
            }

            try
            {
                InitializeTranspilers();
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize one or more transpilers. Mods will still be loaded, but may not function correctly.");
                Log.Exception(e, true);
            }

            SceneManager.sceneLoaded += InitializeTerminal;
        }

        internal void Update()
        {
            Terminal.ApplyStyle();
        }

        private void InitializeTerminal(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= InitializeTerminal;

            Terminal = new Terminal(Settings);
            AddCetrifugeSpecificCommands();
        }

        private void AddCetrifugeSpecificCommands()
        {
            CommandTerminal.Terminal.Shell.AddCommand("cnfg_version", (args) =>
            {
                var thisAssemblyVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                var reactorVersion = FileVersionInfo.GetVersionInfo(AssemblyEx.GetAssemblyByName("Reactor").Location);
                var centrifugeVersion = FileVersionInfo.GetVersionInfo(AssemblyEx.GetAssemblyByName("Centrifuge").Location);
                var reactorApiVersion = FileVersionInfo.GetVersionInfo(AssemblyEx.GetAssemblyByName("Reactor.API").Location);

                CommandTerminal.Terminal.Log($"Centrifuge Bootstrap version: {centrifugeVersion}");
                CommandTerminal.Terminal.Log($"Reactor API version: {reactorApiVersion}");
                CommandTerminal.Terminal.Log($"Reactor version: {reactorVersion}\n");
                CommandTerminal.Terminal.Log($"GTTOD GSL version: {thisAssemblyVersion}");
            }, 0, -1, "Prints versions of all Centrifuge modules.");
        }

        private void InitializeSettings()
        {
            Settings = new Settings("gttod_gsl");

            Settings.GetOrCreate(Global.ConsoleFontNameSettingsKey, "Lucida Console");
            Settings.GetOrCreate(Global.ConsoleFontSizeSettingsKey, 13);
            Settings.GetOrCreate(Global.ConsoleBufferSizeSettingsKey, 1024);
            Settings.GetOrCreate(Global.ConsoleDropDownAnimationSpeedSettingsKey, 720);
            Settings.GetOrCreate(Global.ConsoleShowGuiButtonsSettingsKey, false);

            if (Settings.Dirty)
                Settings.Save();
        }

        private void InitializeMixins()
        {
            HarmonyInstance = HarmonyInstance.Create(GttodGameNamespace);
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void InitializeTranspilers()
        {
            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes();

            foreach (var type in types)
            {
                if (typeof(GameCodeTranspiler).IsAssignableFrom(type) && type != typeof(GameCodeTranspiler))
                {
                    var transpiler = (Activator.CreateInstance(type) as GameCodeTranspiler);

                    Log.Info($"Transpiler: {type.FullName}");
                    transpiler.Apply(HarmonyInstance);
                }
            }
        }
    }
}
