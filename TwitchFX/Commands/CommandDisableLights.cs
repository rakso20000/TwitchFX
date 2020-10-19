using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandDisableLights : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("OR\n" +
			"<duration in seconds>");
			
			string[] args = ParseArgs(argsStr, 0, 1);
			
			float duration = TryParseFloat(args, 0);
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			LightController.instance.SetLightMode(LightMode.Disabled);
			
			if (args.Length >= 1)
				LightController.instance.DisableIn(duration);
			
		}
		
	}
	
}