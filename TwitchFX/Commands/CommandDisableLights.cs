namespace TwitchFX.Commands {
	
	public class CommandDisableLights : Command {
		
		public override void Execute(string args) {
			
			SetUsage("!disablelights");
			
			ParseArgs(args, 0);
			
			if (LightController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			LightController.instance.CancelDisable();
			LightController.instance.UpdateLights(ColorMode.Disabled);
			
		}
		
	}
	
}