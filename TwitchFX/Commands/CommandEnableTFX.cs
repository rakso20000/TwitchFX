namespace TwitchFX.Commands {
	
	public class CommandEnableTFX : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			if (Plugin.instance.enabled) {
				
				Plugin.chat.Send("TwitchFX is already enabled");
				
				return;
				
			}
			
			Plugin.instance.enabled = true;
			
			Plugin.chat.Send("Enabled TwitchFX");
			
		}
		
	}
	
}