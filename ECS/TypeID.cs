using System;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	internal struct TypeID<T>
	{
		public static int ID = -1;

		public override int GetHashCode() => ID;
	}

	internal static class TypeIDs
	{
		private static IDictionary<Type, int> typeToID = new Dictionary<Type, int>();
		private static IDictionary<int, Type> idToType = new Dictionary<int, Type>();
		private static int nextFreeTypeID;

		public static void Add(Type type, int id)
		{
			typeToID[type] = id;
			idToType[id] = type;
		}

		public static int GetTypeID(Type type) => typeToID[type];

		public static Type GetTypeFromID(int typeID) => idToType[typeID];

		public static int GetNextFreeTypeID() => nextFreeTypeID++;
	}
}
