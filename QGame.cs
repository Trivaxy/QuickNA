using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using QuickNA.Assets.Loaders;
using QuickNA.Essentials;
using QuickNA.Rendering;

namespace QuickNA
{
	public abstract class QGame : Game
	{
		private SpriteBatch spriteBatch;
		protected readonly GraphicsDeviceManager GraphicsDeviceManager;

		protected new GraphicsDevice GraphicsDevice => GraphicsDeviceManager.GraphicsDevice;

		protected QGame(int width, int height, bool fullscreen, bool vsync = true, string rootDirectory = "Content")
		{
			GraphicsDeviceManager = new GraphicsDeviceManager(this);

			GraphicsDeviceManager.PreferredBackBufferWidth = width;
			GraphicsDeviceManager.PreferredBackBufferHeight = height;
			GraphicsDeviceManager.IsFullScreen = fullscreen;
			GraphicsDeviceManager.SynchronizeWithVerticalRetrace = vsync;

			Content.RootDirectory = rootDirectory;
		}

		protected sealed override void Initialize()
		{
			World.RegisterBehavior(new InputBehavior());
			Setup();

			base.Initialize();
		}

		protected sealed override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			AssetServer.ContentManager = Content;

			AssetServer.RegisterLoader(new FNALoader<Texture2D>(".png"));
			AssetServer.RegisterLoader(new FNALoader<SoundEffect>(".wav"));

			Assets<Texture2D>.DefaultValue = new Texture2D(GraphicsDevice, 1, 1);
			Assets<SoundEffect>.DefaultValue = new SoundEffect(new byte[] { 0 }, 0, AudioChannels.Mono);

			RegisterLoaders();

			AssetServer.Start();

			base.LoadContent();
		}

		protected sealed override void Draw(GameTime gameTime) => RenderRegistry.RenderAll(spriteBatch);

		protected sealed override void Update(GameTime gameTime) => World.Update(gameTime);

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
