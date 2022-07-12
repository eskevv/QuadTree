namespace QuadTreeMG;

internal class Rectangle
{
   public int X { get; set; }
   public int Y { get; set; }
   public int Width { get; set; }
   public int Height { get; set; }

   public Rectangle(int x, int y, int width, int height)
   {
      X = x;
      Y = y;
      Width = width;
      Height = height;
   }

   public bool Contains(QuadPoint point)
   {
      int right = this.X + this.Width;
      int bottom = this.Y + this.Height;

      return point.X >= this.X && point.X <= right && point.Y >= this.Y && point.Y <= bottom;
   }

   public bool Collides(Rectangle rec)
   {
      int right = this.X + this.Width, rRight = rec.X + rec.Width;
      int bottom = this.Y + this.Height, rBottom = rec.Y + rec.Height;

      return this.X <= rRight && right >= rec.X && this.Y <= rBottom && bottom >= rec.Y;
   }

   public bool Collides(Circle cir)
   {
      float xDist = Math.Abs((X + Width / 2) - cir.X);
      float yDist = Math.Abs((Y + Height / 2) - cir.Y);

      int w = Width / 2;
      int h = Height / 2;

      double edges = Math.Pow((xDist - w), 2) + Math.Pow((yDist - h), 2);

      // no intersection
      if (xDist > (cir.Radius + w) || yDist > (cir.Radius + h))
         return false;

      // intersection within the circle
      if (xDist <= w || yDist <= h)
         return true;

      // intersection on the edge of the circle
      return edges <= (cir.Radius * cir.Radius);
   }

   public bool FitsRect(Rectangle rec)
   {
      int right = this.X + this.Width, rRight = rec.X + rec.Width;
      int bottom = this.Y + this.Height, rBottom = rec.Y + rec.Height;

      return rec.X >= this.X && rRight <= right && rec.Y >= this.Y && rBottom <= bottom;
   }

   public bool FitsCircle(Circle c)
   {
      float right = X + Width;
      float bottom = Y + Height;

      return c.X >= X + c.Radius && c.X <=  right - c.Radius && c.Y >= Y + c.Radius && c.Y <= bottom - c.Radius;
   }

   public override string ToString()
      => $"Rect[X:{X} Y:{Y} W:{Width} H:{Height}]";
}
