using FontStashSharp;
using Microsoft.Xna.Framework;

namespace QuickNA.Assets
{
	/// <summary>
	/// Wrapper type around FontStashSharp's FontSystem to avoid direct referencing.
	/// </summary>
	public class FontSystem
	{
		internal FontStashSharp.FontSystem fontSystem;

		internal FontSystem(FontStashSharp.FontSystem fontSystem)
		{
			this.fontSystem = fontSystem;
		}

		public Vector2 MeasureString(int size, string text)
		{
			Bounds bounds = new Bounds();
			fontSystem.GetFont(size).TextBounds(text, Vector2.Zero, ref bounds);

			return new Vector2(bounds.X2, bounds.Y2);
		}
	}
}
