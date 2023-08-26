using System.Linq;
using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class InputFieldDrawer : IInputFieldDrawer
	{
		private const float INPUT_FIELD_MARGIN = 5;

		private string[] availableCommands;
		
		public string Input { get; private set; }

		public InputFieldDrawer(string[] availableCommands)
		{
			this.availableCommands = availableCommands;
		}
		
		public void Draw(Rect box)
		{
			GUI.Box(box, "");
			
			GUI.backgroundColor = new Color(0, 0, 0, 0);

			GUI.SetNextControlName(Console.INPUT_FIELD_CONTROL_NAME);
			
			Input = GUI.TextField(box.AddMargin(INPUT_FIELD_MARGIN), Input);

			// WIP
			//DrawSuggestion();
			
			if (Input == "`")
			{
				ClearInput();
			}
		}

		public void ClearInput()
		{
			Input = string.Empty;
		}

		private void DrawSuggestion()
		{
			if (string.IsNullOrWhiteSpace(Input))
			{
				return;
			}
			
			var suggestedCommand = availableCommands.FirstOrDefault(s => s.Contains(Input));
				
			if (string.IsNullOrWhiteSpace(suggestedCommand))
			{
				return;
			}
			
			var currentColor = GUI.contentColor;
			GUI.contentColor = Color.gray;
			
			var widthOfInput = GUI.skin.textField.CalcSize(new GUIContent(Input)).x;
			GUILayout.Label(suggestedCommand.Substring(Input.Length), GUILayout.Width(400 - widthOfInput));
			
			GUI.contentColor = currentColor;
		}
	}
}
