namespace QuickNA.Essentials
{
	/// <summary>
	/// A behavior that, on update, will query through every entity of the specified type in the world, allowing you to update them.
	/// </summary>
	/// <typeparam name="T">The type of entity to query.</typeparam>
	public class QueryBehavior<T> : IBehavior
	{
		public void Update()
		{
			foreach (T entity in World.QueryEntities<T>())
				UpdateEntity(entity);
		}

		/// <summary>
		/// Allows you to update the entity that is passed in as a parameter.
		/// This method will run for each entity on each update.
		/// </summary>
		/// <param name="entity">The entity you can update.</param>
		protected virtual void UpdateEntity(T entity) { }
	}

	/// <summary>
	/// A behavior that, on update, will query through every entity that inherits from a specified type and implements a specified interface, allowing you to update them.
	/// </summary>
	/// <typeparam name="T">The type of entity to query.</typeparam>
	/// <typeparam name="U">The interface the entity must implement.</typeparam>
	public class QueryBehavior<T, U> : IBehavior
	{
		public void Update()
		{
			foreach ((T entity, U flag) in World.QueryEntities<T, U>())
				UpdateEntity(entity, flag);
		}

		protected virtual void UpdateEntity(T entity, U flag) { }
	}
}