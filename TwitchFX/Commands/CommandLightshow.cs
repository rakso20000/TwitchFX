namespace TwitchFX.Commands {
	
	public class CommandLightshow : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<name>");
			
			string[] args = ParseArgs(argsStr, 1);
			string name = args[0];
			
			if (!Plugin.instance.inLevel) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			bool success = LightController.instance.ShowCustomLightshow(name);
			
			if (!success)
				Plugin.chat.Send("Lightshow with name " + name + " not found");
			
		}
		
	}
	
}