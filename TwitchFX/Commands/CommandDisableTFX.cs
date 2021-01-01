using TwitchFX.Lights;
using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandDisableTFX : Command {
		
		public CommandDisableTFX() {
			
			requireEnabled = false;
			requireInLevel = false;
			
		}
		
		protected override void Execute(string[] args) {
			
			if (!Plugin.instance.enabled) {
				
				Plugin.chat.Send("TwitchFX is already disabled");
				
				return;
				
			}
			
			Plugin.instance.enabled = false;
			
			if (!ColorController.isNull)
				ColorController.instance.Reset();
			
			if (!LightController.isNull)
				LightController.instance.Reset();
			
			Plugin.chat.Send("Disabled TwitchFX");
			
		}
		
	}
	
}