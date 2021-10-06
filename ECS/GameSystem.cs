using System;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	/// <summary>
	/// GameSystems are used in conjunction with a <see cref="Dispatcher"/> in order to run every frame.
	/// </summary>
	public abstract class GameSystem
	{	
		/// <summary>
		/// The <see cref="Playground"/> this system belongs to.
		/// </summary>
		public Playground Playground { get; internal set; }

		protected internal abstract void Run();

		protected IReadOnlySet<Entity> Query<T>()
			where T : struct
			=> Query(stackalloc int[] { TypeID<T>.ID });

		protected IReadOnlySet<Entity> Query<T1, T2>()
			where T1 : struct
			where T2 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID });

		protected IReadOnlySet<Entity> Query<T1, T2, T3>()
			where T1 : struct
			where T2 : struct
			where T3 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID, TypeID<T3>.ID });

		protected IReadOnlySet<Entity> Query<T1, T2, T3, T4>()
			where T1 : struct
			where T2 : struct
			where T3 : struct
			where T4 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID, TypeID<T3>.ID, TypeID<T4>.ID });

		protected IReadOnlySet<Entity> Query(EntityDescription description) => Playground.Query(description);

		private IReadOnlySet<Entity> Query(Span<int> componentIDs)
		{
			EntityDescription description = new EntityDescription();

			for (int i = 0; i < componentIDs.Length; i++)
				description.AddComponent(componentIDs[i]);

			return Query(description);
		}

		protected void SendMessage<T>(T message)
			where T : struct
			=> Playground.SendMessage(message);

		protected bool IncomingMessages<T>()
			where T : struct
			=> Playground.IncomingMessages<T>();

		protected IEnumerable<T> ReadMessages<T>()
			where T : struct
			=> Playground.ReadMessages<T>();
	}
}