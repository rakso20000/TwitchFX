using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandDisableLights : Command {
		
		public CommandDisableLights() {
			
			usage = "OR\n" +
			"<duration in seconds>";
			
			SetArgsCount(0, 1);
			
		}
		
		protected override void Execute(string[] args) {
			
			float? duration = TryParseFloat(args, 0);
			
			LightController.instance.SetLightMode(LightMode.Disabled, duration);
			
		}
		
	}
	
}