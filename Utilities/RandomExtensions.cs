using System;

namespace QuickNA.Utilities
{
	public static class RandomExtensions
	{
		public static float NextFloat(this Random random, float minValue, float maxValue)
		{
			return (float)random.NextDouble() * (maxValue - minValue) + minValue;
		}
	}
}