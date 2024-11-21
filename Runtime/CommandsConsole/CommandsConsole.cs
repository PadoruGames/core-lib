using System;
using System.Linq;
using System.Text;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.DebugConsole
{
	public class CommandsConsole : IGUIItem
	{
		private const string TEXT_FIELD_CONTROL_NAME = "ConsoleTextField";

		private readonly CommandsConsoleConfig config;
		
		private bool showConsole;
		private string input;
		private GUIStyle textFieldStyle;

		public CommandsConsole(CommandsConsoleConfig config)
		{
			this.config = config;
			
			var sb = new StringBuilder();
			sb.Append("Commands console available commands:");
			sb.Append(Environment.NewLine);

			foreach (var command in config.Commands)
			{
				sb.Append($"  - {command.Key}");
				sb.Append(Environment.NewLine);
			}

			Debug.Log(sb, DebugChannels.COMMANDS_CONSOLE);
		}

		public void OnGUI()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == config.ToggleConsoleKey)
			{
				ToggleConsole();
			}

			if (Event.current.type == EventType.KeyDown && config.HandleInputKeys.Contains(Event.current.keyCode))
			{
				HandleInput();
				input = string.Empty;
				showConsole = false;
			}

			DrawConsole();
			FocusConsole();
		}

		private void DrawConsole()
		{
			if (textFieldStyle == null)
			{
				textFieldStyle = new GUIStyle(GUI.skin.textField)
				{
					fontSize = 24,
					alignment = TextAnchor.UpperLeft
				};
			}
			
			if (!showConsole)
			{
				return;
			}

			var y = 0f;

			GUI.Box(new Rect(0, y, Screen.width, 50f), "");
			GUI.backgroundColor = new Color(0, 0, 0, 0);

			GUI.SetNextControlName(TEXT_FIELD_CONTROL_NAME);
			input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 50f), input, textFieldStyle);

			if (input == "`")
			{
				input = string.Empty;
			}
		}

		private void ToggleConsole()
		{
			showConsole = !showConsole;

			if (showConsole)
			{
				input = string.Empty;
			}
		}

		private void FocusConsole()
		{
			if (!showConsole || GUI.GetNameOfFocusedControl() == TEXT_FIELD_CONTROL_NAME)
			{
				return;
			}

			GUI.FocusControl(TEXT_FIELD_CONTROL_NAME);
		}

		private void HandleInput()
		{
			if (!showConsole)
			{
				return;
			}

			var parameters = input.Split(' ');
			if(parameters.Length <= 0)
			{
				return;
			}

			var args = parameters.Skip(1).ToArray();
			if (config.Commands.TryGetValue(parameters[0].ToLower(), out var command))
			{
				command.Execute(args);
			}
		}
	}
}
