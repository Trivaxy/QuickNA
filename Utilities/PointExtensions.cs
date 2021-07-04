using Microsoft.Xna.Framework;

namespace QuickNA.Utilities
{
	public static class PointExtensions
	{
		public static Vector2 ToVector2(this Point point) => new Vector2(point.X, point.Y);
	}
}