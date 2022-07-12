namespace QuadTreeMG;

internal class Circle
{
   public float X { get; set; }
   public float Y { get; set; }
   public float Radius { get; set; }

   public Circle(float x, float y, float radius)
   {
      X = x;
      Y = y;
      Radius = radius;
   }

   public bool Contains(QuadPoint p)
   {
      // checking if the euclidean distance of the point 
      // and the center of the circle is smaller or equal to the radius of the circle

      double d = Math.Pow((p.X - X), 2) + Math.Pow((p.Y - Y), 2);
      return d <= Radius * Radius;
   }

   public bool Contains(Circle c)
   {
      float distanceCheck = (Radius - c.Radius) * (Radius - c.Radius);

      double d = Math.Pow((c.X - X), 2) + Math.Pow((c.Y - Y), 2);
      return d <= distanceCheck;
   }
}
