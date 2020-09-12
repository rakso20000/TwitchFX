using IPA;
using IPA.Config;
using IPA.Config.Stores;
using TwitchFX.Commands;
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
			
			Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
			Logger.log.Debug("Config loaded");
			
			InitCommands();
			
			chat = new Chat();
			
		}
		
		private void InitCommands() {
			
			new CommandSetLightColor();
			new CommandRestoreLightColor();
			new CommandDisableLights();
			
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