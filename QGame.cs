using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using QuickNA.Assets.Loaders;
using QuickNA.ECS;
using QuickNA.Rendering;
using System.Reflection;

namespace QuickNA
{
	public abstract class QGame : Game
	{
		private SpriteBatch spriteBatch;
		protected readonly GraphicsDeviceManager graphicsDeviceManager;

		protected Playground Playground { get; init; }

		protected Dispatcher Dispatcher { get; init; }

		protected new GraphicsDevice GraphicsDevice => graphicsDeviceManager.GraphicsDevice;

		protected QGame(int width, int height, bool fullscreen, bool vsync = true, string rootDirectory = "Content")
		{
			graphicsDeviceManager = new GraphicsDeviceManager(this);

			graphicsDeviceManager.PreferredBackBufferWidth = width;
			graphicsDeviceManager.PreferredBackBufferHeight = height;
			graphicsDeviceManager.IsFullScreen = fullscreen;
			graphicsDeviceManager.SynchronizeWithVerticalRetrace = vsync;

			Content.RootDirectory = rootDirectory;

			Playground = new Playground();
			Dispatcher = new Dispatcher(Playground);
		}

		protected sealed override void Initialize()
		{
			Assembly.LoadFrom("StbTrueTypeSharp.dll");
			Assembly.LoadFrom("Box2D.NetStandard.dll");

			Setup();

			base.Initialize();
		}

		protected sealed override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			AssetServer.ContentManager = Content;
			AssetServer.GraphicsDevice = GraphicsDevice;

			AssetServer.RegisterLoader(new FNALoader<Texture2D>(".png"));
			AssetServer.RegisterLoader(new FNALoader<SoundEffect>(".wav"));
			AssetServer.RegisterLoader(new FontSystemLoader());

			RegisterLoaders();

			AssetServer.Start();

			base.LoadContent();
		}

		protected sealed override void Draw(GameTime gameTime) => RenderRegistry.RenderAll(Playground, spriteBatch);

		protected sealed override void Update(GameTime gameTime) => Dispatcher.Dispatch();

		/// <summary>
		/// Called when QuickNA is registering loaders. Register your loaders here.
		/// You can also manually add or remove assets as needed in this method as well as set the default value for assets.
		/// </summary>
		protected virtual void RegisterLoaders() { }

		/// <summary>
		/// Called when your game is initializing. Use this method to register systems as well as renderlayers.
		/// </summary>
		protected virtual void Setup() { }
	}
}
