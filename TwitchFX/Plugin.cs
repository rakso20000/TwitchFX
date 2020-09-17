using System;
using System.Collections.Generic;
using System.Reflection;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using TwitchFX.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

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
			
			PluginConfig.Instance = conf.Generated<PluginConfig>();
			Logger.log.Debug("Config loaded");
			
			if (PluginConfig.Instance.commands == null)
				PluginConfig.Instance.commands = new Dictionary<string, string>();
			
			foreach (Type type in Assembly.GetAssembly(typeof(Command)).GetTypes())
				if (type.IsSubclassOf(typeof(Command)))
					Activator.CreateInstance(type);
			
			chat = new Chat();
			
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
				
			}
			
		}
		
	}
	
}