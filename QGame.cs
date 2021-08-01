using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using QuickNA.Assets.Loaders;
using QuickNA.Rendering;
using System.Reflection;

namespace QuickNA
{
	public abstract class QGame : Game
	{
		private SpriteBatch spriteBatch;
		protected readonly GraphicsDeviceManager graphicsDeviceManager;

		protected new GraphicsDevice GraphicsDevice => graphicsDeviceManager.GraphicsDevice;

		protected QGame(int width, int height, bool fullscreen, bool vsync = true, string rootDirectory = "Content")
		{
			graphicsDeviceManager = new GraphicsDeviceManager(this);

			graphicsDeviceManager.PreferredBackBufferWidth = width;
			graphicsDeviceManager.PreferredBackBufferHeight = height;
			graphicsDeviceManager.IsFullScreen = fullscreen;
			graphicsDeviceManager.SynchronizeWithVerticalRetrace = vsync;

			Content.RootDirectory = rootDirectory;
		}

		protected sealed override void Initialize()
		{
			Assembly.LoadFrom("StbTrueTypeSharp.dll"); // needed for FontStashSharp to function without adding a reference to this dll



			base.Initialize();
		}

		protected sealed override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			AssetServer.ContentManager = Content;

			AssetServer.RegisterLoader(new FNALoader<Texture2D>(".png"));
			AssetServer.RegisterLoader(new FNALoader<SoundEffect>(".wav"));
			AssetServer.RegisterLoader(new FontSystemLoader());

			Assets<Texture2D>.DefaultValue = new Texture2D(GraphicsDevice, 1, 1);
			Assets<SoundEffect>.DefaultValue = new SoundEffect(new byte[] { 0 }, 0, AudioChannels.Mono);

			RegisterLoaders();

			AssetServer.Start();

			base.LoadContent();
		}

		protected sealed override void Draw(GameTime gameTime) => RenderRegistry.RenderAll(spriteBatch);

		//protected sealed override void Update(GameTime gameTime) => World.Update(gameTime);

		/// <summary>
		/// Called when QuickNA is registering loaders. Register your loaders here.
		/// You can also manually add or remove assets as needed in this method as well as set the default value for assets.
		/// </summary>
		protected virtual void RegisterLoaders() { }

		/// <summary>
		/// Called when your game is initializing. Use this method to register behaviors and renderlayers.
		/// </summary>
		protected virtual void Setup() { }
	}
}
