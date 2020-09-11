using System.Collections.Generic;

namespace TwitchFX {
	
	public abstract class Command {
		
		private static readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
		
		public static Command GetCommand(string name) {
			
			return commands.ContainsKey(name.ToLower()) ? commands[name.ToLower()] : null;
			
		}
		
		protected Command() {
			
			string name = this.GetType().Name.Substring(7, this.GetType().Name.Length - 7).ToLower();
			
			commands[name.ToLower()] = this;
			
		}
		
		public abstract void Execute(string args);
		
	}
	
}