namespace QuickNA.ECS
{
	internal static class ComponentEvents<T>
		where T : struct
	{
		public delegate void ComponentRemove(Entity entity, T component);

		public static event ComponentRemove OnComponentRemoved;

		internal static void InvokeRemove(Entity entity, T component) => OnComponentRemoved.Invoke(entity, component);
	}
}
