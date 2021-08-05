using Microsoft.Xna.Framework;

namespace QuickNA.ECS.Components
{
	public struct Transform
	{
		public Vector2 Position;
		public Vector2 Scale;
		public float Rotation;

		public Transform(Vector2 position)
		{
			Position = position;
			Scale = Vector2.One;
			Rotation = 0f;
		}

		public Transform(Vector2 position, Vector2 scale)
		{
			Position = position;
			Scale = scale;
			Rotation = 0f;
		}

		public Transform(Vector2 position, float rotation)
		{
			Position = position;
			Scale = Vector2.One;
			Rotation = rotation;
		}

		public Transform(Vector2 position, Vector2 scale, float rotation)
		{
			Position = position;
			Scale = scale;
			Rotation = rotation;
		}
	}
}
