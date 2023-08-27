using System;
using System.Collections.Generic;
using UnityEngine;
using LogType = Padoru.Diagnostics.LogType;

namespace Padoru.Core.Diagnostics
{
	public class LogEntryDrawer
	{
		private GUIStyle textAreaStyle;
		private GUIStyle iconStyle;
		private Dictionary<LogType, Texture2D> logTypeTextures = new();
		
		public LogEntryDrawer()
		{
			textAreaStyle = new GUIStyle
			{
				stretchWidth = true,
				fixedHeight = ConsoleConstants.LOG_ENTRY_HEIGHT,
				alignment = TextAnchor.MiddleLeft,
				normal =
				{
					textColor = Color.white,
				},
				padding = new RectOffset(ConsoleConstants.LOG_ENTRY_PADDING, ConsoleConstants.LOG_ENTRY_PADDING, 0, 0),
			};
			
			iconStyle = new GUIStyle
			{
				fixedHeight = ConsoleConstants.LOG_ENTRY_HEIGHT,
				fixedWidth = ConsoleConstants.LOG_ENTRY_HEIGHT,
				padding = new RectOffset(ConsoleConstants.LOG_ENTRY_PADDING, 
										 ConsoleConstants.LOG_ENTRY_PADDING, 
										 ConsoleConstants.LOG_ENTRY_PADDING, 
										 ConsoleConstants.LOG_ENTRY_PADDING),
			};
			
			var infoTex = Resources.Load<Texture2D>("info");
			var warningTex = Resources.Load<Texture2D>("warning");
			var errorTex = Resources.Load<Texture2D>("error");
			
			logTypeTextures.Add(LogType.Info, infoTex);
			logTypeTextures.Add(LogType.Warning, warningTex);
			logTypeTextures.Add(LogType.Error, errorTex);
			logTypeTextures.Add(LogType.Exception, errorTex);
		}
		
		public void DrawEntry(ConsoleEntry entry, int index)
		{
			GUILayout.Label(logTypeTextures[entry.logType], iconStyle);
			
			GUILayout.Button(GetFirstLine(entry.message.ToString()), textAreaStyle);
		}
		
		string GetFirstLine(string text)
		{
			string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			return lines.Length > 0 ? lines[0] : string.Empty;
		}
	}
}
