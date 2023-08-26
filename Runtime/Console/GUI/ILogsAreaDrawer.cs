namespace Padoru.Core.Diagnostics
{
	public interface ILogsAreaDrawer : IDrawer
	{
		void AddLog(LogEntry log);
	}
}
