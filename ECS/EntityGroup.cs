using System.Collections;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	internal sealed class EntityGroup : IEnumerable
	{
		public static readonly EntityGroup Empty = new EntityGroup(new EntityDescription());

		public EntityDescription entityDescription;
		internal HashSet<Entity> entities;

		public EntityGroup(EntityDescription entityDescription)
		{
			this.entityDescription = entityDescription;
			entities = new HashSet<Entity>(64);
		}

		public void Add(Entity entity) => entities.Add(entity);

		public void Remove(Entity entity) => entities.Remove(entity);

		public bool HasComponent(int componentID) => entityDescription.Components.Contains((byte)componentID);

		public IEnumerator GetEnumerator() => entities.GetEnumerator();
	}
}
