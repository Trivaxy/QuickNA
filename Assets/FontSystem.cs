using FontStashSharp;

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
	}
}
