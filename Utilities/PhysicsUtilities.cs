using Microsoft.Xna.Framework;
using QuickNA.ECS.Systems;

namespace QuickNA.Utilities
{
	public static class PhysicsUtilities
	{
		public static Vector2[] CreateBoxVertices(float width, float height)
		{
			width *= PhysicsSystem.PixelsPerMeterRatio;
			height *= PhysicsSystem.PixelsPerMeterRatio;

			Vector2[] vertices = new Vector2[4];
			vertices[0] = new Vector2(0, 0);
			vertices[1] = new Vector2(width, 0);
			vertices[2] = new Vector2(0, height);
			vertices[3] = new Vector2(width, height);

			return vertices;
		}
	}
}
