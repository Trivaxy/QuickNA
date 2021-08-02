using System.Collections;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	internal sealed class EntityGroup : IEnumerable
	{
		public static readonly EntityGroup Empty = new EntityGroup(new EntityDescription());

		internal EntityDescription description;
		internal HashSet<Entity> entities;

		public EntityGroup(EntityDescription description)
		{
			this.description = description;
			entities = new HashSet<Entity>(64);
		}

		public void Add(Entity entity) => entities.Add(entity);

		public void Remove(Entity entity) => entities.Remove(entity);

		public bool AcceptsEntity(Entity entity) => description.SubsetOf(entity.Description);

		public IEnumerator GetEnumerator() => entities.GetEnumerator();

		public override string ToString() => "Count: " + entities.Count;
	}
}