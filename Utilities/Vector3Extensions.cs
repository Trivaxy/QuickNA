using Microsoft.Xna.Framework;

namespace QuickNA.Utilities
{
	public static class Vector3Extensions
	{
		public static Vector2 Flatten(this Vector3 vector) => new Vector2(vector.X, vector.Y);
	}
}
