using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace TwitchFX.Configuration {
	
	internal class PluginConfig {
		
		public static PluginConfig instance { get; set; }
		
		[UseConverter(typeof(DictionaryConverter<string>))]
		public virtual Dictionary<string, string> commands { get; set; } = new Dictionary<string, string>();
		
		[UseConverter(typeof(DictionaryConverter<string>))]
		public virtual Dictionary<string, string> commandsRequiredPermissions { get; set; } = new Dictionary<string, string>();
		public virtual bool allowRaksoPermissionsOverride { get; set; } = true;
		
		public virtual IDisposable ChangeTransaction() {
			
			return null;
			
		}
		
	}
	
}