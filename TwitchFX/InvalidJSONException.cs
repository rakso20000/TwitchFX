using System;

namespace TwitchFX {
	
	public class InvalidJSONException : Exception {
		
		public readonly string errorMessage;
		
		public InvalidJSONException(string errorMessage) {
			
			this.errorMessage = errorMessage;
			
		}
		
	}
	
}