using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandSpinRings : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			ParseArgs(argsStr, 0);
			
			RingController.instance.Spin();
			
		}
		
	}
	
}