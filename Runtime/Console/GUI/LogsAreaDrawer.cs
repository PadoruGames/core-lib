using System.Collections.Generic;
using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class LogsAreaDrawer : ILogsAreaDrawer
	{
		private List<ConsoleEntry> logs = new();
		private Vector2 scrollPosition = Vector2.zero;
		private Vector2 lastMousePosition = Vector2.zero;
		private LogEntryDrawer logEntryDrawer = new();
		private Texture2D bgTex;

		public LogsAreaDrawer()
		{
			bgTex = MakeTex(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.5f));
		}
		
		public void Draw()
		{
			var scrollViewStyle = new GUIStyle(GUI.skin.box)
			{
				stretchWidth = true,
				padding = new RectOffset(0, 0, 0, 0),
			};
			var horizontalStyle = new GUIStyle
			{
				stretchWidth = true,
				fixedHeight = ConsoleConstants.LOG_ENTRY_HEIGHT,
				alignment = TextAnchor.MiddleLeft,
				normal =
				{
					textColor = Color.white,
				},
				padding = new RectOffset(0, 0, 0, 0),
			};
			
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, scrollViewStyle, GUILayout.MaxHeight(250));

			for (int i = 0; i < logs.Count; i++)
			{
				horizontalStyle.normal.background = i % 2 == 0 ? bgTex : null;
				
				GUILayout.BeginHorizontal(horizontalStyle);
				
				logEntryDrawer.DrawEntry(logs[i], i);
				
				GUILayout.EndHorizontal();
			}
			
			GUILayout.EndScrollView();
			
			var scrollViewRect = GUILayoutUtility.GetLastRect();

			HandleMouseScrolling(scrollViewRect);
		}

		public void AddLog(ConsoleEntry console)
		{
			logs.Add(console);
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
	}
}
