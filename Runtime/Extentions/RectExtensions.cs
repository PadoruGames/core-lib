using UnityEngine;

namespace Padoru.Core
{
	public static class RectExtensions
	{
		public static Rect Add(this Rect a, Rect b)
		{
			return new Rect(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height);
		}
		
		public static Rect AddMargin(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2, rect.height - margin * 2);
		}
	}
}
