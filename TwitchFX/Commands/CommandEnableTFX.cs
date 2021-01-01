namespace TwitchFX.Commands {
	
	public class CommandEnableTFX : Command {
		
		public CommandEnableTFX() {
			
			requireEnabled = false;
			requireInLevel = false;
			
		}
		
		protected override void Execute(string[] args) {
			
			if (Plugin.instance.enabled) {
				
				Plugin.chat.Send("TwitchFX is already enabled");
				
				return;
				
			}
			
			Plugin.instance.enabled = true;
			
			Plugin.chat.Send("Enabled TwitchFX");
			
		}
		
	}
	
}