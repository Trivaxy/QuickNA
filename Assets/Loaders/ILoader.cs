namespace QuickNA.Assets.Loaders
{
	/// <summary>
	/// Implementing this interface allows your class to be used as a loader for content by the AssetServer.
	/// </summary>
	public interface ILoader
	{
		/// <summary>
		/// The file extension your loader listens to, including the "." at the start.
		/// </summary>
		string RequestedFileExtension { get; }

		/// <summary>
		/// Called whenever a file that matches the required extension needs to be loaded.
		/// </summary>
		/// <param name="path">The full path to the file.</param>
		void Load(string path);
	}
}
