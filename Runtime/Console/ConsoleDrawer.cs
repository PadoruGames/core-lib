using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Diagnostics
{
	public class ConsoleDrawer : MonoBehaviour, IInitializable
	{
		private Console console;

		public void Init()
		{
			Debug.Log();
			
			console = Locator.Get<Console>();
		}

		private void OnGUI()
		{
			console?.Draw();
		}

		private void LateUpdate()
		{
			UnityEngine.Debug.developerConsoleVisible = false;
		}
	}
}
