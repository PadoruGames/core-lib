namespace Padoru.Core.Diagnostics
{
	public interface IInputFieldDrawer : IDrawer
	{
		string Input { get; }
		
		void ClearInput();
	}
}
