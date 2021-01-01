using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandResetWallColor : Command {
		
		protected override void Execute(string[] args) {
			
			if (Plugin.instance.inLevel)
				ColorController.instance.DisableWallColor();
			
		}
		
	}
	
}