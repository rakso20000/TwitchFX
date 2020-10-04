namespace TwitchFX.Commands {
	
	public class CommandRestoreLights : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			LightController.instance?.UpdateLights(ColorMode.Default);
			
		}
		
	}
	
}