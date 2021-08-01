using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace QuickNA.ECS
{
	/// <summary>
	/// Used to build criterias that can be used with <see cref="Playground.Query(EntityCriteria)"/> to specify what components to look for in entities.
	/// When there's a few component types involved, you can skip this class and use any of the generic <see cref="Playground.Query{T}"/> overloads.
	/// </summary>
	public sealed class EntityCriteria
	{
		private List<int> componentIDs = new List<int>(); // TODO: could be a hashset but im lazy
		internal int targetGroupID;

		/// <summary>
		/// Adds the specified component to the criteria.
		/// </summary>
		/// <typeparam name="T">The component type to add to the criteria.</typeparam>
		public EntityCriteria With<T>()
			where T : struct
		{
			if (componentIDs.Contains(TypeID<T>.ID))
				return this;
				
			componentIDs.Add(TypeID<T>.ID);
			targetGroupID = TypeIDs.HashTypeIDs(CollectionsMarshal.AsSpan(componentIDs));
			return this;
		}
	}
}
