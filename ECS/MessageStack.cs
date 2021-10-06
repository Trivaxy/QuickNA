using System;
using System.Collections.Generic;

namespace QuickNA.ECS
{
	public interface IMessageStack
	{
		public bool IncomingMessages { get; }

		public void Send(object message);

		public void Clear();
	}

	public class MessageStack<T> : IMessageStack
		where T : struct
	{
		private T[] messages = new T[32];
		private int currentIndex = -1;

		public bool IncomingMessages => currentIndex > -1;

		public void Send(T message)
		{
			if (++currentIndex >= messages.Length)
				Array.Resize(ref messages, messages.Length * 2);

			messages[currentIndex] = message;
		}

		public void Send(object message) => Send((T)message);

		public IEnumerable<T> Read()
		{
			for (int i = currentIndex; i >= 0; i--)
				yield return messages[i];
		}

		public void Clear()
		{
			Array.Clear(messages, 0, messages.Length);
			currentIndex = -1;
		}
	}
}
