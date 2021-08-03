using System;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	internal struct TypeID<T>
	{
		private static int id = -1;

		public static int ID
		{
			get
			{
				if (id == -1)
					id = TypeIDs.Register<T>();
				return id;
			}
		}

		public override int GetHashCode() => ID;
	}

	internal static class TypeIDs
	{
		private static IDictionary<Type, int> typeToID = new Dictionary<Type, int>();
		private static IDictionary<int, Type> idToType = new Dictionary<int, Type>();
		private static int nextFreeTypeID;

		public static int GetTypeID(Type type) => typeToID[type];

		public static Type GetTypeFromID(int typeID) => idToType[typeID];

		public static int Register<T>()
		{
			int id = nextFreeTypeID++;
			typeToID[typeof(T)] = id;
			idToType[id] = typeof(T);
			return id;
		}
	}
}
