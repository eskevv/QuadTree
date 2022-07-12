namespace QuadTreeMG;

internal static class Primitives
{
   private static GraphicsDevice? _device;
   private static BasicEffect? _effect;

   private static short[] _indices;
   private static VertexPositionColor[] _vertices;

   private static int _maxIndeces;
   private static int _maxVertices;

   private static int _vertexCount;
   private static int _indexCount;
   private static int _shapeCount;

   static Primitives()
   {
      _maxVertices = 60000;
      _maxIndeces = _maxVertices * 3;

      _vertices = new VertexPositionColor[_maxVertices];
      _indices = new short[_maxIndeces];
   }

   public static void Initialize(CoreGame game)
   {
      _device = game.GraphicsDevice;
      _effect = new BasicEffect(game.GraphicsDevice);

      _effect.VertexColorEnabled = true;
      _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, _device.Viewport.Width, _device.Viewport.Height, 0f, -1, 1);
      _effect.World = Matrix.CreateTranslation(0.5f, 0.5f, 0f);
      _effect.View = Matrix.Identity;
   }

   public static void Flush()
   {
      if (_effect == null)
         throw new NullReferenceException("Initialize was never called.");

      if (_shapeCount == 0) return;

      foreach (var effectPass in _effect.CurrentTechnique.Passes)
      {
         effectPass.Apply();
         _device?.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertexCount, _indices, 0, _shapeCount);
      }

      _vertexCount = 0;
      _indexCount = 0;
      _shapeCount = 0;
   }

   private static void EnsureSpace(int vertexCount)
   {
      if (vertexCount > _maxVertices)
         throw new Exception($"Max vertex count per shape exceeded: {_maxVertices}.");
      if (_vertexCount + vertexCount > _maxVertices)
         Flush();
   }

   private static void AddShapes(VertexPositionColor[] vertices, short[] indices)
   { // automatically flushes if vertex limit per batch is reached 
      EnsureSpace(vertices.Length);

      foreach (var item in vertices)
      {
         _vertices[_vertexCount++] = item;
      }

      foreach (var item in indices)
      {
         _indices[_indexCount++] = item;
      }

      _shapeCount += indices.Length / 3;
   }

   // __Configurations__

   private static short[] GetIndeces(int count)
   {
      var indeces = new short[count];
      int start = _vertexCount;

      for (short i = 0; i < indeces.Length; i++)
      {
         int loop = i % 3;
         int offset = loop == 0 ? 0 : i / 3;
         indeces[i] = (short)(start + loop + offset);
      }

      return indeces;
   }

   private static VertexPositionColor[] GetPiVertices(Vector2 position, int sides, float radius, float rotation, Color color)
   {
      float pi_rota = MathHelper.TwoPi / sides;
      float cos = MathF.Cos(pi_rota);
      float sin = MathF.Sin(pi_rota);

      /* Rotation Formula =>
          x2 = cos_x1 - sin_y1
          y2 = sin_x1 + cos_y1 
      */

      float cos_i = MathF.Cos(rotation);
      float sin_i = MathF.Sin(rotation);
      float ax = cos_i * (radius) - sin_i * 0;
      float ay = sin_i * (radius) + cos_i * 0;

      var vertices = new VertexPositionColor[sides];
      for (int i = 0; i < vertices.Length; i++)
      {
         float x = (cos * ax - sin * ay);
         float y = (sin * ax + cos * ay);

         ax = x;
         ay = y;

         vertices[i] = new VertexPositionColor(new Vector3(x + position.X, y + position.Y, 0), color);
      }

      return vertices;
   }

   // __DrawMethods__

   public static void DrawRect(Vector2 position, int w, int h, Color color)
   {
      float left = position.X;
      float top = position.Y;
      float right = position.X + (w - 1);
      float bottom = position.Y + (h - 1);

      var vertices = new VertexPositionColor[4]
      {
         new VertexPositionColor(new Vector3(left, top, 0), color),
         new VertexPositionColor(new Vector3(right, top, 0), color),
         new VertexPositionColor(new Vector3(right, bottom, 0), color),
         new VertexPositionColor(new Vector3(left, bottom, 0), color),
      };

      short[] indices = GetIndeces(4);

      AddShapes(vertices, indices);
   }
   public static void DrawRect(int x, int y, int w, int h, Color color)
   {
      float left = x;
      float top = y;
      float right = x + (w - 1);
      float bottom = y + (h - 1);

      var vertices = new VertexPositionColor[4]
      {
         new VertexPositionColor(new Vector3(left, top, 0), color),
         new VertexPositionColor(new Vector3(right, top, 0), color),
         new VertexPositionColor(new Vector3(right, bottom, 0), color),
         new VertexPositionColor(new Vector3(left, bottom, 0), color),
      };

      short[] indices = GetIndeces(4);

      AddShapes(vertices, indices);
   }
   public static void DrawRectUser(int x, int y, int w, int h, Color color)
   {
      if (_effect == null || _device == null) return;

      float left = x;
      float top = y;
      float right = x + (w - 1);
      float bottom = y + (h - 1);

      var vertices = new VertexPositionColor[4]
      {
         new VertexPositionColor(new Vector3(left, top, 0), color),
         new VertexPositionColor(new Vector3(right, top, 0), color),
         new VertexPositionColor(new Vector3(right, bottom, 0), color),
         new VertexPositionColor(new Vector3(left, bottom, 0), color),
      };

      short[] indices = new [] {
         (short)0, 
         (short)1, 
         (short)2, 
         (short)3, 
         (short)0
      };

      // _effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
      
      foreach (var effectPass in _effect.CurrentTechnique.Passes)
      {
         effectPass.Apply();
         _device.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length, indices, 0, 4);
      }
   }

   public static void DrawFillRect(Vector2 position, int w, int h, Color color)
   {
      float left = position.X;
      float top = position.Y;
      float right = position.X + (w - 1);
      float bottom = position.Y + (h - 1);

      var vertices = new VertexPositionColor[4]
      {
         new VertexPositionColor(new Vector3(left, top, 0), color),
         new VertexPositionColor(new Vector3(right, top, 0), color),
         new VertexPositionColor(new Vector3(right, bottom, 0), color),
         new VertexPositionColor(new Vector3(left, bottom, 0), color),
      };

      short[] indices = GetIndeces(6);

      AddShapes(vertices, indices);
   }
   public static void DrawFillRect(int x, int y, int w, int h, Color color)
   {
      float left = x;
      float top = y;
      float right = x + (w - 1);
      float bottom = y + (h - 1);

      var vertices = new VertexPositionColor[4]
      {
         new VertexPositionColor(new Vector3(left, top, 0), color),
         new VertexPositionColor(new Vector3(right, top, 0), color),
         new VertexPositionColor(new Vector3(right, bottom, 0), color),
         new VertexPositionColor(new Vector3(left, bottom, 0), color),
      };

      short[] indices = GetIndeces(6);

      AddShapes(vertices, indices);
   }

   public static void DrawPolygon(Vector2 position, int sides, float radius, float rotation, Color color)
   {
      VertexPositionColor[] vertices = GetPiVertices(position, sides, radius, rotation, color);
      short[] indices = GetIndeces(vertices.Length);

      AddShapes(vertices, indices);
   }

   public static void DrawFillPolygon(Vector2 position, int sides, float radius, float rotation, Color color)
   {
      int faces = sides - 2;

      VertexPositionColor[] vertices = GetPiVertices(position, sides, radius, rotation, color);
      short[] indices = GetIndeces(faces * 3);

      AddShapes(vertices, indices);
   }

   public static void DrawCircle(Vector2 position, float radius, Color color)
   {
      DrawPolygon(position, 60, radius, 0, color);
   }

   public static void DrawFillCircle(Vector2 position, float radius, Color color)
   {
      DrawFillPolygon(position, 20, radius, 0, color);
   }
}
