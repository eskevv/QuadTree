namespace QuadTreeMG;

internal class Particle
{
   public float X { get; set; }
   public float Y { get; set; }
   public float Radius { get; set; }
   public Color Color { get; set; }
   public Vector2 Direction { get; set; }
   public float Speed { get; set; }

   private Random _rand;

   public Particle(int x, int y, float radius)
   {
      _rand = new Random();

      X = x;
      Y = y;
      Radius = radius;
      Color = Color.Aquamarine;
      Speed = _rand.Next(1, 1);

      float xSpeed = _rand.Next(2) == 0 ? Speed : -Speed;
      float ySpeed = _rand.Next(2) == 0 ? Speed : -Speed;
      Direction = new Vector2((float)(xSpeed * (_rand.NextDouble() + 0.05f)), (float)(ySpeed * (_rand.NextDouble() + 0.05f)));
   }

   public bool IsColliding(Particle p)
   {
      float totalRadiusSquared = (p.Radius + Radius) * (p.Radius + Radius);

      float distX = p.X - X, distY = p.Y - Y;
      float distanceSq = (distX * distX) + (distY * distY);

      return (distanceSq < totalRadiusSquared);
   }

   public void Update()
   {
      X += Direction.X;
      Y += Direction.Y;

      if (X >= CoreGame.ScreenWidth - Radius || X <= 0 + Radius)
         Direction = new Vector2(Direction.X * -1, Direction.Y);
      if (Y >= CoreGame.ScreenHeight - Radius || Y <= 0 + Radius)
         Direction = new Vector2(Direction.X, Direction.Y * -1);
   }
}
