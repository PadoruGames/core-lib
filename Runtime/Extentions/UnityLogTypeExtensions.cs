using UnityEngine;

namespace Padoru.Core
{
	public static class UnityLogTypeExtensions
	{
		public static Padoru.Diagnostics.LogType ToPadoruLogType(this LogType logType)
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

			// If it is an assert, we'll just assume it's a normal log
			return Padoru.Diagnostics.LogType.Info;
		}
	}
}
