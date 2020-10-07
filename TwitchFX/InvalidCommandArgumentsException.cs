using System;

namespace TwitchFX {
	
	public class InvalidCommandArgumentsException : Exception {
		
		public readonly string[] usage;
		
		public InvalidCommandArgumentsException(string[] usage) {
			
			this.usage = usage;
			
		}
		
	}
	
}