namespace TwitchFX.Commands {
	
	public class CommandReloadTFX : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			Plugin.instance.Reload();
			
			Plugin.chat.Send("Reloaded TwitchFX");
			
		}
		
	}
	
}