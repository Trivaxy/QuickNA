namespace QuickNA.Essentials
{
	/// <summary>
	/// Implementing this interface allows your class to update every tick.
	/// Make sure to register your implementing class in order for it to update.
	/// </summary>
	public interface IBehavior
	{
		/// <summary>
		/// Called every tick.
		/// </summary>
		void Update();
	}
}
