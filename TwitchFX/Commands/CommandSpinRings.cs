using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandSpinRings : Command {
		
		protected override void Execute(string[] args) {
			
			RingController.instance.Spin();
			
		}
		
	}
	
}