namespace TwitchFX.Commands {
	
	public class CommandReloadTFX : Command {
		
		public CommandReloadTFX() {
			
			requireEnabled = false;
			requireInLevel = false;
			
		}
		
		protected override void Execute(string[] args) {
			
			Plugin.instance.Reload();
			
			Plugin.chat.Send("Reloaded TwitchFX");
			
		}
		
	}
	
}