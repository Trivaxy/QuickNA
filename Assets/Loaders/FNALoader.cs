using System;
using System.IO;

namespace QuickNA.Assets.Loaders
{
	/// <summary>
	/// Loader that is designed to load content types in FNA.
	/// FNALoader instances for important content types are automatically registered when your game starts.
	/// </summary>
	public sealed class FNALoader<T> : ILoader // this will likely become an internal class in the future
	{
		private string requestedFileExtension;

		public string RequestedFileExtension => requestedFileExtension;

		public FNALoader(string requestedFileExtension)
		{
			this.requestedFileExtension = requestedFileExtension;
		}

		public void Load(string path)
		{
			T asset = LoadAsset(path.Substring(AssetServer.RootDirectory.Length).TrimStart(Path.DirectorySeparatorChar));
			string fileName = Path.GetFileNameWithoutExtension(path);

			// this check is needed in order to dispose assets that are being replaced, to avoid a memory leak
			if (Assets<T>.Has(fileName) && Assets<T>.Get(fileName) is IDisposable disposable)
			{
				Assets<T>.Add(fileName, asset);
				disposable.Dispose();
			}
			else
				Assets<T>.Add(fileName, asset);
		}

		private T LoadAsset(string contentPath) => AssetServer.ContentManager.Load<T>(contentPath);
	}
}