namespace QuickNA.ECS
{
	/// <summary>
	/// Represents an entity in the world. You can add, remove and modify components in entities.
	/// </summary>
	public readonly struct Entity
	{
		/// <summary>
		/// The ID of the entity.
		/// </summary>
		public readonly uint ID;

		/// <summary>
		/// The playground the entity resides in.
		/// </summary>
		public readonly uint PlaygroundID;

		internal Entity(uint id, uint playgroundID)
		{
			ID = id;
			PlaygroundID = playgroundID;
		}

		/// <summary>
		/// Checks if the entity has the given component type.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		public bool Has<T>()
			where T : struct
			=> Playground.Playgrounds[PlaygroundID].EntityHasComponent<T>(ID);

		/// <summary>
		/// Adds the given component to the entity. If the entity already has a component of the given type, it is replaced.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		/// <param name="component">The component value.</param>
		public void Add<T>(T component)
			where T : struct
			=> Playground.Playgrounds[PlaygroundID].AddComponentToEntity(ID, component);

		/// <summary>
		/// Removes the component of the given type from the entity. If the entity does not have the component, nothing happens.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		public void Remove<T>()
			where T : struct
			=> Playground.Playgrounds[PlaygroundID].RemoveComponentFromEntity<T>(ID);

		/// <summary>
		/// Gets the component of the specified type in the entity.
		/// If the entity does not have the component, either an exception will occur or a default value is returned.
		/// </summary>
		/// <typeparam name="T">The component type.</typeparam>
		public ref T Get<T>()
			where T : struct
			=> ref Playground.Playgrounds[PlaygroundID].GetEntityComponent<T>(ID);

		/// <summary>
		/// Destroys the entity and frees all its components.
		/// </summary>
		public void Destroy() => Playground.Playgrounds[PlaygroundID].DestroyEntity(ID);

		public override int GetHashCode() => (int)ID;

		public override string ToString()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			Playground playground = Playground.Playgrounds[PlaygroundID];
			EntityDescription entityDescription = playground.entityDescriptions[ID];
			stringBuilder.Append($"Entity ({ID})");

			foreach (byte componentID in entityDescription.Components)
				stringBuilder.Append($" [{TypeIDs.GetTypeFromID(componentID).Name} {playground.GetEntityComponent(ID, componentID)}]");

			return stringBuilder.ToString();
		}
	}
}