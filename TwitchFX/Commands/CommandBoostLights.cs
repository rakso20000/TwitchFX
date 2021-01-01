using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandBoostLights : Command {
		
		public CommandBoostLights() {
			
			usage = "<duration>";
			
			SetArgsCount(1);
			
		}
		
		protected override void Execute(string[] args) {
			
			float duration = TryParseFloat(args, 0).Value;
			
			LightController.instance.BoostLights(duration);
			
		}
		
	}
	
}