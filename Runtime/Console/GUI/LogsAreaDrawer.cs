using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class LogsAreaDrawer : ILogsAreaDrawer
	{
		private List<LogEntry> logs = new();
		private Vector2 scrollPosition = Vector2.zero;
		private Vector2 lastMousePosition = Vector2.zero;
		
		public void Draw()
		{
			var scrollViewStyle = new GUIStyle(GUI.skin.box)
			{
				stretchWidth = true,
				padding = new RectOffset(0, 0, 0, 0),
			};
			
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.MaxHeight(250));
			
			var bgTex = MakeTex(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.5f));
			var textAreaStyle = new GUIStyle()
			{
				stretchWidth = true,
				fixedHeight = 25,
				alignment = TextAnchor.MiddleLeft,
				normal =
				{
					textColor = Color.white,
				},
				padding = new RectOffset(5, 5, 0, 0),
			};

			for (int i = 0; i < logs.Count; i++)
			{
				textAreaStyle.normal.background = i % 2 == 0 ? bgTex : null;
				
				GUILayout.Button(GetFirstLine(logs[i].message.ToString()), textAreaStyle);
			}
			
			GUILayout.EndScrollView();
			
			var scrollViewRect = GUILayoutUtility.GetLastRect();
			
			Debug.LogError(scrollViewRect);

			HandleMouseScrolling(scrollViewRect);
		}

		public void AddLog(LogEntry log)
		{
			logs.Add(log);
		}
		
		void HandleMouseScrolling(Rect scrollViewRect)
		{
			if (Event.current.type == EventType.MouseDown && scrollViewRect.Contains(Event.current.mousePosition))
			{
				lastMousePosition = Event.current.mousePosition;
			}

			if (Event.current.type == EventType.MouseDrag)
			{
				var delta = Event.current.mousePosition - lastMousePosition;
				lastMousePosition = Event.current.mousePosition;

				scrollPosition -= delta;

				// Consume the event to avoid any other processing of the drag by other controls
				Event.current.Use();
			}
		}
		
		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];
			for (int i = 0; i < pix.Length; i++)
			{
				pix[i] = col;
			}

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}
		
		string GetFirstLine(string text)
		{
			string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			return lines.Length > 0 ? lines[0] : string.Empty;
		}
	}
}
