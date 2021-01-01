using TwitchFX.Lights;

namespace TwitchFX.Commands {
	
	public class CommandLightshow : Command {
		
		public CommandLightshow() {
			
			usage = "<name>";
			
			SetArgsCount(1);
			
		}
		
		protected override void Execute(string[] args) {
			
			string name = args[0];
			
			bool success = LightController.instance.ShowCustomLightshow(name);
			
			if (!success)
				Plugin.chat.Send("Lightshow with name " + name + " not found");
			
		}
		
	}
	
}