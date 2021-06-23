using System.Collections.Generic;

namespace QuickNA.Assets
{
	public static class Assets<T>
	{
		private static IDictionary<string, T> assets = new Dictionary<string, T>();

		public static T DefaultValue
		{
			get;
			set;
		}

		public static int Count => assets.Count;

		public static T Get(string name)
		{
			if (assets.TryGetValue(name, out T value))
				return value;
			return DefaultValue;
		}

		public static void Add(string name, T value) => assets[name] = value;

		public static bool Has(string name) => assets.ContainsKey(name);
	}
}
