using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwitchFX.Commands {
	
	public abstract class Command {
		
		private static readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
		
		public static Command GetCommand(string name) {
			
			return commands.ContainsKey(name.ToLower()) ? commands[name.ToLower()] : null;
			
		}
		
		public static void ResetCommands() {
			
			commands.Clear();
			
		}
		
		private readonly PermissionsLevel requiredPermissions;
		private readonly string name;
		
		private int argsCount = 0;
		private int minArgsCount = -1;
		private int maxArgsCount = -1;
		
		protected string usage = "";
		protected bool requireEnabled = true;
		protected bool requireInLevel = true;
		
		protected Command() {
			
			string command = GetType().Name.Substring(7, GetType().Name.Length - 7).ToLower();
			
			if (!Plugin.config.commands.ContainsKey(command))
				Plugin.config.commands[command] = command;
			
			name = Plugin.config.commands[command];
			
			commands[name.ToLower()] = this;
			
			if (!Plugin.config.commandsRequiredPermissions.TryGetValue(command, out requiredPermissions))
				Plugin.config.commandsRequiredPermissions[command] = requiredPermissions = PermissionsLevel.Everyone;
			
		}
		
		public void TryExecute(string argsStr, PermissionsLevel permissions) {
			
			if (permissions < requiredPermissions)
				throw new InvalidCommandExecutionException("You're not allowed to use this command");
			
			if (requireEnabled && !Plugin.instance.enabled)
				throw new InvalidCommandExecutionException("TwitchFX is currently disabled");
			
			if (requireInLevel && !Plugin.instance.inLevel)
				throw new InvalidCommandExecutionException("Please use this command during a song");
			
			string[] args = argsStr.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			
			int length = args.Length;
			
			if (argsCount != -1 && length != argsCount || minArgsCount != -1 && length < minArgsCount || maxArgsCount != -1 && length > maxArgsCount)
				throw CreateInvalidArgs();
			
			Execute(args);
			
		}
		
		protected void SetArgsCount(int minArgsCount, int maxArgsCount = -1) {
			
			if (maxArgsCount != -1) {
				
				argsCount = -1;
				this.minArgsCount = minArgsCount;
				this.maxArgsCount = maxArgsCount;
				
				return;
				
			}
			
			argsCount = minArgsCount;
			
		}
		
		protected InvalidCommandArgumentsException CreateInvalidArgs() {
			
			string[] lines = usage.Split('\n');
			
			for (int i = 0; i < lines.Length; i++)
				lines[i] = "!" + name + " " + lines[i];
			
			throw new InvalidCommandArgumentsException(lines);
			
		}
		
		//returns null if the index doesn't exist and throws if the index does exist but is not a float
		protected float? TryParseFloat(string[] args, int index) {
			
			if (index >= args.Length)
				return null;
			
			if (!float.TryParse(args[index], out float f))
				throw CreateInvalidArgs();
			
			return f;
			
		}
		
		protected Color ParseColor(string colorStr) {
			
			if (!Helper.TryParseColor(colorStr, out Color color))
				throw CreateInvalidArgs();
			
			return color;
			
		}
		
		protected abstract void Execute(string[] args);
		
	}
	
}