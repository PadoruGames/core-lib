using Padoru.Diagnostics;

namespace Padoru.Core.Diagnostics
{
	public class PadoruLoggerConsoleOutput : IDebugOutput
	{
		private readonly Console console;

		public PadoruLoggerConsoleOutput(Console console)
		{
			this.console = console;
		}
		
		public void WriteToOuput(LogType logType, object message, string channel, object context)
		{
			var logEntry = new LogEntry()
			{
				message = message,
				logType = logType,
				context = context,
				channel = channel
			};

			console.Log(logEntry);
		}
	}
}
