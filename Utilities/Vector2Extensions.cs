using Microsoft.Xna.Framework;
using System;

namespace QuickNA.Utilities
{
	public static class Vector2Extensions
	{
		private static Random random = new Random();

		public static Vector2 RotatedByRandom(this Vector2 vector, double maxRadians)
			=> vector.RotatedBy(random.NextDouble() * maxRadians - random.NextDouble() * maxRadians);

		public static Vector2 RotatedBy(this Vector2 vector, double radians, Vector2 center = default(Vector2))
		{
			float cos = (float)Math.Cos(radians);
			float sin = (float)Math.Sin(radians);

			Vector2 distance = vector - center;
			Vector2 result = center;
			result.X += distance.X * cos - distance.Y * sin;
			result.Y += distance.X * sin + distance.Y * cos;

			return result;
		}

		public static Point ToPoint(this Vector2 vector) => new Point((int)vector.X, (int)vector.Y);

		public static System.Numerics.Vector2 ToNumericsVector2(this Vector2 vector)
			=> new System.Numerics.Vector2(vector.X, vector.Y);

		public static Vector2 ToXNAVector2(this System.Numerics.Vector2 vector)
			=> new Vector2(vector.X, vector.Y);
	}
}
