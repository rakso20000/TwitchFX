using TwitchFX.Lights;
using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandResetTFX : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			ParseArgs(argsStr, 0);
			
			if (!ColorController.isNull)
				ColorController.instance.Reset();
			
			if (!LightController.isNull)
				LightController.instance.Reset();
			
		}
		
	}
	
}