namespace TwitchFX.Commands {
	
	class CommandRestoreLights : Command {
		
		public override void Execute(string args) {
			
			SetUsage("!restorelightcolor");
			
			ParseArgs(args, 0);
			
			LightController.instance?.UpdateLights(ColorMode.Default);
			
		}
		
	}
	
}