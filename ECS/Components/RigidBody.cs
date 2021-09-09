using Box2D.NetStandard.Dynamics.Bodies;
using System;
using System.Numerics;

namespace QuickNA.ECS.Components
{
	public struct RigidBody
	{
		internal float friction;
		internal float restitution;
		internal float density;
		internal bool fixedRotation;
		internal bool isStatic;
		internal Vector2[] vertices;
		internal Body body;

		public float Friction
		{
			get => friction;
			set
			{
				friction = value;
				body.GetFixtureList().m_friction = value;
			}
		}

		public float Restitution
		{
			get => restitution;
			set
			{
				restitution = value;
				body.GetFixtureList().Restitution = value;
			}
		}

		public float Density
		{
			get => density;
			set
			{
				density = value;
				body.GetFixtureList().Density = value;
			}
		}

		public bool FixedRotation
		{
			get => fixedRotation;
			set
			{
				fixedRotation = value;
				body.SetFixedRotation(value);
			}
		}

		public bool IsStatic => isStatic;

		public RigidBody(float friction, float restitution, float density, bool fixedRotation, bool isStatic, Span<Microsoft.Xna.Framework.Vector2> vertices)
		{
			this.friction = friction;
			this.restitution = restitution;
			this.density = density;
			this.fixedRotation = fixedRotation;
			this.isStatic = isStatic;

			this.vertices = new Vector2[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
				this.vertices[i] = new Vector2(vertices[i].X, vertices[i].Y);

			body = null;
		}
	}
}
