namespace TwitchFX.Commands {
	
	public class CommandRestoreLights : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			if (Plugin.instance.inLevel)
				LightController.instance.SetColorMode(ColorMode.Default);
			
		}
		
	}
	
}