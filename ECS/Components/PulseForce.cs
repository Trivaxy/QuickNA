using Microsoft.Xna.Framework;

namespace QuickNA.ECS.Components
{
	public struct PulseForce
	{
		public Vector2 Force;

		public PulseForce(Vector2 force)
		{
			Force = force;
		}

		public PulseForce(float x, float y)
		{
			Force = new Vector2(x, y);
		}
	}
}
