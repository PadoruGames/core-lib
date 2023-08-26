using Padoru.Diagnostics;

namespace Padoru.Core.Diagnostics
{
	public class LogEntry
	{
		public LogType logType;
		public object message;
		public string channel;
		public object context;
	}
}
