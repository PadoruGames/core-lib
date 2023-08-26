using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Diagnostics
{
	[ConsoleCommand(CommandName = "log")]
	public class LogCommand : ConsoleCommand
	{
		public override void Execute(params string[] args)
		{
			if(args.Length <= 0)
			{
				return;
			}

			string log = string.Join(" ", args);

			Debug.Log(log);
		}
	}
}
