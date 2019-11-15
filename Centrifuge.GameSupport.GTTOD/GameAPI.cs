using Centrifuge.GTTOD.Internal;
using Centrifuge.GTTOD.ResourceManagement;
using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Extensions;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centrifuge.GTTOD
{
    [GameSupportLibraryEntryPoint(GttodGameNamespace, AwakeAfterInitialize = true)]
    internal sealed class GameAPI : MonoBehaviour
    {
        internal const string GttodGameNamespace = "com.github.ciastex/Centrifuge.GTTOD";

        private Log Log => LogManager.GetForCurrentAssembly();

        private Settings Settings { get; set; }
        private PrefabInitializer PrefabInitializer { get; set; }

        private Terminal Terminal { get; set; }

        public void Initialize(IManager manager)
        {
            InitializeSettings();

            PrefabInitializer = new PrefabInitializer();
            PrefabInitializer.HookUpEvent();

            try
            {
                RuntimePatcher.AutoPatch();
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize mix-ins. Mods will still be loaded, but may not function correctly.");
                Log.Exception(e);
            }

            try
            {
                RuntimePatcher.RunTranspilers();
            }
            catch (Exception e)
            {
                Log.Error("Failed to initialize one or more transpilers. Mods will still be loaded, but may not function correctly.");
                Log.Exception(e);
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

                CommandTerminal.Terminal.Log(centrifugeVersion.ToString());
                CommandTerminal.Terminal.Log(reactorApiVersion.ToString());
                CommandTerminal.Terminal.Log(reactorVersion.ToString());
                CommandTerminal.Terminal.Log(thisAssemblyVersion.ToString());
            }, 0, -1, "Prints versions of all core Centrifuge modules.");
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
    }
}
