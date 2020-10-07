namespace TwitchFX.Commands {
	
	public class CommandResetNoteColor : Command {
		
		public override void Execute(string argsStr) {
			
			ParseArgs(argsStr, 0);
			
			ColorController.instance?.DisableNoteColors();
			
		}
		
	}
	
}