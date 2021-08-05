namespace QuickNA.Assets
{
	public struct Handle<T>
	{
		private int slot;

		internal Handle(int slot)
		{
			this.slot = slot;
		}

		public T GetValue() => Assets<T>.Get(slot);

		public static implicit operator T(Handle<T> handle) => handle.GetValue();
	}
}
