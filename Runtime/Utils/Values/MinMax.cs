using System;
using UnityEngine;

namespace Padoru.Core.Utils
{
	[Serializable]
	public struct MinMax : IEquatable<MinMax>
	{
		public float Min;
		public float Max;

		public bool Equals(MinMax other)
		{
			return Mathf.Approximately(Min, other.Min) && Mathf.Approximately(Max, other.Max);
		}

		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}
			
			return Equals((MinMax)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Min, Max);
		}

		public override string ToString()
		{
			return $"Min: {Min} | Max {Max}";
		}
	}
}