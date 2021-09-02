using System;

namespace QuickNA.ECS
{
	internal class MessageCollection
	{
		private object[] messages = new object[32];

		public void Send<T>(T value)
			where T : struct
		{
			int typeID = TypeID<T>.ID;

			if (typeID >= messages.Length)
				Array.Resize(ref messages, typeID * 2);

			messages[typeID] = value;
		}

		public bool CheckForMessage<T>(out T message)
			where T : struct
		{
			int typeID = TypeID<T>.ID;
			message = default;

			if (typeID >= messages.Length || messages[typeID] == null)
				return false;

			message = (T)messages[typeID];
			return true;
		}

		public void Clear() => Array.Fill(messages, null);
	}
}
