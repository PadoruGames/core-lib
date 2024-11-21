using System;
using System.Collections.Generic;
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
		private List<string> commandHistory = new();
		private int historyIndex = -1;
		private bool moveCursorToEnd;

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
			
			if (Event.current.type == EventType.KeyDown && showConsole)
			{
				HandleHistoryNavigation(Event.current.keyCode);
			}

			DrawConsole();
			FocusConsole();
			MoveCursorToEndIfNeeded();
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
				historyIndex = -1;
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

			if (!string.IsNullOrWhiteSpace(input))
			{
				if (commandHistory.Count == 0 || commandHistory.Last() != input)
				{
					commandHistory.Add(input);
				}

				var parameters = input.Split(' ');
				var args = parameters.Skip(1).ToArray();
				if (config.Commands.TryGetValue(parameters[0].ToLower(), out var command))
				{
					command.Execute(args);
				}
			}
		}
		
		private void HandleHistoryNavigation(KeyCode keyCode)
		{
			if (keyCode == KeyCode.UpArrow)
			{
				if (historyIndex == -1)
				{
					historyIndex = commandHistory.Count - 1;
				}
				else if (historyIndex > 0)
				{
					historyIndex--;
				}

				if (historyIndex >= 0 && historyIndex < commandHistory.Count)
				{
					input = commandHistory[historyIndex];
					SetCursorToEndNextFrame();
				}
			}
			else if (keyCode == KeyCode.DownArrow)
			{
				if (historyIndex >= 0)
				{
					historyIndex++;
					if (historyIndex < commandHistory.Count)
					{
						input = commandHistory[historyIndex];
						SetCursorToEndNextFrame();
					}
					else
					{
						historyIndex = -1;
						input = string.Empty;
					}
				}
			}
		}
		
		private void SetCursorToEndNextFrame()
		{
			moveCursorToEnd = true;
		}

		private void MoveCursorToEndIfNeeded()
		{
			if (moveCursorToEnd)
			{
				moveCursorToEnd = false;

				// Force the cursor to the end of the input
				GUI.FocusControl(TEXT_FIELD_CONTROL_NAME);
				var textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null)
				{
					textEditor.cursorIndex = input.Length;
					textEditor.selectIndex = input.Length;
				}
			}
		}
	}
}
