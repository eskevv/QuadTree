namespace QuadTreeMG;

internal class QuadPoint
{
   public int X { get; set; }
   public int Y { get; set; }
   public Particle Data { get; set; }

   public QuadPoint(int x, int y, Particle data)
   {
      X = x;
      Y = y;
      Data = data;
   }

   public override string ToString() => $"Point[{X}, {Y}]";
}
