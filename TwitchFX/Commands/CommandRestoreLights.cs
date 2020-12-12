using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandRestoreLights : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			ParseArgs(argsStr, 0);
			
			if (Plugin.instance.inLevel)
				LightController.instance.SetLightMode(LightMode.Default);
			
		}
		
	}
	
}