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

		internal IDictionary<EntityDescription, EntityGroup> entityGroups = new Dictionary<EntityDescription, EntityGroup>();
		internal EntityDescription[] entityDescriptions = new EntityDescription[32];
		private IComponentCollection[] componentCollections = new IComponentCollection[4];
		private MessageCollection messages = new MessageCollection();
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

			if (entityID >= entityDescriptions.Length)
				Array.Resize(ref entityDescriptions, entityDescriptions.Length * 2);

			entityDescriptions[entityID] = new EntityDescription();
			EntityCount++;

			return new Entity(entityID, ID);
		}

		internal void DestroyEntity(uint entityID)
		{
			reusableEntityIDs.Push(entityID);

			foreach (int component in entityDescriptions[entityID].GetComponents())
				RemoveComponentFromEntity(entityID, component);

			EntityCount--;
		}

		internal bool EntityHasComponent<T>(uint entityID)
			where T : struct
			=> EntityHasComponent(entityID, TypeID<T>.ID);

		internal bool EntityHasComponent(uint entityID, int componentID)
			=> entityDescriptions[entityID].HasComponent(componentID);

		internal ref T GetEntityComponent<T>(uint entityID)
			where T : struct
			=> ref ((ComponentCollection<T>)componentCollections[TypeID<T>.ID]).Get(entityID);

		// should only be used for debugging
		internal object GetEntityComponent(uint entityID, int componentID)
			=> componentCollections[componentID].GetComponent(entityID);

		internal void AddComponentToEntity<T>(uint entityID, T component)
			where T : struct
		{
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

				EntityDescription entityDescription = entityDescriptions[entityID];
				if (!entityGroups.ContainsKey(entityDescription))
				{
					entityGroups[entityDescription] = new EntityGroup(entityDescription);

					foreach (EntityGroup group in entityGroups.Values)
						foreach (Entity otherEntity in group)
							if (entityGroups[entityDescription].AcceptsEntity(otherEntity))
								entityGroups[entityDescription].Add(otherEntity);
				}

				Entity entity = new Entity(entityID, ID);
				foreach (EntityGroup group in entityGroups.Values)
					if (group.AcceptsEntity(entity))
						group.Add(entity);
			}
		}

		internal void RemoveComponentFromEntity<T>(uint entityID)
			where T : struct
		{
			int componentID = TypeID<T>.ID;
			if (!EntityHasComponent(entityID, componentID))
				return;

			ComponentEvents<T>.InvokeRemove(new Entity(entityID, ID), GetEntityComponent<T>(entityID));
			RemoveComponentFromEntity(entityID, componentID);
		}

		internal void RemoveComponentFromEntity(uint entityID, int componentTypeID)
		{
			componentCollections[componentTypeID].RemoveComponent(entityID);

			ref EntityDescription description = ref entityDescriptions[entityID];
			description.RemoveComponent(componentTypeID);

			if (!entityGroups.ContainsKey(description))
				entityGroups[description] = new EntityGroup(description);

			Entity entity = new Entity(entityID, ID);
			entityGroups[description].Add(entity);

			foreach (EntityGroup group in entityGroups.Values)
				if (group.AcceptsEntity(entity))
					group.Add(entity);
				else
					group.Remove(entity);
		}

		internal IReadOnlySet<Entity> Query(EntityDescription description)
		{
			if (entityGroups.TryGetValue(description, out EntityGroup group))
				return group.entities;

			EntityGroup newGroup = new EntityGroup(description);

			foreach (EntityGroup matchedGroup in entityGroups.Values)
				if (description.SubsetOf(matchedGroup.description))
					foreach (Entity entity in matchedGroup)
						newGroup.Add(entity);

			entityGroups[description] = newGroup;

			return newGroup.entities;
		}

		internal void SendMessage<T>(T value)
			where T : struct
			=> messages.Send(value);

		internal bool CheckForMessage<T>(out T message)
			where T : struct
			=> messages.CheckForMessage(out message);

		internal void ClearMessages()
			=> messages.Clear();

		private uint GetFreeEntityID() => reusableEntityIDs.Count == 0 ? EntityCount : reusableEntityIDs.Pop();
	}
}
