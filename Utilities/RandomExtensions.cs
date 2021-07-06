using Microsoft.Xna.Framework;
using System;

namespace QuickNA.Utilities
{
	public static class RandomExtensions
	{
		public static float NextFloat(this Random random) => (float)random.NextDouble();

		public static float NextFloat(this Random random, float minValue, float maxValue)
		{
			return random.NextFloat() * (maxValue - minValue) + minValue;
		}

		public static Vector2 NextVector2(this Random random, float minRotation = 0f, float maxRotation = (float)Math.PI * 2)
		{
			float angle = minRotation + maxRotation * random.NextFloat();
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}
	}
}