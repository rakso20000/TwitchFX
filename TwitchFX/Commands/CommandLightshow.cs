using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandLightshow : Command {
		
		public override void Execute(string argsStr) {
			
			RequireInLevel();
			
			SetUsage("<name>");
			
			string[] args = ParseArgs(argsStr, 1);
			string name = args[0];
			
			bool success = LightController.instance.ShowCustomLightshow(name);
			
			if (!success)
				Plugin.chat.Send("Lightshow with name " + name + " not found");
			
		}
		
	}
	
}