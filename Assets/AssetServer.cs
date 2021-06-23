using Microsoft.Xna.Framework.Content;
using QuickNA.Assets.Loaders;
using System.Collections.Generic;
using System.IO;

namespace QuickNA.Assets
{
	/// <summary>
	/// Manages all registered loaders and provides realtime asset-replacing.
	/// </summary>
	public static class AssetServer
	{
		private static IDictionary<string, List<ILoader>> loaderCollections = new Dictionary<string, List<ILoader>>();
		private static FileSystemWatcher watcher;

		/// <summary>
		/// The root directory of the internal FNA ContentManager.
		/// </summary>
		public static string RootDirectory => ContentManager.RootDirectory;

		internal static ContentManager ContentManager { get; set; }

		/// <summary>
		/// Registers a loader.
		/// </summary>
		/// <param name="loader">The loader to register.</param>
		public static void RegisterLoader(ILoader loader)
		{
			if (!loaderCollections.ContainsKey(loader.RequestedFileExtension))
				loaderCollections[loader.RequestedFileExtension] = new List<ILoader>();

			loaderCollections[loader.RequestedFileExtension].Add(loader);
		}

		internal static void Start()
		{
			LoadAssetsInitially();

			watcher = new FileSystemWatcher(ContentManager.RootDirectory);
			watcher.IncludeSubdirectories = true;

			watcher.Created += OnFileAffected;
			watcher.Changed += OnFileAffected;
			watcher.Renamed += OnFileAffected;
		}

		private static void OnFileAffected(object sender, FileSystemEventArgs e) => LoadFile(e.FullPath);

		private static void LoadAssetsInitially()
		{
			foreach (string file in Directory.EnumerateFiles(ContentManager.RootDirectory))
				LoadFile(file);
		}

		private static void LoadFile(string file)
		{
			string extension = Path.GetExtension(file);

			if (loaderCollections.ContainsKey(extension))
				foreach (ILoader loader in loaderCollections[extension])
					loader.Load(file);
		}
	}
}