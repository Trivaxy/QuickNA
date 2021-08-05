using Microsoft.Xna.Framework;
using QuickNA.Assets;
using QuickNA.Rendering;

namespace QuickNA.ECS.Components
{
	public struct RenderText
	{
		public Handle<RenderLayer> RenderLayerHandle;
		public string Text;
		public Handle<FontSystem> FontSystemHandle;
		public int FontSize;
		public Color Color;
		public Vector2 Origin;

		public RenderText(Handle<RenderLayer> renderLayerHandle, string text, Handle<FontSystem> fontSystemHandle, int fontSize)
		{
			RenderLayerHandle = renderLayerHandle;
			Text = text;
			FontSystemHandle = fontSystemHandle;
			FontSize = fontSize;
			Color = Color.White;
			Origin = Vector2.Zero;
		}

		public RenderText(Handle<RenderLayer> renderLayerHandle, string text, Handle<FontSystem> fontSystemHandle, int fontSize, Color color)
		{
			RenderLayerHandle = renderLayerHandle;
			Text = text;
			FontSystemHandle = fontSystemHandle;
			FontSize = fontSize;
			Color = color;
			Origin = Vector2.Zero;
		}

		public RenderText(Handle<RenderLayer> renderLayerHandle, string text, Handle<FontSystem> fontSystemHandle, int fontSize, Color color, Vector2 origin)
		{
			RenderLayerHandle = renderLayerHandle;
			Text = text;
			FontSystemHandle = fontSystemHandle;
			FontSize = fontSize;
			Color = color;
			Origin = origin;
		}
	}
}
