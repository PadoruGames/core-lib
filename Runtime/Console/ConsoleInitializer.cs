using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Diagnostics
{
	public class ConsoleInitializer : MonoBehaviour
	{
		private Console console;

		private void Awake()
		{
			var commands = GetCommands();
			
			console = new Console(new HeaderDrawer(), new LogsAreaDrawer(), new InputFieldDrawer(commands.Keys.ToArray()), commands);
			
			var consoleOutput = new PadoruLoggerConsoleOutput(console);
			
			Debug.AddOutput(consoleOutput);
		}

		private void OnGUI()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.BackQuote)
			{
				console.ToggleConsole();
			}
			
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				console.ProcessInput();
			}
			
			console.Draw();
		}

		private Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> GetCommands()
		{
			var sb = new StringBuilder();
			sb.Append("Debug Commands Console initialized. Commands:");
			sb.Append(Environment.NewLine);

			var data = AttributeUtils.GetTypesWithAttributeInstanced<ConsoleCommand, ConsoleCommandAttribute>();
			var commands = new Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>>();

			foreach (var entry in data)
			{
				var commandName = entry.Attribute.CommandName.ToLower();
				commands.Add(commandName, entry);

				sb.Append($"  - {commandName}");
				sb.Append(Environment.NewLine);
			}
			
			Debug.Log(sb);

			return commands;
		}
	}
}
