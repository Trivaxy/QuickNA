using System;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	/// <summary>
	/// The main part of QuickNA's ECS. Playgrounds are responsible for storing entities and their components while providing access to them.
	/// In order to efficiently query and iterate entities that have certain components, look into <see cref="Dispatcher"/>.
	/// </summary>
	public class Playground
	{
		internal static Playground[] Playgrounds = new Playground[8];

		internal IDictionary<int, EntityGroup> entityGroups = new Dictionary<int, EntityGroup>();
		internal EntityDescription[] entityDescriptions = new EntityDescription[32];
		private IComponentCollection[] componentCollections = new IComponentCollection[4];
		private Stack<uint> reusableEntityIDs = new Stack<uint>(64);

		public uint ID { get; private set; }

		public uint EntityCount { get; private set; }

		public Playground()
		{
			for (uint i = 0; i < Playgrounds.Length; i++)
			{
				if (Playgrounds[i] == null)
				{
					ID = i;
					Playgrounds[i] = this;
					return;
				}
			}

			ID = (uint)Playgrounds.Length;
			Array.Resize(ref Playgrounds, Playgrounds.Length * 2);
			Playgrounds[ID] = this;
		}

		public Entity NewEntity()
		{
			uint entityID = GetFreeEntityID();
			entityDescriptions[entityID] = new EntityDescription();
			EntityCount++;

			return new Entity(entityID, ID);
		}

		public void DestroyEntity(uint entityID)
		{
			reusableEntityIDs.Push(entityID);

			foreach (int component in entityDescriptions[entityID].Components)
				RemoveComponentFromEntity(entityID, component);

			EntityCount--;
		}

		public IReadOnlySet<Entity> Query<T>()
			where T : struct
			=> Query(stackalloc int[] { TypeID<T>.ID });

		public IReadOnlySet<Entity> Query<T1, T2>()
			where T1 : struct
			where T2 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID });

		public IReadOnlySet<Entity> Query<T1, T2, T3>()
			where T1 : struct
			where T2 : struct
			where T3 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID, TypeID<T3>.ID });

		public IReadOnlySet<Entity> Query<T1, T2, T3, T4>()
			where T1 : struct
			where T2 : struct
			where T3 : struct
			where T4 : struct
			=> Query(stackalloc int[] { TypeID<T1>.ID, TypeID<T2>.ID, TypeID<T3>.ID, TypeID<T4>.ID });

		public IReadOnlySet<Entity> Query(EntityCriteria criteria) => GetGroupOrEmpty(criteria.targetGroupID);

		internal bool EntityHasComponent<T>(uint entityID)
			where T : struct
			=> entityDescriptions[entityID].HasComponent<T>();

		internal ref T GetEntityComponent<T>(uint entityID)
			where T : struct
			=> ref ((ComponentCollection<T>)componentCollections[TypeID<T>.ID]).Get(entityID);

		// should only be used for debugging
		internal object GetEntityComponent(uint entityID, int componentID)
			=> componentCollections[componentID].GetComponent(entityID);

		internal void AddComponentToEntity<T>(uint entityID, T component)
			where T : struct
		{
			if (TypeID<T>.ID == -1)
			{
				int typeID = TypeIDs.GetNextFreeTypeID();
				TypeID<T>.ID = typeID;
				TypeIDs.Add(typeof(T), typeID);
			}

			if (TypeID<T>.ID >= componentCollections.Length)
				Array.Resize(ref componentCollections, componentCollections.Length * 2);

			if (componentCollections[TypeID<T>.ID] == null)
				componentCollections[TypeID<T>.ID] = new ComponentCollection<T>();

			ComponentCollection<T> componentCollection = (ComponentCollection<T>)componentCollections[TypeID<T>.ID];

			if (EntityHasComponent<T>(entityID))
				componentCollection.Replace(entityID, component);
			else
			{
				componentCollection.Add(entityID, component);
				entityDescriptions[entityID].AddComponent<T>();

				int groupID = HashEntityComponentIDs(entityID);
				if (!entityGroups.ContainsKey(groupID))
					entityGroups[groupID] = new EntityGroup(entityDescriptions[entityID].Clone());

				int componentGroupID = TypeIDs.HashTypeID(TypeID<T>.ID);
				if (!entityGroups.ContainsKey(componentGroupID))
					entityGroups[componentGroupID] = new EntityGroup(new EntityDescription(TypeID<T>.ID));

				Entity entity = new Entity(entityID, ID);
				entityGroups[groupID].Add(entity);
				entityGroups[componentGroupID].Add(entity);
			}
		}

		internal void RemoveComponentFromEntity<T>(uint entityID)
			where T : struct
			=> RemoveComponentFromEntity(entityID, TypeID<T>.ID);

		internal void RemoveComponentFromEntity(uint entityID, int componentTypeID)
		{
			componentCollections[componentTypeID].RemoveComponent(entityID);

			foreach (EntityGroup group in entityGroups.Values)
				if (group.HasComponent(componentTypeID))
					group.Remove(new Entity(entityID, ID));
		}

		private IReadOnlySet<Entity> Query(Span<int> componentIDs) => GetGroupOrEmpty(TypeIDs.HashTypeIDs(componentIDs));

		private IReadOnlySet<Entity> GetGroupOrEmpty(int groupID)
		{
			if (entityGroups.TryGetValue(groupID, out EntityGroup group))
				return group.entities;

			return EntityGroup.Empty.entities;
		}

		internal int HashEntityComponentIDs(uint entityID)
		{
			EntityDescription entityDescription = entityDescriptions[entityID];
			Span<int> componentIDs = stackalloc int[entityDescription.Components.Count];

			int i = 0;
			foreach (byte componentID in entityDescription.Components)
			{
				componentIDs[i] = componentID;
				i++;
			}

			return TypeIDs.HashTypeIDs(componentIDs);
		}

		private uint GetFreeEntityID() => reusableEntityIDs.Count == 0 ? EntityCount : reusableEntityIDs.Pop();
	}
}
