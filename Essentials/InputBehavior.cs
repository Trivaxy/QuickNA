using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace QuickNA.Essentials
{
	/// <summary>
	/// A behavior that is responsible for providing input-handling utilities.
	/// </summary>
	public class InputBehavior : IBehavior
	{
		internal static KeyboardState previousKeyboardState;
		internal static MouseState previousMouseState;

		/// <summary>
		/// Checks if the specified key is pressed.
		/// </summary>
		public static bool IsKeyPressed(Keys key) => Keyboard.GetState().IsKeyDown(key);

		/// <summary>
		/// Checks if the specified key has just been pressed.
		/// </summary>
		public static bool IsKeyJustPressed(Keys key) => IsKeyPressed(key) && !previousKeyboardState.IsKeyDown(key);

		/// <summary>
		/// Checks if the specified key has just been released.
		/// </summary>
		public static bool IsKeyJustReleased(Keys key) => !IsKeyPressed(key) && previousKeyboardState.IsKeyDown(key);

		/// <summary>
		/// Checks if the left mouse button is currently pressed.
		/// </summary>
		public static bool IsMouseLeftPressed() => Mouse.GetState().LeftButton == ButtonState.Pressed;

		/// <summary>
		/// Checks if the left mouse button is not pressed.
		/// </summary>
		public static bool IsMouseLeftReleased() => Mouse.GetState().LeftButton == ButtonState.Released;

		/// <summary>
		/// Checks if the right mouse button is currently pressed.
		/// </summary>
		public static bool IsMouseRightPressed() => Mouse.GetState().RightButton == ButtonState.Pressed;

		/// <summary>
		/// Checks if the right mouse button is not pressed.
		/// </summary>
		public static bool IsMouseRightReleased() => Mouse.GetState().LeftButton == ButtonState.Released;

		/// <summary>
		/// Checks if the left mouse button was just clicked.
		/// </summary>
		public static bool IsMouseLeftJustClicked() => IsMouseLeftPressed() && previousMouseState.LeftButton == ButtonState.Released;

		/// <summary>
		/// Checks if the left mouse button was just released.
		/// </summary>
		public static bool IsMouseLeftJustReleased() => !IsMouseLeftPressed() && previousMouseState.LeftButton == ButtonState.Pressed;

		/// <summary>
		/// Checks if the left mouse button was just clicked.
		/// </summary>
		public static bool IsMouseRightJustClicked() => IsMouseRightPressed() && previousMouseState.RightButton == ButtonState.Released;

		/// <summary>
		/// Checks if the left mouse button was just released.
		/// </summary>
		public static bool IsMouseRightJustReleased() => !IsMouseRightPressed() && previousMouseState.RightButton == ButtonState.Pressed;

		/// <summary>
		/// Gets the position of the mouse relative to the game window.
		/// </summary>
		public static Point GetMousePosition()
		{
			MouseState mouse = Mouse.GetState();

			return new Point(mouse.X, mouse.Y);
		}

		public void Run()
		{
			previousKeyboardState = Keyboard.GetState();
			previousMouseState = Mouse.GetState();
		}
	}
}
