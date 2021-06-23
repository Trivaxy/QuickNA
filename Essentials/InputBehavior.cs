using Microsoft.Xna.Framework.Input;

namespace QuickNA.Essentials
{
	/// <summary>
	/// A behavior that is responsible for providing input-handling utilities.
	/// </summary>
	public class InputBehavior : IBehavior
	{
		internal static KeyboardState previousKeyboardState;

		/// <summary>
		/// Checks if the specified key is pressed.
		/// </summary>
		public static bool IsKeyPressed(Keys key) => Keyboard.GetState().IsKeyDown(key);

		/// <summary>
		/// Checks if the specified key has just been pressed.
		/// </summary>
		public static bool IsKeyJustPressed(Keys key) => Keyboard.GetState().IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);

		/// <summary>
		/// Checks if the specified key has just been released.
		/// </summary>
		public static bool IsKeyJustReleased(Keys key) => !Keyboard.GetState().IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);

		public void Update() => previousKeyboardState = Keyboard.GetState();
	}
}
