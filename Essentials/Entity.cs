namespace QuickNA.Essentials
{
	/// <summary>
	/// Represents an entity in the world.
	/// You must derive from Entity in order to create custom entities.
	/// Entities cannot update on their own, look into the QueryBehavior class.
	/// </summary>
	public abstract class Entity
	{
		public int ID
		{
			get;
			internal set;
		}

		/// <summary>
		/// Called when your entity is drawn.
		/// </summary>
		public virtual void Draw() { }
	}
}
