using System;

namespace QuickNA.ECS
{
	internal interface IComponentCollection
	{
		object GetComponent(uint entityID);

		void RemoveComponent(uint entityID);
	}

	internal class ComponentCollection<T> : IComponentCollection
		where T : struct
	{
		public static uint DefaultSize = 32;

		private T[] components = new T[DefaultSize];
		private uint[] indicesPerEntity = new uint[DefaultSize];
		private uint[] freeSlotStack = new uint[DefaultSize / 2];
		private uint nextFreeSlot;

		public uint Count { get; private set; }

		public ref T Get(uint entityID)
			=> ref components[indicesPerEntity[entityID]];

		public void Add(uint entityID, T component)
		{
			if (entityID >= indicesPerEntity.Length)
				Array.Resize(ref indicesPerEntity, indicesPerEntity.Length * 2);

			uint slot = nextFreeSlot > 0 ? freeSlotStack[--nextFreeSlot] : Count;

			if (slot >= components.Length)
				Array.Resize(ref components, components.Length * 2);

			indicesPerEntity[entityID] = slot;
			components[slot] = component;
			Count++;
		}

		public void Replace(uint entityID, T component)
			=> components[indicesPerEntity[entityID]] = component;

		public void RemoveComponent(uint entityID)
		{
			if (nextFreeSlot + 1 >= freeSlotStack.Length)
				Array.Resize(ref freeSlotStack, freeSlotStack.Length * 2);

			freeSlotStack[nextFreeSlot++] = indicesPerEntity[entityID];
			Count--;
		}

		public object GetComponent(uint entityID) => Get(entityID);
	}
}