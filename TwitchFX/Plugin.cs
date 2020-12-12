using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Utilities;
using IPA.Loader;
using TwitchFX.Configuration;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using ChatCore.Utilities;
using TwitchFX.Lights;
using TwitchFX.Colors;
using TwitchFX.Commands;
using TwitchFX.Hooking;
using UnityEngine;

namespace TwitchFX {
	
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin {
		
		public static ChatController chat;
		
		internal static Plugin instance { get; private set; }
		internal static string Name => "TwitchFX";
		
		public string version { get; private set; }
		
		public bool inLevel = false;
		
		private Assembly assembly;
		
		[Init]
		public void Init(IPALogger logger, Config conf, PluginMetadata plugin) {
			
			SemVer.Version semver = plugin.Version;
			
			version = semver.Major + "." + semver.Minor + "." + semver.Patch;
			
			instance = this;
			Logger.log = logger;
			Logger.log.Debug("Logger initialized.");
			
			PluginConfig.instance = conf.Generated<PluginConfig>();
			Logger.log.Debug("Config loaded");
			
			if (PluginConfig.instance.commands == null)
				PluginConfig.instance.commands = new Dictionary<string, string>();
			
			if (PluginConfig.instance.commandsRequiredPermissions == null)
				PluginConfig.instance.commandsRequiredPermissions = new Dictionary<string, string>();
			
			assembly = Assembly.GetExecutingAssembly();
			
			using (PluginConfig.instance.ChangeTransaction()) {
				
				foreach (Type type in assembly.GetTypes())
					if (type.IsSubclassOf(typeof(Command)))
						Activator.CreateInstance(type);
				
			}
			
		}
		
		[OnStart]
		public void OnStart() {
			
			chat = new GameObject("TwitchFXChat").AddComponent<ChatController>();
			
			SetUpHooks();
			
			LoadColorPresets(UnityGame.UserDataPath + "\\TwitchFX\\ColorPresets");
			LoadLightshows(UnityGame.UserDataPath + "\\TwitchFX\\Lightshows");
			
		}
		
		private void SetUpHooks() {
			
			HookManager.instance.HookAll(assembly);
			
			HookManager.instance.BindOnCreation<BeatEffectSpawner>();
			
			HookManager.instance.BindOnCreation<LightSwitchEventEffect>(true);
			HookManager.instance.BindOnCreation<TrackLaneRingsRotationEffectSpawner>(true);
			HookManager.instance.BindOnCreation<TrackLaneRingsPositionStepEffectSpawner>(true);
			HookManager.instance.BindOnCreation<LightRotationEventEffect>(true);
			HookManager.instance.BindOnCreation<LightPairRotationEventEffect>(true);
			
		}
		
		private void LoadColorPresets(string colorPresetFolderPath) {
			
			if (!Directory.Exists(colorPresetFolderPath))
				Directory.CreateDirectory(colorPresetFolderPath);
			
			string[] colorPresetFilePaths = Directory.GetFiles(colorPresetFolderPath);
			
			foreach (string colorPresetFilePath in colorPresetFilePaths) {
				
				string name = Path.GetFileName(colorPresetFilePath);
				
				if (!name.EndsWith(".json"))
					continue;
				
				name = name.Substring(0, name.Length - 5);
				
				string json = File.ReadAllText(colorPresetFilePath);
				
				JSONNode rootJSON;
					
				try {
					
					rootJSON = JSON.Parse(json);
					
				} catch (Exception e) {
					
					Logger.log.Error("Failed loading color preset: " + name);
					Logger.log.Error("\t" + e.Message);
					
					continue;
					
				}
				
				ColorPreset colorPreset;
				
				try {
					
					colorPreset = ColorPreset.LoadColorPresetFromJSON(rootJSON);
					
				} catch (InvalidJSONException e) {
					
					Logger.log.Error("Failed loading color preset: " + name);
					Logger.log.Error("\tInvalid JSON data: " + e.errorMessage);
					
					continue;
					
				}
				
				ColorPreset.SetColorPreset(name, colorPreset);
				
			}
			
			Logger.log.Info(ColorPreset.GetColorPresetCount() + " color presets loaded from " + colorPresetFolderPath);
			
		}
		
		private void LoadLightshows(string lightshowFolderPath) {
			
			if (!Directory.Exists(lightshowFolderPath))
				Directory.CreateDirectory(lightshowFolderPath);
			
			string[] lightshowFilePaths = Directory.GetFiles(lightshowFolderPath);
			
			foreach (string lightshowFilePath in lightshowFilePaths) {
				
				string name = Path.GetFileName(lightshowFilePath);
				
				if (!name.EndsWith(".json"))
					continue;
				
				name = name.Substring(0, name.Length - 5);
				
				string json = File.ReadAllText(lightshowFilePath);
				
				JSONNode rootJSON;
				
				try {
					
					rootJSON = JSON.Parse(json);
					
				} catch (Exception e) {
					
					Logger.log.Error("Failed loading lightshow: " + name);
					Logger.log.Error("\t" + e.Message);
					
					continue;
					
				}
				
				CustomLightshowData lightshow;
				
				try {
					
					lightshow = CustomLightshowData.LoadLightshowDataFromJSON(rootJSON, name);
					
				} catch (InvalidJSONException e) {
					
					Logger.log.Error("Failed loading lightshow: " + name);
					Logger.log.Error("\tInvalid JSON data: " + e.errorMessage);
					
					continue;
					
				}
				
				CustomLightshowData.SetLightshowData(name, lightshow);
				
			}
			
			Logger.log.Info(CustomLightshowData.GetLightshowDataCount() + " lightshows loaded from " + lightshowFolderPath);
			
		}
		
		[OnEnable]
		public void OnEnable() {
			
			SceneManager.activeSceneChanged += OnSceneChanged;
			
		}
		
		[OnDisable]
		public void OnDisable() {
			
			SceneManager.activeSceneChanged -= OnSceneChanged;
			
		}
		
		public void OnSceneChanged(Scene prevScene, Scene nextScene) {
			
			if (nextScene.name == "GameCore") {
				
				inLevel = true;
				
				new GameObject("TwitchFXInjector").AddComponent<Injector>();
				
			} else {
				
				inLevel = false;
				
			}
			
		}
		
	}
	
}