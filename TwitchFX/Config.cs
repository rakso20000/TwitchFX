using System;
using System.Collections.Generic;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

namespace TwitchFX {
	
	public class Config {
		
		public virtual float minNoteColorDiff { get; set; } = 0.2f;
		public virtual float minSaberColorDiff { get; set; } = 0.2f;
		public virtual float minLightColorDiff { get; set; } = 0f;
		
		[UseConverter(typeof(DictionaryConverter<string>))]
		public virtual Dictionary<string, string> commands { get; set; } = new Dictionary<string, string>();
		
		[UseConverter(typeof(DictionaryConverter<PermissionsLevel, CaseInsensitiveEnumConverter<PermissionsLevel>>))]
		public virtual Dictionary<string, PermissionsLevel> commandsRequiredPermissions { get; set; } = new Dictionary<string, PermissionsLevel>();
		
		public virtual bool allowRaksoPermissionsOverride { get; set; } = true;
		
		public virtual IDisposable ChangeTransaction() {
			
			return null;
			
		}
		
	}
	
}