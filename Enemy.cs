using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;
// This is, for experiment
public class Enemy : GameObject, IDrawable, IMovable
{
    private Bitmap _bitmap;
    private double _x, _y;
    private Rectangle _sourceRect;
    // public DateTime LastCollisionTime { get; set; }
    private double _enemySpeed = 2.8;
    public Enemy(Bitmap sheet, double x, double y, int width, int height)
    {
        _bitmap = sheet;
        _x = x;
        _y = y;
        _sourceRect = SplashKit.RectangleFrom(7, 300, width, height); // Adjust according to specific sprite coordinates
    }

    public void Move()
    {
        // Move down gradually to simulate coming towards the player
        _y += _enemySpeed;
    }

    public void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_bitmap, (float)_x, (float)_y, SplashKit.OptionPartBmp(_sourceRect));
    }

    public bool IsOffScreen(Window gameWindow)
    {
        // Check if the enemy has moved off the bottom of the screen
        return _y > gameWindow.Height;
    }

    public static List<Enemy> Spawn(Bitmap sheet, int numberOfEnemies, Window gameWindow)
    {
        List<Enemy> enemies = new List<Enemy>();
        List<Point2D> usedCoordinates = new List<Point2D>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Generate unique coordinates
            double x, y;
            do
            {
                x = SplashKit.Rnd(gameWindow.Width);
                y = -SplashKit.Rnd(gameWindow.Width);
            } while (usedCoordinates.Any(point => point.X == x || point.Y == y));

            usedCoordinates.Add(new Point2D { X = x, Y = y });
            enemies.Add(new Enemy(sheet, x, y, 60, 50));
        }

        return enemies;
    }
    
    public bool Intersects(Bullet bullet)
    {
        // Define the boundary of the bullet
        double bulletRightEdge = bullet.X + 2; // Bullet width is considered as 2 for collision
        double bulletLeftEdge = bullet.X;
        double bulletTopEdge = bullet.Y;
        double bulletBottomEdge = bullet.Y + 2; // Bullet height is considered as 2 for collision

        // Define the boundary of the enemy
        double enemyRightEdge = _x + _sourceRect.Width;
        double enemyLeftEdge = _x;
        double enemyTopEdge = _y;
        double enemyBottomEdge = _y + _sourceRect.Height;

        // Check if there is an overlap between the bullet's and enemy's boundaries
        bool horizontalOverlap = enemyRightEdge > bulletLeftEdge && enemyLeftEdge < bulletRightEdge;
        bool verticalOverlap = enemyBottomEdge > bulletTopEdge && enemyTopEdge < bulletBottomEdge;

        // Return true if both horizontal and vertical overlaps exist, indicating a collision
        return horizontalOverlap && verticalOverlap;
    }


    public double X
    {
        get { return _x; }
    }
    public double Y
    {
        get { return _y; }
    }
    public double Width
    {
        get { return _sourceRect.Width; }
    }
    public double Height
    {
        get { return _sourceRect.Height; }
    }
        
}