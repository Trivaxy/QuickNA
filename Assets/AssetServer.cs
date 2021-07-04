using Microsoft.Xna.Framework.Content;
using QuickNA.Assets.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

		internal static Dictionary<string, object> ContentManagerCache { get; set; }

		internal static IList<IDisposable> ContentManagerDisposeCache { get; set; }

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

			// FNA caches things that are loaded with the ContentManager
			// Reloads mandate that I uncache them with reflection, so we obtain references to the internal caches
			ContentManagerCache = typeof(ContentManager)
				.GetField("loadedAssets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ContentManager)
				as Dictionary<string, object>;

			ContentManagerDisposeCache = typeof(ContentManager)
				.GetField("disposableAssets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ContentManager)
				as List<IDisposable>;

			watcher = new FileSystemWatcher(ContentManager.RootDirectory);
			watcher.IncludeSubdirectories = true;

			watcher.Created += OnFileAffected;
			watcher.Changed += OnFileAffected;
			watcher.Renamed += OnFileAffected;

			watcher.EnableRaisingEvents = true;
		}

		private static void OnFileAffected(object sender, FileSystemEventArgs e)
		{
			string assetKey = e.FullPath.Substring(RootDirectory.Length).TrimStart(Path.DirectorySeparatorChar);
			if (ContentManagerCache.TryGetValue(assetKey, out object value))
			{
				ContentManagerCache.Remove(assetKey);

				if (value is IDisposable disposable)
					ContentManagerDisposeCache.Remove(disposable); // We need to dispose it ourselves otherwise FNA disposes again after us
			}

			LoadFile(e.FullPath);
		}

		private static void LoadAssetsInitially()
		{
			foreach (string file in Directory.EnumerateFiles(ContentManager.RootDirectory, "*.*", SearchOption.AllDirectories))
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