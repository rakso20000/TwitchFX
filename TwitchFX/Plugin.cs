using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace TwitchFX {
	
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin {
		
		internal static Plugin instance { get; private set; }
		internal static string Name => "TwitchFX";
		
		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Config conf) {
			
			instance = this;
			Logger.log = logger;
			Logger.log.Debug("Logger initialized.");
			
			Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
			Logger.log.Debug("Config loaded");
			
		}
		
		[OnStart]
		public void OnApplicationStart() {
			
			Logger.log.Debug("OnApplicationStart");
			new GameObject("TwitchFXController").AddComponent<TwitchFXController>();
			
		}
		
		[OnExit]
		public void OnApplicationQuit() {
			
			Logger.log.Debug("OnApplicationQuit");
			
		}
		
	}
	
}