using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using QuickNA.Rendering;

namespace QuickNA.ECS.Components
{
	public struct Render
	{
		public Handle<RenderLayer> RenderLayerHandle;
		public Handle<Texture2D> TextureHandle;
		public Color Color;
		public Rectangle? SourceRectangle;
		public SpriteEffects SpriteEffects;
		public Vector2 Origin;

		public Render(Handle<RenderLayer> renderLayerHandle, Handle<Texture2D> textureHandle)
		{
			RenderLayerHandle = renderLayerHandle;
			TextureHandle = textureHandle;
			Color = Color.White;
			SourceRectangle = null;
			SpriteEffects = SpriteEffects.None;
			Origin = Vector2.Zero;
		}

		public Render(Handle<RenderLayer> renderLayerHandle, Handle<Texture2D> textureHandle, Color color)
		{
			RenderLayerHandle = renderLayerHandle;
			TextureHandle = textureHandle;
			Color = color;
			SourceRectangle = null;
			SpriteEffects = SpriteEffects.None;
			Origin = Vector2.Zero;
		}

		public Render(Handle<RenderLayer> renderLayerHandle, Handle<Texture2D> textureHandle, Color color, Rectangle sourceRectangle, SpriteEffects spriteEffects, Vector2 origin)
		{
			RenderLayerHandle = renderLayerHandle;
			TextureHandle = textureHandle;
			Color = color;
			SourceRectangle = sourceRectangle;
			SpriteEffects = spriteEffects;
			Origin = origin;
		}
	}
}
