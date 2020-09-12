namespace TwitchFX.Commands {
	
	class CommandRestoreLights : Command {
		
		public override void Execute(string args) {
			
			SetUsage("!restorelightcolor");
			
			ParseArgs(args, 0);
			
			LightController.instance.colorLeft?.SetMode(ColorMode.Default);
			LightController.instance.colorRight?.SetMode(ColorMode.Default);
			LightController.instance.highlightcolorLeft?.SetMode(ColorMode.Default);
			LightController.instance.highlightcolorRight?.SetMode(ColorMode.Default);
			
		}
		
	}
	
}