using TwitchFX.Lights;
using TwitchFX.Colors;

namespace TwitchFX.Commands {
	
	public class CommandResetTFX : Command {
		
		protected override void Execute(string[] args) {
			
			if (!ColorController.isNull)
				ColorController.instance.Reset();
			
			if (!LightController.isNull)
				LightController.instance.Reset();
			
		}
		
	}
	
}