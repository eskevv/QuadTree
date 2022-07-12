using Microsoft.Xna.Framework.Input;

public class Input
{
   private static KeyboardState _currentKeyboard;
   private static KeyboardState _prevKeyboard;
   private static MouseState _currentMouse;
   private static MouseState _previousMouse;

   private static Key _positiveRightKey = Key.D;
   private static Key _negativeRightKey = Key.A;
   private static Key _positiveUpKey = Key.S;
   private static Key _negativeUpKey = Key.W;

   public static Vector2 MousePosition => new Vector2(_currentMouse.X, _currentMouse.Y);
   public static float RawHorizontal => GetRawHorizontal();
   public static float RawVertical => GetRawVertical();

   // __Updates__

   public static void Update()
   {
      _prevKeyboard = _currentKeyboard;
      _currentKeyboard = Keyboard.GetState();
      _previousMouse = _currentMouse;
      _currentMouse = Mouse.GetState();
   }

   // __Keyboard__

   public static bool KeyReleased(Key key) => _currentKeyboard.IsKeyUp((Keys)key) && _prevKeyboard.IsKeyDown((Keys)key);

   public static bool KeyPressed(Key key) => _currentKeyboard.IsKeyDown((Keys)key) && _prevKeyboard.IsKeyUp((Keys)key);

   public static bool KeyHeld(Key key) => _currentKeyboard.IsKeyDown((Keys)key);

   public static bool KeyUp(Key key) => _currentKeyboard.IsKeyUp((Keys)key);


   // __Mouse__

   public static bool MouseReleased(MouseButton button) => GetPreviousButtonState(button) == ButtonState.Pressed && GetCurrentButtonState(button) == ButtonState.Released;

   public static bool MousePressed(MouseButton button) => GetPreviousButtonState(button) == ButtonState.Released && GetCurrentButtonState(button) == ButtonState.Pressed;

   public static bool MouseHeld(MouseButton button) => GetCurrentButtonState(button) == ButtonState.Pressed;

   public static bool MouseUp(MouseButton button) => GetCurrentButtonState(button) == ButtonState.Released;

   private static ButtonState GetCurrentButtonState(MouseButton button)
   {
      return button switch
      {
         MouseButton.LeftButton => _currentMouse.LeftButton,
         MouseButton.RightButton => _currentMouse.RightButton,
         MouseButton.ScrollWheel => _currentMouse.MiddleButton,
         _ => _currentMouse.LeftButton,
      };
   }

   private static ButtonState GetPreviousButtonState(MouseButton button)
   {
      return button switch
      {
         MouseButton.LeftButton => _previousMouse.LeftButton,
         MouseButton.RightButton => _previousMouse.RightButton,
         MouseButton.ScrollWheel => _previousMouse.MiddleButton,
         _ => _previousMouse.LeftButton,
      };
   }

   // __Shortcuts__

   private static float GetRawHorizontal()
   {
      float rawInput = 0;
      if (KeyHeld(_negativeRightKey))
      {
         rawInput -= 1;
      }
      if (KeyHeld(_positiveRightKey))
      {
         rawInput += 1;
      }

      return rawInput;
   }

   private static float GetRawVertical()
   {
      float rawInput = 0;
      if (KeyHeld(_negativeUpKey))
      {
         rawInput -= 1;
      }
      if (KeyHeld(_positiveUpKey))
      {
         rawInput += 1;
      }

      return rawInput;
   }
}