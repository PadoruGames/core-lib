using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LogType = Padoru.Diagnostics.LogType;

namespace Padoru.Core.Diagnostics
{
	public class Console
	{
		public const string INPUT_FIELD_CONTROL_NAME = "ConsoleInputField";
		public const string USER_INPUT_CHANNEL = "UserInput";
		
		private readonly Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> commands;
		private readonly IHeaderDrawer headerDrawer;
		private readonly ILogsAreaDrawer logsAreaDrawer;
		private readonly IInputFieldDrawer inputFieldDrawer;
		
		private bool showConsole;

		public Console(
			IHeaderDrawer headerDrawer, 
			ILogsAreaDrawer logsAreaDrawer, 
			IInputFieldDrawer inputFieldDrawer,
			Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> commands)
		{
			this.headerDrawer = headerDrawer;
			this.logsAreaDrawer = logsAreaDrawer;
			this.inputFieldDrawer = inputFieldDrawer;
			this.commands = commands;
		}

		public void ToggleConsole()
		{
			showConsole = !showConsole;

			if (showConsole)
			{
				inputFieldDrawer.ClearInput();
			}
		}
		
		public void Draw()
		{
			if (!showConsole)
			{
				return;
			}

			var consoleAreaStyle = new GUIStyle(GUI.skin.box)
			{
				fixedWidth = Screen.width
			};

			GUILayout.BeginVertical(consoleAreaStyle);

			//headerDrawer.Draw();
			logsAreaDrawer.Draw();
			inputFieldDrawer.Draw();
			
			GUILayout.EndVertical();
			
			// Force focus to the console while opened
			FocusConsole();
		}
		

		public void ProcessInput()
		{
			if (string.IsNullOrWhiteSpace(inputFieldDrawer.Input))
			{
				return;
			}

			Log(new LogEntry()
			{
				message = inputFieldDrawer.Input,
				channel = USER_INPUT_CHANNEL,
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
				command.Instance.Execute(args);
			}

			inputFieldDrawer.ClearInput();
		}

		public void Log(LogEntry logEntry)
		{
			logsAreaDrawer.AddLog(logEntry);
		}

		private void FocusConsole()
		{
			if (!showConsole || GUI.GetNameOfFocusedControl() == INPUT_FIELD_CONTROL_NAME)
			{
				return;
			}

			GUI.FocusControl(INPUT_FIELD_CONTROL_NAME);
		}
	}
}
