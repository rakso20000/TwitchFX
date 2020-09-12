namespace TwitchFX.Commands {
	
	class CommandDisableLights : Command {
		
		public override void Execute(string args) {
			
			SetUsage("!disablelights");
			
			ParseArgs(args, 0);
			
			if (LightController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			LightController.instance.colorLeft.SetMode(ColorMode.Disabled);
			LightController.instance.colorRight.SetMode(ColorMode.Disabled);
			LightController.instance.highlightcolorLeft.SetMode(ColorMode.Disabled);
			LightController.instance.highlightcolorRight.SetMode(ColorMode.Disabled);
			
		}
		
	}
	
}