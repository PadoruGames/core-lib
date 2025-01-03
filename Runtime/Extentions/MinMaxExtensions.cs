using Padoru.Core.Utils;
using UnityEngine;

namespace Padoru.Core
{
	public static class MinMaxExtensions
	{
		public static int GetRandomIntValue(this MinMax minMax)
		{
			return Mathf.FloorToInt(Random.Range(minMax.Min, minMax.Max + 1));
		}
		
		public static float GetRandomValue(this MinMax minMax)
		{
			return Random.Range(minMax.Min, minMax.Max + 1);
		}
	}
}