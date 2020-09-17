using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace TwitchFX.Configuration {
	
	internal class PluginConfig {
		
		public static PluginConfig Instance { get; set; }
		
		[UseConverter(typeof(DictionaryConverter<string>))]
		public virtual Dictionary<string, string> commands { get; set; } = new Dictionary<string, string>();
		
	}
	
}