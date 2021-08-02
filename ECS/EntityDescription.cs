using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace QuickNA.ECS
{
	public struct EntityDescription
	{
		private ulong data;

		public EntityDescription With<T>()
			where T : struct
		{
			AddComponent<T>();
			return this;
		}

		internal bool Empty => data == 0;

		internal void AddComponent<T>()
			where T : struct
			=> AddComponent(TypeID<T>.ID);

		internal void AddComponent(int id)
			=> data |= (ulong)1 << id;

		internal void RemoveComponent<T>()
			where T : struct
			=> RemoveComponent(TypeID<T>.ID);

		internal void RemoveComponent(int id)
			=> data &= ~((ulong)1 << id);

		internal bool HasComponent<T>()
			where T : struct
			=> HasComponent(TypeID<T>.ID);

		internal bool HasComponent(int id)
			=> (data & ((ulong)1 << id)) == (ulong)1 << id;

		internal bool SubsetOf(EntityDescription other) => (other.data & data) == data;

		public IEnumerable<int> GetComponents()
		{
			for (int i = BitOperations.LeadingZeroCount(data); i >= 0; i--)
				if (HasComponent(i))
					yield return i;
		}

		public override int GetHashCode() => data.GetHashCode();

		public override string ToString() => "[" + string.Join(", ", GetComponents().Select(id => TypeIDs.GetTypeFromID(id))) + "]";
	}
}
