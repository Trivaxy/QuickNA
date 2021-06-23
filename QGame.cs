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
			RegisterBehaviors();
			RegisterRenderLayers();

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

		protected virtual void RegisterLoaders() { }

		protected virtual void RegisterBehaviors() { }

		protected virtual void RegisterRenderLayers() { }
	}
}
