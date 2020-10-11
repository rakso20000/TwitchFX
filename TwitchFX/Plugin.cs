using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Utilities;
using TwitchFX.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using IPALogger = IPA.Logging.Logger;
using ChatCore.SimpleJSON;

namespace TwitchFX {
	
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin {
		
		public static Chat chat;
		
		internal static Plugin instance { get; private set; }
		internal static string Name => "TwitchFX";
		
		[Init]
		public void Init(IPALogger logger, Config conf) {
			
			instance = this;
			Logger.log = logger;
			Logger.log.Debug("Logger initialized.");
			
			PluginConfig.instance = conf.Generated<PluginConfig>();
			Logger.log.Debug("Config loaded");
			
			if (PluginConfig.instance.commands == null)
				PluginConfig.instance.commands = new Dictionary<string, string>();
			
			foreach (Type type in Assembly.GetAssembly(typeof(Command)).GetTypes())
				if (type.IsSubclassOf(typeof(Command)))
					Activator.CreateInstance(type);
			
			chat = new Chat();
			
		}
		
		[OnStart]
		public void OnStart() {
			
			Harmony harmony = new Harmony("com.rakso20000.beatsaber.twitchfx");
			
			harmony.PatchAll();
			
			LoadColorPresets(UnityGame.UserDataPath + "\\TwitchFX\\ColorPresets");
			
			string lightshowFolderPath = UnityGame.UserDataPath + "\\TwitchFX\\Lightshows";
			
			if (!Directory.Exists(lightshowFolderPath))
				Directory.CreateDirectory(lightshowFolderPath);
			
			string[] lightshowFilePaths = Directory.GetFiles(lightshowFolderPath);
			
			foreach (string lightshowFilePath in lightshowFilePaths) {
				
				string name = Path.GetFileName(lightshowFilePath);
				
				if (!name.EndsWith(".json") && !name.EndsWith(".dat"))
					continue;
				
				name = name.Substring(0, name.Length - (name.EndsWith(".json") ? 5 : 4));
				
				CustomLightshowData lightshow = CustomLightshowData.LoadLightshowDataFromFile(lightshowFilePath);
				
				if (lightshow == null) {
					
					Logger.log.Error("Failed loading lightshow: " + name);
					
					continue;
					
				}
				
				CustomLightshowData.SetLightshowData(name, lightshow);
				
			}
			
			Logger.log.Info(CustomLightshowData.GetLightshowDataCount() + " lightshows loaded from " + lightshowFolderPath);
			
		}
		
		private void LoadColorPresets(string colorPresetFolderPath) {
			
			if (!Directory.Exists(colorPresetFolderPath))
				Directory.CreateDirectory(colorPresetFolderPath);
			
			string[] colorPresetFilePaths = Directory.GetFiles(colorPresetFolderPath);
			
			foreach (string colorPresetFilePath in colorPresetFilePaths) {
				
				string name = Path.GetFileName(colorPresetFilePath);
				
				if (!name.EndsWith(".json") && !name.EndsWith(".dat"))
					continue;
				
				name = name.Substring(0, name.Length - (name.EndsWith(".json") ? 5 : 4));
				
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
				
				Logger.log.Info(colorPreset.leftSaberColor.r + ", " + colorPreset.leftSaberColor.g + ", " + colorPreset.leftSaberColor.b);
				
				ColorPreset.SetColorPreset(name, colorPreset);
				
			}
			
			Logger.log.Info(ColorPreset.GetColorPresetCount() + " color presets loaded from " + colorPresetFolderPath);
			
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
				
				new GameObject("TwitchFXLightController").AddComponent<LightController>();
				new GameObject("TwitchFXColorController").AddComponent<ColorController>();
				
			}
			
		}
		
	}
	
}