namespace QuickNA.ECS
{
	/// <summary>
	/// GameSystems are used in conjunction with a <see cref="Dispatcher"/> in order to run every frame.
	/// </summary>
	public abstract class GameSystem
	{	
		/// <summary>
		/// The <see cref="Playground"/> this system belongs to.
		/// </summary>
		public Playground Playground { get; internal set; }

		protected internal abstract void Run();
	}
}