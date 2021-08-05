using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuickNA.Assets;
using QuickNA.ECS;
using QuickNA.ECS.Components;
using System.Collections.Generic;

namespace QuickNA.Rendering
{
	/// <summary>
	/// The class that handles storing all RenderLayers and drawing them automatically.
	/// You must register your RenderLayers in this class in order for them to function.
	/// </summary>
	public static class RenderRegistry
	{
		private static IList<(string, RenderLayer)> renderLayers = new List<(string, RenderLayer)>();
		private static EntityDescription renderEntityDescription = new EntityDescription().With<Transform>().With<Render>();
		private static EntityDescription renderTextDescription = new EntityDescription().With<Transform>().With<RenderText>();

		/// <summary>
		/// Registers a RenderLayer with a given name and returns a handle to it.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		public static Handle<RenderLayer> RegisterLayer(string name, RenderLayer layer)
		{
			CheckLayerRegistered(name);
			renderLayers.Add((name, layer));
			return Assets<RenderLayer>.Register(name, layer);
		}

		/// <summary>
		/// Registers a RenderLayer with a given name after another RenderLayer with a specified name, causing it to draw over the target. Returns a handle to the layer.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		/// <param name="targetLayerName">The target RenderLayer's name.</param>
		public static Handle<RenderLayer> RegisterLayerAfter(string name, RenderLayer layer, string targetLayerName)
		{
			CheckLayerRegistered(name);

			for (int i = 0; i < renderLayers.Count; i++)
			{
				(string layerName, _) = renderLayers[i];

				if (layerName == targetLayerName)
				{
					renderLayers.Insert(i + 1, (name, layer));
					return Assets<RenderLayer>.Register(name, layer);
				}
			}

			throw new QuickNAException($"No layer with the name {name} has been registered");
		}

		/// <summary>
		/// Registers a RenderLayer with a given name behind another RenderLayer with a specified name, causing it to draw behind the target. Returns a handle to the layer.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		/// <param name="targetLayerName">The target RenderLayer's name.</param>
		public static Handle<RenderLayer> RegisterLayerBehind(string name, RenderLayer layer, string targetLayerName)
		{
			CheckLayerRegistered(name);

			for (int i = 0; i < renderLayers.Count; i++)
			{
				(string layerName, _) = renderLayers[i];

				if (layerName == targetLayerName)
				{
					renderLayers.Insert(i, (name, layer));
					return Assets<RenderLayer>.Register(name, layer);
				}
			}

			throw new QuickNAException($"No layer with the name {name} has been registered");
		}

		internal static void RenderAll(Playground playground, SpriteBatch spriteBatch)
		{
			foreach (Entity entity in playground.Query(renderEntityDescription))
			{
				Transform transform = entity.Get<Transform>();
				Render render = entity.Get<Render>();

				render.RenderLayerHandle
					.GetValue()
					.Render(render.TextureHandle.GetValue(), transform.Position, render.SourceRectangle, render.Color, transform.Rotation, render.Origin, transform.Scale, render.SpriteEffects);
			}

			foreach (Entity entity in playground.Query(renderTextDescription))
			{
				Transform transform = entity.Get<Transform>();
				RenderText renderText = entity.Get<RenderText>();

				renderText.RenderLayerHandle
					.GetValue()
					.RenderText(renderText.Text, renderText.FontSystemHandle.GetValue(), renderText.FontSize, transform.Position, renderText.Color, transform.Rotation, renderText.Origin, transform.Scale);
			}

			foreach ((_, RenderLayer layer) in renderLayers)
				if (layer.Active)
					layer.RenderToTarget(spriteBatch);

			spriteBatch.GraphicsDevice.SetRenderTarget(null);

			foreach ((_, RenderLayer layer) in renderLayers)
			{
				if (!layer.Active)
					continue;

				layer.postEffectSetup?.Invoke(layer.PostEffect);

				spriteBatch.Begin(SpriteSortMode.Deferred, layer.BlendState, layer.SamplerState, layer.DepthStencilState, layer.RasterizerState, layer.PostEffect, layer.TransformationMatrix);
				spriteBatch.Draw(layer.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				spriteBatch.End();
			}
		}

		private static void CheckLayerRegistered(string name)
		{
			foreach ((string layerName, _) in renderLayers)
				if (name == layerName)
					throw new QuickNAException($"A layer with the name {name} has already been registered");
		}
	}
}
