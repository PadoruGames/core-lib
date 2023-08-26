using UnityEngine;

namespace Padoru.Core.Diagnostics
{
	public class HeaderDrawer : IHeaderDrawer
	{
		public void Draw()
		{
			var headerStyle = new GUIStyle(GUI.skin.box);
			
			GUILayout.Box("", headerStyle);
		}
	}
}
