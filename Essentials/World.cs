using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace QuickNA.Essentials
{
	public static class World
	{
		private static Entity[] entities = new Entity[500];
		private static int nextFreeSlot;
		private static IList<IBehavior> behaviors = new List<IBehavior>();

		/// <summary>
		/// The amount of active entities currently in the world.
		/// </summary>
		public static int AliveEntities
		{
			get;
			private set;
		}

		/// <summary>
		/// The current tick's GameTime.
		/// </summary>
		public static GameTime GameTime
		{
			get;
			private set;
		}

		public static void Update(GameTime gameTime)
		{
			GameTime = gameTime;

			foreach (IBehavior behavior in behaviors)
				behavior.Update();
		}

		/// <summary>
		/// Spawns an entity into the world.
		/// </summary>
		/// <param name="entity">The entity to spawn into the world.</param>
		public static void SpawnEntity(Entity entity)
		{
			if (AliveEntities == entities.Length)
				return;

			entities[nextFreeSlot] = entity;
			entity.ID = nextFreeSlot;
			AliveEntities++;

			if (nextFreeSlot + 1 < entities.Length && entities[nextFreeSlot] == null)
				nextFreeSlot++;
			else
				for (int i = 0; i < nextFreeSlot; i++)
					if (entities[i] == null)
						nextFreeSlot = i;
		}

		/// <summary>
		/// Kills an entity and removes it from the world.
		/// </summary>
		/// <param name="entity">The entity to kill.</param>
		public static void KillEntity(Entity entity)
		{
			entities[entity.ID] = null;
			AliveEntities--;
		}

		/// <summary>
		/// Registers a behavior in the world, allowing it to update every tick.
		/// </summary>
		/// <param name="behavior">The behavior to register.</param>
		public static void RegisterBehavior(IBehavior behavior)
		{
			foreach (IBehavior otherBehavior in behaviors)
				if (behavior.GetType() == otherBehavior.GetType())
					throw new QuickNAException("Cannot register a behavior more than once: " + behavior);

			behaviors.Add(behavior);
		}

		/// <summary>
		/// Kills all entities in the world.
		/// </summary>
		public static void KillAllEntities()
		{
			Array.Clear(entities, 0, entities.Length);
			AliveEntities = 0;
		}

		/// <summary>
		/// Yields all entities that are of the specified type or interface.
		/// </summary>
		/// <typeparam name="T">The type or interface to query.</typeparam>
		/// <returns>An iterator over all matching entities.</returns>
		public static IEnumerable<T> QueryEntities<T>()
		{
			foreach (Entity entity in entities)
				if (entity is T t)
					yield return t;
		}
	}
}