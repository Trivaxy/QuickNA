namespace QuickNA.Essentials
{
	/// <summary>
	/// Implementing this interface allows your class to be used as a behavior.
	/// </summary>
	public interface IBehavior
	{
		/// <summary>
		/// Called when the behavior runs.
		/// </summary>
		void Run();
	}
}