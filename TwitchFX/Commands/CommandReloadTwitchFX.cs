namespace TwitchFX.Commands {
	
	public class CommandReloadTwitchFX : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			Plugin.instance.Reload();
			
			Plugin.chat.Send("Reloaded TwitchFX");
			
		}
		
	}
	
}