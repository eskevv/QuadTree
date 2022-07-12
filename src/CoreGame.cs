global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using System.Diagnostics;

namespace QuadTreeMG;

internal class CoreGame : Game
{
   private GraphicsDeviceManager _graphics;
   private SpriteBatch? _spriteBatch;

   private Stopwatch _watch;
   private Random _rand;
   private Color _clearColr;
   private float _deltaTime;
   private Dictionary<string, SpriteFont> _fonts;

   public static int ScreenWidth => 1600;
   public static int ScreenHeight => 900;
   public static bool IsFullScreen => false;
   public static int TargetFPS => 120;
   public static bool IsVSync => false;

   // --
   private List<Particle> _objects;
   private QuadTree _qTree;
   private List<Rectangle> _blocks;
   private int _collisions;
   private int _checks;
   private float _usualChecks;
   //

   public CoreGame()
   {
      _graphics = new GraphicsDeviceManager(this);

      _watch = new Stopwatch();
      _rand = new Random();
      _clearColr = new Color(240, 240, 240);
      _fonts = new Dictionary<string, SpriteFont>();

      _objects = new List<Particle>();
      _qTree = new QuadTree(new Rectangle(0, 0, 1600, 900), 2);
      _blocks = new List<Rectangle>();
   }

   protected override void Initialize()
   {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _graphics.PreferredBackBufferWidth = ScreenWidth;
      _graphics.PreferredBackBufferHeight = ScreenHeight;
      _graphics.SynchronizeWithVerticalRetrace = IsVSync;
      _graphics.IsFullScreen = IsFullScreen;
      _graphics.ApplyChanges();

      Content.RootDirectory = "content";
      IsMouseVisible = true;
      TargetElapsedTime = System.TimeSpan.FromSeconds(1d / TargetFPS);

      Primitives.Initialize(this);

      base.Initialize();
   }

   protected override void LoadContent()
   {
      _fonts["ui"] = Content.Load<SpriteFont>("ui-font");

      for (int x = 0; x < 1000; x++)
      {
         var p = new Particle(_rand.Next(20, 1580), _rand.Next(20, 880), 3f);
         _objects.Add(p);
      }
      _usualChecks = _objects.Count * _objects.Count;
   }

   protected override void Update(GameTime gameTime)
   {
      if (Keyboard.GetState().IsKeyDown(Keys.Escape))
         Exit();

      _qTree = new QuadTree(new Rectangle(0, 0, 1600, 900), 3);
      foreach (var item in _objects)
      {
         item.Update();
         item.Color = Color.Turquoise;
         _qTree.Insert(new QuadPoint((int)item.X, (int)item.Y, item));
      }

      _blocks = _qTree.GetBoundaries();

      _collisions = 0;
      _checks = 0;
      foreach (var item in _objects)
      {
         var range = new Circle(item.X, item.Y, item.Radius * 2);
         var checks = _qTree.PointsInRange(range);
         _checks += checks.Count;

         foreach (var point in checks)
         {
            Particle p = point.Data;
            if (item != p && item.IsColliding(p))
            {
               _collisions++;
               item.Color = Color.Crimson;
            }
         }
      }

      base.Update(gameTime);
   }

   protected override void Draw(GameTime gameTime)
   {
      GraphicsDevice.Clear(_clearColr);

      foreach (var item in _blocks)
      {
         Primitives.DrawRectUser(item.X, item.Y, item.Width, item.Height, Color.BlueViolet);
      }
      foreach (var item in _objects)
      {
         Primitives.DrawFillCircle(new Vector2(item.X, item.Y), item.Radius, item.Color);
      }
      
      Primitives.DrawFillRect(Vector2.Zero, ScreenWidth, 40, new Color(50, 20, 20, 220));
      Primitives.Flush();
      
      _spriteBatch?.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      _spriteBatch?.DrawString(_fonts["ui"], $"{(int)(1d / _deltaTime)} FPS", new Vector2(10f, 10f), Color.GhostWhite);
      _spriteBatch?.DrawString(_fonts["ui"], $"|", new Vector2(110f, 10f), Color.Teal);
      _spriteBatch?.DrawString(_fonts["ui"], $"{_objects.Count} Objects", new Vector2(130, 10f), Color.GhostWhite);
      _spriteBatch?.DrawString(_fonts["ui"], $"|", new Vector2(290f, 10f), Color.Teal);
      _spriteBatch?.DrawString(_fonts["ui"], $"{_collisions} Collisions", new Vector2(310, 10f), Color.GhostWhite);
      _spriteBatch?.DrawString(_fonts["ui"], $"|", new Vector2(490f, 10f), Color.Teal);
      _spriteBatch?.DrawString(_fonts["ui"], $"{_checks} Checks", new Vector2(510, 10f), Color.GhostWhite);
      _spriteBatch?.DrawString(_fonts["ui"], $"|", new Vector2(660f, 10f), Color.Teal);
      _spriteBatch?.DrawString(_fonts["ui"], $"{(int)_usualChecks / _checks}X Faster", new Vector2(680, 10f), Color.GhostWhite);

      _spriteBatch?.End();

      base.Draw(gameTime);
      _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
   }
}
