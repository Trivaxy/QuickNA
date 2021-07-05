using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using System;
using System.Collections.Generic;

namespace QuickNA.Rendering
{
	public sealed class RenderLayer
	{
		private interface IDrawable
		{
			public Vector2 Position { get; init; }

			public Color Color { get; init; }

			public float Rotation { get; init; }

			public Vector2 Origin { get; init; }

			public Vector2 Scale { get; init; }

			public void Draw(SpriteBatch spriteBatch);
		}

		private struct TextureDraw : IDrawable
		{
			public Texture2D Texture { get; init; }

			public Vector2 Position { get; init; }

			public Rectangle? SourceRectangle { get; init; }

			public Color Color { get; init; }

			public float Rotation { get; init; }

			public Vector2 Origin { get; init; }

			public Vector2 Scale { get; init; }

			public SpriteEffects SpriteEffects { get; init; }

			public TextureDraw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
			{
				Texture = texture;
				Position = position;
				SourceRectangle = sourceRectangle;
				Color = color;
				Rotation = rotation;
				Origin = origin;
				Scale = scale;
				SpriteEffects = spriteEffects;
			}

			public void Draw(SpriteBatch spriteBatch)
				=> spriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects, 0f);
		}

		private struct StringDraw : IDrawable
		{
			public FontStashSharp.FontSystem FontSystem { get; init; }

			public int FontSize { get; init; }

			public string Text { get; init; }

			public Vector2 Position { get; init; }

			public Color Color { get; init; }

			public float Rotation { get; init; }

			public Vector2 Origin { get; init; }

			public Vector2 Scale { get; init; }

			public StringDraw(FontStashSharp.FontSystem font, int fontSize, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
			{
				FontSystem = font;
				FontSize = fontSize;
				Text = text;
				Position = position;
				Color = color;
				Rotation = rotation;
				Origin = origin;
				Scale = scale;
			}

			public void Draw(SpriteBatch spriteBatch)
				=> FontSystem.GetFont(FontSize).DrawText(spriteBatch, Text, Position, Color, Scale, Rotation, Origin);
		}

		public readonly SpriteSortMode SpriteSortMode;
		public readonly BlendState BlendState;
		public readonly SamplerState SamplerState;
		public readonly DepthStencilState DepthStencilState;
		public readonly RasterizerState RasterizerState;
		public readonly Effect PostEffect;
		public readonly Effect InnerEffect;
		public readonly RenderTarget2D RenderTarget;
		public Matrix TransformationMatrix;
		public Color ClearColor;
		public bool Active;
		private IList<IDrawable> draws;
		internal Action<Effect> postEffectSetup;
		private Action<Effect> innerEffectSetup;

		public RenderLayer(
			SpriteSortMode spriteSortMode,
			BlendState blendState,
			SamplerState samplerState,
			DepthStencilState depthStencilState,
			RasterizerState rasterizerState,
			Effect postEffect,
			Effect innerEffect,
			Matrix transformMatrix,
			Color clearColor,
			int width,
			int height,
			GraphicsDevice graphicsDevice,
			Action<Effect> postEffectSetup = null,
			Action<Effect> innerEffectSetup = null
			)
		{
			SpriteSortMode = spriteSortMode;
			BlendState = blendState;
			SamplerState = samplerState;
			DepthStencilState = depthStencilState;
			RasterizerState = rasterizerState;
			PostEffect = postEffect;
			InnerEffect = innerEffect;
			TransformationMatrix = transformMatrix;
			ClearColor = clearColor;
			Active = true;
			draws = new List<IDrawable>();
			RenderTarget = new RenderTarget2D(graphicsDevice, width, height);
			this.postEffectSetup = postEffectSetup;
			this.innerEffectSetup = innerEffectSetup;
		}

		public void Render(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects)
			=> draws.Add(new TextureDraw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), spriteEffects));

		public void Render(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
			=> draws.Add(new TextureDraw(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects));

		public void RenderText(string text, FontSystem font, int size, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
			=> draws.Add(new StringDraw(font.fontSystem, size, text, position, color, rotation, origin, new Vector2(scale)));

		public void RenderText(string text, FontSystem font, int size, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
			=> draws.Add(new StringDraw(font.fontSystem, size, text, position, color, rotation, origin, scale));

		internal void RenderToTarget(SpriteBatch spriteBatch)
		{
			spriteBatch.GraphicsDevice.SetRenderTarget(RenderTarget);
			spriteBatch.GraphicsDevice.Clear(ClearColor);

			innerEffectSetup?.Invoke(InnerEffect);

			spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, InnerEffect);

			foreach (IDrawable drawable in draws)
				drawable.Draw(spriteBatch);

			spriteBatch.End();

			draws.Clear();
		}
	}
}
