using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using Microsoft.Xna.Framework;
using QuickNA.ECS.Components;
using QuickNA.Utilities;

namespace QuickNA.ECS.Systems
{
	public class PhysicsSystem : GameSystem
	{
		public static readonly float PixelsPerMeterRatio = 0.025f;

		private World world;
		private int velocityIterations;
		private int positionIterations;

		public PhysicsSystem(Vector2 gravity, int velocityIterations = 6, int positionIterations = 2)
		{
			this.world = new World(gravity.ToNumericsVector2() * PixelsPerMeterRatio);
			this.velocityIterations = velocityIterations;
			this.positionIterations = positionIterations;
		}

		static PhysicsSystem()
		{
			ComponentEvents<RigidBody>.OnComponentRemoved += ReleaseEntityBody;
		}

		protected internal override void Run()
		{
			world.Step(1f / 60f, velocityIterations, positionIterations);

			foreach (Entity entity in Query<RigidBody, Transform>())
			{
				ref RigidBody rigidBody = ref entity.Get<RigidBody>();

				if (rigidBody.body == null)
					ProcessEntityBody(entity);

				if (entity.Has<PulseForce>())
				{
					rigidBody.body.ApplyForceToCenter(entity.Get<PulseForce>().Force.ToNumericsVector2());
					entity.Remove<PulseForce>();
				}

				ref Transform transform = ref entity.Get<Transform>();
				transform.Position = rigidBody.body.Position.ToXNAVector2() / PixelsPerMeterRatio;
				transform.Rotation = rigidBody.body.GetAngle();
			}

			//foreach (Entity entity in Query<RigidBody, Velocity>())
			//{
			//	ref RigidBody rigidBody = ref entity.Get<RigidBody>();
			//	ref Velocity velocity = ref entity.Get<Velocity>();

			//	velocity.Value = rigidBody.body.GetLinearVelocity().ToXNAVector2();
			//}	
		}

		private static void ReleaseEntityBody(Entity entity, RigidBody component)
		{
			Body body = component.body;
			body.GetWorld().DestroyBody(body);
		}

		private void ProcessEntityBody(Entity entity)
		{
			Transform transform = entity.Get<Transform>();
			ref RigidBody entityBodyData = ref entity.Get<RigidBody>();

			BodyDef bodyDefinition = new BodyDef();
			bodyDefinition.type = entityBodyData.isStatic ? BodyType.Static : BodyType.Dynamic;
			bodyDefinition.position = transform.Position.ToNumericsVector2() * PixelsPerMeterRatio;
			bodyDefinition.angle = transform.Rotation;
			bodyDefinition.allowSleep = true;
			bodyDefinition.awake = true;
			bodyDefinition.fixedRotation = entityBodyData.fixedRotation;
			bodyDefinition.bullet = false;

			entityBodyData.body = world.CreateBody(bodyDefinition);

			PolygonShape shape = new PolygonShape();
			shape.Set(entityBodyData.vertices);

			FixtureDef fixtureDefinition = new FixtureDef();
			fixtureDefinition.friction = entityBodyData.friction;
			fixtureDefinition.restitution = entityBodyData.restitution;
			fixtureDefinition.density = entityBodyData.density;
			fixtureDefinition.shape = shape;

			entityBodyData.body.CreateFixture(fixtureDefinition);
		}
	}
}
