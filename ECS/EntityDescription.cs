using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace QuickNA.ECS
{
	internal struct EntityDescription
	{
		private ulong data;

		public bool Empty => data == 0;

		public void AddComponent<T>()
			where T : struct
			=> AddComponent(TypeID<T>.ID);

		public void AddComponent(int id)
			=> data |= (ulong)1 << id;

		public void RemoveComponent<T>()
			where T : struct
			=> RemoveComponent(TypeID<T>.ID);

		public void RemoveComponent(int id)
			=> data &= ~((ulong)1 << id);

		public bool HasComponent<T>()
			where T : struct
			=> HasComponent(TypeID<T>.ID);

		public bool HasComponent(int id)
			=> (data & ((ulong)1 << id)) == (ulong)1 << id;

		public bool SubsetOf(EntityDescription other) => (other.data & data) == data;

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
