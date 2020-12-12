using UnityEngine;
using TwitchFX.Lights;
using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandDisableTwitchFX : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			if (!Plugin.instance.enabled) {
				
				Plugin.chat.Send("TwitchFX is already disabled");
				
				return;
				
			}
			
			Plugin.instance.enabled = false;
			
			if (!ColorController.isNull) {
				
				ColorController.instance.DisableSaberColors();
				ColorController.instance.DisableNoteColors();
				ColorController.instance.DisableWallColor();
				
			}
			
			if (!LightController.isNull) {
				
				LightController.instance.SetLightMode(LightMode.Default);
				
				if (LightController.instance.lightshowController != null)
					Object.Destroy(LightController.instance.lightshowController);
				
			}
			
			Plugin.chat.Send("Disabled TwitchFX");
			
		}
		
	}
	
}