﻿using Harmony;
using Reactor.API.Attributes;
using Reactor.API.Configuration;
using Reactor.API.Extensions;
using Centrifuge.GTTOD.Infrastructure;
using Centrifuge.GTTOD.Internal;
using System;
using System.Reflection;
using UnityEngine;
using Logger = Reactor.API.Logging.Logger;
using UnityEngine.SceneManagement;

namespace Centrifuge.GTTOD
{
    [GameSupportLibraryEntryPoint(GttodGameNamespace)]
    internal sealed class GameAPI : MonoBehaviour
    {
        internal const string GttodGameNamespace = "com.github.ciastex/Centrifuge.GTTOD";

        private Settings Settings { get; set; }
        private Logger Logger { get; set; }

        private HarmonyInstance HarmonyInstance { get; set; }
        private Terminal Terminal { get; set; }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Logger = new Logger("gttod_gsl");

            InitializeSettings();

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

            Terminal = new Terminal(Settings);
            AddCetrifugeSpecificCommands();

            try
            {
                InitializeMixins();
            }
            catch (Exception e)
            {
                Logger.Error("Failed to initialize Game API mix-ins. Mods will still be loaded, but may not function correctly.");
                Logger.ExceptionSilent(e);
            }

            try
            {
                InitializeTranspilers();
            }
            catch (Exception e)
            {
                Logger.Error("Failed to initialize one or more transpilers. Mods will still be loaded, but may not function correctly.");
                Logger.Exception(e);
            }
        }

        public void Update()
        {
            Terminal.ApplyStyle();
        }

        private void AddCetrifugeSpecificCommands()
        {
            CommandTerminal.Terminal.Shell.AddCommand("cnfg_version", (args) =>
            {
                var reactorAssembly = AssemblyEx.GetAssemblyByName("Reactor");
                var centrifugeAssembly = AssemblyEx.GetAssemblyByName("Centrifuge");
                var reactorApiAssembly = AssemblyEx.GetAssemblyByName("Reactor.API");
                var reactorGameApiAssembly = AssemblyEx.GetAssemblyByName("Centrifuge.GTTOD");

                CommandTerminal.Terminal.Log($"Reactor ModLoader version: {reactorAssembly.GetName().Version.ToString()}");
                CommandTerminal.Terminal.Log($"Reactor GameAPI version: {reactorGameApiAssembly.GetName().Version.ToString()}");
                CommandTerminal.Terminal.Log($"Reactor API version: {reactorApiAssembly.GetName().Version.ToString()}");
                CommandTerminal.Terminal.Log($"Centrifuge version: {centrifugeAssembly.GetName().Version.ToString()}");
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

                    Logger.Info($"Transpiler: {type.FullName}");
                    transpiler.Apply(HarmonyInstance);
                }
            }
        }
    }
}
