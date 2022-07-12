public enum Key
{
   LShift = 160,
   RShift = 161,
   LCtrl = 162,
   RCtrl = 163,
   Left = 37,
   Up = 38,
   Right = 39,
   Down = 40,
   Space = 32,
   Enter = 13,
   Escape = 27,
   D0 = 48,
   D1 = 49,
   D2 = 50,
   D3 = 51,
   D4 = 52,
   D5 = 53,
   D6 = 54,
   D7 = 55,
   D8 = 56,
   D9 = 57,
   A = 65,
   B = 66,
   C = 67,
   D = 68,
   E = 69,
   F = 70,
   G = 71,
   H = 72,
   I = 73,
   J = 74,
   K = 75,
   L = 76,
   M = 77,
   N = 78,
   O = 79,
   P = 80,
   Q = 81,
   R = 82,
   S = 83,
   T = 84,
   U = 85,
   V = 86,
   W = 87,
   X = 88,
   Y = 89,
   Z = 90,
}

public enum MouseButton
{
   LeftButton,
   RightButton,
   ScrollWheel,
}

public static class Tags
{
   public const string Player = "Player";
   public const string Enemy = "Enemy";
   public const string Projectile = "Projectile";
}

public enum TextOrigin
{
   TopLeft,
   TopRight,
   BottomLeft,
   BottomRight,
   TopCenter,
   BottomCenter,
   CenterLeft,
   CenterRight,
   Center,
}