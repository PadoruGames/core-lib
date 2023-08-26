using System;

namespace Padoru.Core.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ConsoleCommandAttribute : Attribute
	{
		public string CommandName;
	}
}
