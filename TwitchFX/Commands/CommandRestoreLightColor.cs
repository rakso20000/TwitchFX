namespace TwitchFX.Commands {
	
	class CommandRestoreLightColor : Command {
		
		public override void Execute(string args) {
			
			SetUsage("!restorelightcolor");
			
			ParseArgs(args, 0);
			
			LightController.instance.colorLeft?.Disable();
			LightController.instance.colorRight?.Disable();
			LightController.instance.highlightcolorLeft?.Disable();
			LightController.instance.highlightcolorRight?.Disable();
			
		}
		
	}
	
}