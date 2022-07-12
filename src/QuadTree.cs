namespace QuadTreeMG;

internal class QuadTree
{
   private List<QuadPoint> points;
   private Rectangle boundary;
   private int capacity;
   private bool divided;

   private QuadTree? northWest;
   private QuadTree? northEast;
   private QuadTree? southWest;
   private QuadTree? southEast;

   /* Ctors */
   
   public QuadTree(Rectangle boundary, int capacity)
   {
      this.points = new List<QuadPoint>();
      this.boundary = boundary;
      this.capacity = capacity;
      this.divided = false;
   }

   // __Methods__

   public bool Insert(QuadPoint point)
   {
      if (!this.boundary.Contains(point)) return false;

      if (this.points.Count < capacity)
      {
         this.points.Add(point);
         return true;
      }

      return AddToQuads(point);
   }

   private bool AddToQuads(QuadPoint point)
   {
      if (!this.divided)
         Subdivide();

      if (this.northWest?.Insert(point) == true) return true;
      if (this.northEast?.Insert(point) == true) return true;
      if (this.northEast?.Insert(point) == true) return true;
      if (this.southWest?.Insert(point) == true) return true;
      if (this.southEast?.Insert(point) == true) return true;

      return false;
   }

   public void Subdivide()
   {
      int centerX = this.boundary.X + this.boundary.Width / 2;
      int centerY = this.boundary.Y + this.boundary.Height / 2;
      int halfedWidth = this.boundary.Width / 2;
      int halfedHeight = this.boundary.Height / 2;

      var nw = new Rectangle(this.boundary.X, this.boundary.Y, halfedWidth, halfedHeight);
      var sw = new Rectangle(this.boundary.X, centerY, halfedWidth, halfedHeight);
      var ne = new Rectangle(centerX, this.boundary.Y, halfedWidth, halfedHeight);
      var se = new Rectangle(centerX, centerY, halfedWidth, halfedHeight);

      this.divided = true;
      this.northWest = new QuadTree(nw, this.capacity);
      this.northEast = new QuadTree(ne, this.capacity);
      this.southWest = new QuadTree(sw, this.capacity);
      this.southEast = new QuadTree(se, this.capacity);
   }

   public List<Rectangle> GetBoundaries()
   {
      var output = new List<Rectangle>();

      output.Add(boundary);
      if (!this.divided) return output;

      output.AddRange(northWest?.GetBoundaries() ?? new List<Rectangle>());
      output.AddRange(northEast?.GetBoundaries() ?? new List<Rectangle>());
      output.AddRange(southWest?.GetBoundaries() ?? new List<Rectangle>());
      output.AddRange(southEast?.GetBoundaries() ?? new List<Rectangle>());

      return output;
   }

   public List<QuadPoint> GetAllPoints()
   {
      var output = new List<QuadPoint>();

      output.AddRange(this.points);
      if (!this.divided) return output;

      output.AddRange(northWest?.GetAllPoints() ?? new List<QuadPoint>());
      output.AddRange(northEast?.GetAllPoints() ?? new List<QuadPoint>());
      output.AddRange(southWest?.GetAllPoints() ?? new List<QuadPoint>());
      output.AddRange(southEast?.GetAllPoints() ?? new List<QuadPoint>());

      return output;
   }

   public List<QuadPoint> PointsInRange(Circle range)
   {
      var output = new List<QuadPoint>();

      if (!this.boundary.Collides(range)) return output;

      AppendPoints(output, range);

      if (!this.divided) return output;

      output.AddRange(northWest?.PointsInRange(range) ?? new List<QuadPoint>());
      output.AddRange(northEast?.PointsInRange(range) ?? new List<QuadPoint>());
      output.AddRange(southWest?.PointsInRange(range) ?? new List<QuadPoint>());
      output.AddRange(southEast?.PointsInRange(range) ?? new List<QuadPoint>());

      return output;
   }

   private void AppendPoints(List<QuadPoint> points, Circle range)
   {
      foreach (var item in this.points)
      {
         if (range.Contains(item))
         {
            points.Add(item);
         }
      }
   }
}