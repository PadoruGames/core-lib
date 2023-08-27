using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LogType = Padoru.Diagnostics.LogType;

namespace Padoru.Core.Diagnostics
{
	public class Console
	{
		private readonly Dictionary<string, ConsoleCommand> commands;
		private readonly IHeaderDrawer headerDrawer;
		private readonly ILogsAreaDrawer logsAreaDrawer;
		private readonly IInputFieldDrawer inputFieldDrawer;
		
		private bool showConsole;

		public Console(
			IHeaderDrawer headerDrawer, 
			ILogsAreaDrawer logsAreaDrawer, 
			IInputFieldDrawer inputFieldDrawer,
			Dictionary<string, ConsoleCommand> commands)
		{
			this.headerDrawer = headerDrawer;
			this.logsAreaDrawer = logsAreaDrawer;
			this.inputFieldDrawer = inputFieldDrawer;
			this.commands = commands;
		}
		
		public void Draw()
		{
			CheckForKeysPressed();
			
			if (!showConsole)
			{
				return;
			}

			var consoleAreaStyle = new GUIStyle(GUI.skin.box)
			{
				fixedWidth = Screen.width
			};

			GUILayout.BeginVertical(consoleAreaStyle);

			headerDrawer.Draw();
			logsAreaDrawer.Draw();
			inputFieldDrawer.Draw();
			
			GUILayout.EndVertical();
			
			// Force focus to the console while opened
			FocusConsole();
		}

		public void Log(ConsoleEntry consoleEntry)
		{
			logsAreaDrawer.AddLog(consoleEntry);
		}

		private void ToggleConsole()
		{
			showConsole = !showConsole;

			if (showConsole)
			{
				inputFieldDrawer.ClearInput();
			}
		}
		
		private void ProcessInput()
		{
			if (string.IsNullOrWhiteSpace(inputFieldDrawer.Input))
			{
				return;
			}

			Log(new ConsoleEntry()
			{
				message = inputFieldDrawer.Input,
				channel = ConsoleConstants.USER_INPUT_CHANNEL,
				logType = LogType.Info
			});
			
			var parameters = inputFieldDrawer.Input.Split(' ');
			if(parameters.Length <= 0)
			{
				return;
			}

			var args = parameters.Skip(1).ToArray();
			var commandId = parameters[0].ToLower();
			if (commands.TryGetValue(commandId, out var command))
			{
				command.Execute(args);
			}

			inputFieldDrawer.ClearInput();
		}

		private void CheckForKeysPressed()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.BackQuote)
			{
				ToggleConsole();
			}
			
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				ProcessInput();
			}
		}

		private void FocusConsole()
		{
			if (!showConsole || GUI.GetNameOfFocusedControl() == ConsoleConstants.INPUT_FIELD_CONTROL_NAME)
			{
				return;
			}

			GUI.FocusControl(ConsoleConstants.INPUT_FIELD_CONTROL_NAME);
		}
	}
}
