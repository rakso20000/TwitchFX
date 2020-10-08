namespace TwitchFX.Commands {
	
	public class CommandBoostLights : Command {
		
		public override void Execute(string argsStr) {
			
			SetUsage("<duration>");
			
			string[] args = ParseArgs(argsStr, 1);
			
			float duration = TryParseFloat(args, 0);
			
			if (LightController.instance == null) {
				
				Plugin.chat.Send("Please use this command during a song");
				
				return;
				
			}
			
			LightController.instance.BoostLights(duration);
			
		}
		
	}
	
}