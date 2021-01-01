using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandRestoreLights : Command {
		
		protected override void Execute(string[] args) {
			
			if (Plugin.instance.inLevel)
				LightController.instance.SetLightMode(LightMode.Default);
			
		}
		
	}
	
}