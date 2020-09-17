namespace TwitchFX.Commands {
	
	public class CommandRestoreLights : Command {
		
		public override void Execute(string args) {
			
			ParseArgs(args, 0);
			
			LightController.instance?.CancelDisable();
			LightController.instance?.UpdateLights(ColorMode.Default);
			
		}
		
	}
	
}