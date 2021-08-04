namespace QuickNA.Assets
{
	public struct Handle<T>
	{
		private int slot;

		internal Handle(int slot)
		{
			this.slot = slot;
		}

		public static implicit operator T(Handle<T> handle) => Assets<T>.Get(handle.slot);
	}
}
