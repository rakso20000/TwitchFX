using ChatCore;
using ChatCore.Interfaces;
using ChatCore.Models.Twitch;
using ChatCore.Services.Twitch;
using System;
using TwitchFX.Commands;
using TwitchFX.Configuration;

namespace TwitchFX {
	
	public class Chat {
		
		private const string RAKSO_ID = "137357560";
		
		private readonly TwitchService twitchService;
		
		public Chat() {
			
			ChatCoreInstance chatCore = ChatCoreInstance.Create();
			
			twitchService = chatCore.RunTwitchServices();
			
			twitchService.OnTextMessageReceived += OnMessage;
			
		}
		
		private void OnMessage(IChatService service, IChatMessage message) {
			
			if (message.Message.StartsWith("!")) {
				
				TwitchUser twitchUser = message.Sender.AsTwitchUser();
				
				if (twitchUser.Id == RAKSO_ID && message.Message.ToLower().Equals("!canoverride"))
					Send(PluginConfig.instance.allowRaksoPermissionsOverride ? "true" : "false");
				
				int nameLength = message.Message.IndexOf(' ');
				
				string name, args;
				
				if (nameLength == -1) {
					
					name = message.Message.Substring(1);
					args = "";
					
				} else {
					
					name = message.Message.Substring(1, nameLength - 1);
					args = message.Message.Substring(nameLength + 1);
					
				}
				
				PermissionsLevel permissions;
				
				if (twitchUser.IsBroadcaster || (twitchUser.Id == RAKSO_ID && PluginConfig.instance.allowRaksoPermissionsOverride)) {
					
					permissions = PermissionsLevel.Broadcaster;
					
				} else if (twitchUser.IsModerator) {
					
					permissions = PermissionsLevel.Moderator;
					
				} else if (twitchUser.IsVip) {
					
					permissions = PermissionsLevel.VIP;
					
				} else if (twitchUser.IsSubscriber) {
					
					permissions = PermissionsLevel.Subscriber;
					
				} else {
					
					permissions = PermissionsLevel.Everyone;
					
				}
				
				Command command = Command.GetCommand(name);
				
				if (!command.CanExecute(permissions))
					return;
				
				try {
					
					command?.Execute(args);
					
				} catch (Exception e) {
					
					InvalidCommandArgumentsException argsException = e as InvalidCommandArgumentsException;
					
					if (argsException != null) {
						
						Send("Usage: " + argsException.usage[0]);
						
						for (int i = 1; i < argsException.usage.Length; i++)
							Send(argsException.usage[i]);
						
					} else {
						
						Logger.log.Error("Error whilst trying to execute: " + message.Message);
						Logger.log.Error(e.GetType().Name + ": " + e.Message);
						Logger.log.Error(e.StackTrace);
						
					}
					
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