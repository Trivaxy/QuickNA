using System.Collections.Generic;

namespace QuickNA.ECS
{
	/// <summary>
	/// Handles the registering of systems and running them. Dispatchers can be instantiated and associated with a <see cref="Playground"/>.
	/// </summary>
	public sealed class Dispatcher
	{
		private IList<GameSystem> systems = new List<GameSystem>();
		private IList<GameSystem> startupSystems = new List<GameSystem>();
		private Playground playground;
		private bool ranStartupSystems;

		public Dispatcher(Playground playground)
		{
			this.playground = playground;
		}

		/// <summary>
		/// Registers a system to be ran when the dispatcher runs.
		/// Your systems must be registered in the order you want them to run.
		/// </summary>
		/// <param name="system">The system to register.</param>
		public void Add(GameSystem system)
		{
			if (systems.Contains(system))
				throw new QuickNAException("Cannot register the same system more than once");

			system.Playground = playground;
			systems.Add(system);
		}

		/// <summary>
		/// Registers a system to be ran when the dispatcher runs for the first time.
		/// Your systems must be registered in the order you want them to run.
		/// </summary>
		/// <param name="system">The startup system to register.</param>
		public void AddStartup(GameSystem system)
		{
			if (startupSystems.Contains(system))
				throw new QuickNAException("Cannot register the same startup system more than once");

			system.Playground = playground;
			startupSystems.Add(system);
		}

		/// <summary>
		/// Runs the dispatcher which in turn runs all registered systems, as well as run startup systems if this is the first dispatch.
		/// </summary>
		public void Dispatch()
		{
			if (!ranStartupSystems)
			{
				foreach (GameSystem system in startupSystems)
					system.Run();

				ranStartupSystems = true;
			}

			foreach (GameSystem system in systems)
				system.Run();
		}
	}
}