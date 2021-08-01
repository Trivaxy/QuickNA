using System.Collections.Generic;

namespace QuickNA.ECS
{
	internal class EntityDescription
	{
		public HashSet<byte> Components = new HashSet<byte>(); // TODO: a bitmask?

		public EntityDescription(params int[] componentIDs)
		{
			foreach (int id in componentIDs)
				Components.Add((byte)id);
		}

		public bool HasComponent<T>()
			where T : struct
			=> Components.Contains((byte)TypeID<T>.ID);

		public void AddComponent<T>()
			where T : struct
			=> AddComponent(TypeID<T>.ID);

		public void AddComponent(int componentID)
			=> Components.Add((byte)componentID);

		public void RemoveComponent<T>()
			where T : struct
			=> Components.Remove((byte)TypeID<T>.ID);

		public EntityDescription Clone()
		{
			EntityDescription entityDescription = new EntityDescription();
			HashSet<byte> newSet = new HashSet<byte>();

			foreach (byte component in Components)
				newSet.Add(component);

			entityDescription.Components = newSet;

			return entityDescription;
		}
	}
}