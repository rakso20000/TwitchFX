using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace TwitchFX.Configuration {
	
	internal class PluginConfig {
		
		public static PluginConfig Instance { get; set; }
		public virtual int IntValue { get; set; } = 1337;
		
	}
	
}