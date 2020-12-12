using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandDisableLights : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("OR\n" +
			"<duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 0, 1);
			
			float? duration = TryParseFloat(args, 0);
			
			LightController.instance.SetLightMode(LightMode.Disabled, duration);
			
		}
		
	}
	
}