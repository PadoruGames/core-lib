using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class Console
	{
		public const string INPUT_FIELD_CONTROL_NAME = "ConsoleInputField";
		
		private readonly Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> commands;
		private readonly IHeaderDrawer headerDrawer;
		private readonly ITextAreaDrawer textAreaDrawer;
		private readonly IInputFieldDrawer inputFieldDrawer;
		
		private bool showConsole;
		private Rect headerRect;
		private Rect textAreaRect;
		private Rect inputFieldRect;
		private Rect consoleRect;
		private Rect inputFieldBox;

		public Console(
			IHeaderDrawer headerDrawer, 
			ITextAreaDrawer textAreaDrawer, 
			IInputFieldDrawer inputFieldDrawer,
			Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> commands)
		{
			this.headerDrawer = headerDrawer;
			this.textAreaDrawer = textAreaDrawer;
			this.inputFieldDrawer = inputFieldDrawer;
			this.commands = commands;

			CalculateRects();
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
			
			DrawBackground();
			headerDrawer.Draw(headerRect);
			textAreaDrawer.Draw(textAreaRect);
			inputFieldDrawer.Draw(inputFieldBox);
			
			// Force focus to the console while opened
			FocusConsole();
		}
		

		public void ProcessInput()
		{
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

		private void FocusConsole()
		{
			if (!showConsole || GUI.GetNameOfFocusedControl() == INPUT_FIELD_CONTROL_NAME)
			{
				return;
			}

			GUI.FocusControl(INPUT_FIELD_CONTROL_NAME);
		}

		private void DrawBackground()
		{
			GUI.Box(consoleRect, "");
		}

		private void CalculateRects()
		{
			var y = 0f;
			headerRect = new Rect(0, y, Screen.width, 30);
			y += headerRect.height;
			textAreaRect = new Rect(0, y, Screen.width, 300);
			y += textAreaRect.height;
			inputFieldRect = new Rect(0, y, Screen.width, 40);
			inputFieldBox = inputFieldRect.AddMargin(5);
			
			consoleRect = new Rect(
				0, 
				0, 
				Screen.width,
				headerRect.height + inputFieldRect.height + textAreaRect.height);
		}
	}
}
