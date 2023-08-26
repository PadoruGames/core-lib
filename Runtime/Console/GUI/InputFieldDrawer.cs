using System.Linq;
using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class InputFieldDrawer : IInputFieldDrawer
	{
		private readonly string[] availableCommands;
		
		public string Input { get; private set; }

		public InputFieldDrawer(string[] availableCommands)
		{
			this.availableCommands = availableCommands;
		}
		
		public void Draw()
		{
			GUI.SetNextControlName(Console.INPUT_FIELD_CONTROL_NAME);

			// For some reason when using a GUIStyle on this one, the TextField disappears. 
			// The background should be drawn here so we can stack the text and the suggestion horizontally
			GUILayout.BeginHorizontal(GUILayout.Height(25));
			
			var inputFieldStyle = new GUIStyle(GUI.skin.box)
			{
				alignment = TextAnchor.MiddleLeft,
				normal =
				{
					textColor = Color.white
				},
			};
			
			Input = GUILayout.TextField(Input, inputFieldStyle, GUILayout.ExpandHeight(true));

			//DrawSuggestion();
			
			GUILayout.EndHorizontal();
			
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
