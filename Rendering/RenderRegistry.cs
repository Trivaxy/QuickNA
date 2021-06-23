using Microsoft.Xna.Framework.Graphics;
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

		/// <summary>
		/// Registers a RenderLayer with a given name.
		/// Make sure you have a reference to your RenderLayer stored elsewhere, as the RenderRegistry will not expose them.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		public static void RegisterLayer(string name, RenderLayer layer)
		{
			CheckLayerRegistered(name);
			renderLayers.Add((name, layer));
		}

		/// <summary>
		/// Registers a RenderLayer with a given name after another RenderLayer with a specified name, causing it to draw over the target.
		/// Make sure you have a reference to your RenderLayer stored elsewhere, as the RenderRegistry will not expose them.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		/// <param name="targetLayerName">The target RenderLayer's name.</param>
		public static void RegisterLayerAfter(string name, RenderLayer layer, string targetLayerName)
		{
			CheckLayerRegistered(name);

			for (int i = 0; i < renderLayers.Count; i++)
			{
				(string layerName, _) = renderLayers[i];

				if (layerName == targetLayerName)
				{
					renderLayers.Insert(i + 1, (name, layer));
					return;
				}
			}

			throw new QuickNAException($"No layer with the name {name} has been registered");
		}

		/// <summary>
		/// Registers a RenderLayer with a given name behind another RenderLayer with a specified name, causing it to draw behind the target.
		/// Make sure you have a reference to your RenderLayer stored elsewhere, as the RenderRegistry will not expose them.
		/// </summary>
		/// <param name="name">The name of the RenderLayer.</param>
		/// <param name="layer">The RenderLayer.</param>
		/// <param name="targetLayerName">The target RenderLayer's name.</param>
		public static void RegisterLayerBehind(string name, RenderLayer layer, string targetLayerName)
		{
			CheckLayerRegistered(name);

			for (int i = 0; i < renderLayers.Count; i++)
			{
				(string layerName, _) = renderLayers[i];

				if (layerName == targetLayerName)
				{
					renderLayers.Insert(i, (name, layer));
					return;
				}
			}

			throw new QuickNAException($"No layer with the name {name} has been registered");
		}

		internal static void RenderAll(SpriteBatch spriteBatch)
		{
			foreach ((_, RenderLayer layer) in renderLayers)
				if (layer.Active)
					layer.Draw(spriteBatch);
		}

		private static void CheckLayerRegistered(string name)
		{
			foreach ((string layerName, _) in renderLayers)
				if (name == layerName)
					throw new QuickNAException($"A layer with the name {name} has already been registered");
		}
	}
}
