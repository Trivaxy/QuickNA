using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace QuickNA.Rendering
{
	public sealed class RenderLayer
	{
		private struct DrawInfo
		{
			public readonly Texture2D Texture;
			public readonly Vector2 Position;
			public readonly Rectangle? SourceRectangle;
			public readonly Color Color;
			public readonly float Rotation;
			public readonly Vector2 Origin;
			public readonly Vector2 Scale;
			public readonly SpriteEffects SpriteEffects;
			
			public DrawInfo(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
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
		private IList<DrawInfo> draws;
		private GraphicsDevice graphicsDevice;
		private Action<Effect> postEffectSetup;
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
			draws = new List<DrawInfo>();
			this.graphicsDevice = graphicsDevice;
			RenderTarget = new RenderTarget2D(graphicsDevice, width, height);
			this.postEffectSetup = postEffectSetup;
			this.innerEffectSetup = innerEffectSetup;
		}

		public void Render(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects)
			=> draws.Add(new DrawInfo(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), spriteEffects));

		public void Render(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
			=> draws.Add(new DrawInfo(texture, position, sourceRectangle, color, rotation, origin, scale, spriteEffects));

		internal void RenderToTarget(SpriteBatch spriteBatch)
		{
			graphicsDevice.SetRenderTarget(RenderTarget);
			graphicsDevice.Clear(ClearColor);

			innerEffectSetup?.Invoke(InnerEffect);

			spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, InnerEffect, TransformationMatrix);

			foreach (DrawInfo draw in draws)
				spriteBatch.Draw(draw.Texture, draw.Position, draw.SourceRectangle, draw.Color, draw.Rotation, draw.Origin, draw.Scale, draw.SpriteEffects, 0f);

			spriteBatch.End();

			draws.Clear();
		}

		internal void Draw(SpriteBatch spriteBatch)
		{
			RenderToTarget(spriteBatch);

			graphicsDevice.SetRenderTarget(null);

			postEffectSetup?.Invoke(PostEffect);

			spriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, PostEffect, TransformationMatrix);
			spriteBatch.Draw(RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.End();
		}
	}
}
