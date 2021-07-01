using FontStashSharp;
using QuickNA.Utilities;
using System;
using System.IO;

namespace QuickNA.Assets.Loaders
{
	public class FontSystemLoader : ILoader
	{
		public string RequestedFileExtension => ".qfont";

		public void Load(string path)
		{
			using StreamReader reader = File.OpenText(path);
			TomlTable table = TOML.Parse(reader);

			if (table.TryGetNode("name", out TomlNode name) && !name.IsString)
				throw new QuickNAException("No name specified for the fontsystem: " + path);

			if (table.TryGetNode("fonts", out TomlNode fonts) && !fonts.IsArray)
				throw new QuickNAException("No fonts specified for the fontsystem: " + path);

			FontSystemSettings settings = ParseFontSystemSettings(table);
			FontStashSharp.FontSystem fontSystem = new FontStashSharp.FontSystem(settings);

			foreach (TomlNode font in fonts)
			{
				if (!font.IsString)
					throw new QuickNAException("Invalid font path: " + font.ToString());

				fontSystem.AddFont(File.ReadAllBytes(FindFontPath(font.ToString())));
			}

			Assets<FontSystem>.Add(name, new FontSystem(fontSystem));
		}

		private string FindFontPath(string fontPath)
		{
			string foundPath = null;
			string fontPathInContent = Path.Combine(AssetServer.RootDirectory, fontPath);

			if (!File.Exists(fontPathInContent))
			{
				fontPathInContent += ".ttf";
				if (!File.Exists(fontPathInContent))
				{
					bool success = false;

					foreach (string file in Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)))
					{
						if (Path.GetFileNameWithoutExtension(file).ToLower() == fontPath.ToLower() && Path.GetExtension(file).ToLower() == ".ttf")
						{
							foundPath = file;
							success = true;
							break;
						}
					}

					if (!success)
						throw new QuickNAException("Could not find font file: " + fontPath);
				}
				else
					foundPath = fontPathInContent;
			}
			else
				foundPath = fontPathInContent;

			return foundPath;
		}

		private FontSystemSettings ParseFontSystemSettings(TomlNode table)
		{
			FontSystemSettings settings = new FontSystemSettings();

			if (table.TryGetNode("effect", out TomlNode effect))
			{
				if (effect.TryGetNode("type", out TomlNode effectType) && !effectType.IsString)
					throw new QuickNAException("FontSystem effect must have a type");

				settings.Effect = effectType.ToString().ToLower() switch
				{
					"blurry" => FontSystemEffect.Blurry,
					"stroked" => FontSystemEffect.Stroked,
					_ => throw new QuickNAException("Unknown fontsystem effect: " + effectType)
				};

				if (effect.TryGetNode("strength", out TomlNode effectStrength) && !effectStrength.IsInteger)
					throw new QuickNAException("FontSystem effect must have a strength");

				settings.EffectAmount = effectStrength;
			}

			return settings;
		}
	}
}
