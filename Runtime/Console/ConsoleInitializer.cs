using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Diagnostics
{
	public class ConsoleInitializer : MonoBehaviour
	{
		private const string UNITY_LOG_CHANNEL = "Unity";
		
		private Console console;

		private void Awake()
		{
			var commands = GetCommands();
			
			console = new Console(new HeaderDrawer(), new LogsAreaDrawer(), new InputFieldDrawer(commands.Keys.ToArray()), commands);

			Application.logMessageReceived += (message, trace, type) =>
			{
				console.Log(new LogEntry()
				{
					message = message + Environment.NewLine + trace,
					logType = UnityToPadoruLogType(type),
					channel = UNITY_LOG_CHANNEL,
				});
			};
		}

		private void OnGUI()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.BackQuote)
			{
				console.ToggleConsole();
			}
			
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				console.ProcessInput();
			}
			
			console.Draw();
		}

		private void LateUpdate()
		{
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		private Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>> GetCommands()
		{
			var sb = new StringBuilder();
			sb.Append("Debug Commands Console initialized. Commands:");
			sb.Append(Environment.NewLine);

			var data = AttributeUtils.GetTypesWithAttributeInstanced<ConsoleCommand, ConsoleCommandAttribute>();
			var commands = new Dictionary<string, InstancedTypeData<ConsoleCommand, ConsoleCommandAttribute>>();

			foreach (var entry in data)
			{
				var commandName = entry.Attribute.CommandName.ToLower();
				commands.Add(commandName, entry);

				sb.Append($"  - {commandName}");
				sb.Append(Environment.NewLine);
			}
			
			Debug.Log(sb);

			return commands;
		}

		private Padoru.Diagnostics.LogType UnityToPadoruLogType(LogType logType)
		{
			if (logType == LogType.Log)
			{
				return Padoru.Diagnostics.LogType.Info;
			}
			if (logType == LogType.Warning)
			{
				return Padoru.Diagnostics.LogType.Warning;
			}
			if (logType == LogType.Error)
			{
				return Padoru.Diagnostics.LogType.Error;
			}
			if (logType == LogType.Exception)
			{
				return Padoru.Diagnostics.LogType.Exception;
			}

			return Padoru.Diagnostics.LogType.Info;
		}
	}
}
