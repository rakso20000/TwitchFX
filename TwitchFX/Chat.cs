using ChatCore;
using ChatCore.Interfaces;
using ChatCore.Services.Twitch;
using System;

namespace TwitchFX {
	
	public class Chat {
		
		private readonly TwitchService twitchService;
		
		public Chat() {
			
			ChatCoreInstance chatCore = ChatCoreInstance.Create();
			
			twitchService = chatCore.RunTwitchServices();
			
			twitchService.OnTextMessageReceived += OnMessage;
			
		}
		
		private void OnMessage(IChatService service, IChatMessage message) {
			
			if (message.Message.StartsWith("!")) {
				
				int nameLength = message.Message.IndexOf(' ');
				
				string name, args;
				
				if (nameLength == -1) {
					
					name = message.Message.Substring(1);
					args = "";
					
				} else {
					
					name = message.Message.Substring(1, nameLength - 1);
					args = message.Message.Substring(nameLength + 1);
					
				}
				
				try {
					
					Command.GetCommand(name)?.Execute(args);
					
				} catch (Exception e) {
					
					Logger.log.Error("Error whilst trying to execute: " + message.Message);
					Logger.log.Error(e.GetType().Name + ": " + e.Message);
					Logger.log.Error(e.StackTrace);
					
				}
				
			}
			
		}
		
		public void Send(string message) {
			
			foreach (IChatChannel channel in twitchService.Channels.Values) {
				
				twitchService.SendTextMessage(message, channel);
				
			}
			
		}
		
	}
	
}