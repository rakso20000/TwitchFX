using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandBoostLights : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("<duration>");
			
			string[] args = ParseArgs(argsStr, 1);
			
			float duration = TryParseFloat(args, 0).Value;
			
			LightController.instance.BoostLights(duration);
			
		}
		
	}
	
}