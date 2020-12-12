using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandResetWallColor : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			ParseArgs(argsStr, 0);
			
			if (Plugin.instance.inLevel)
				ColorController.instance.DisableWallColor();
			
		}
		
	}
	
}