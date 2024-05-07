using SplashKitSDK;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class Enemy
{
    private Bitmap _bitmap;
    private double _x, _y;
    private Rectangle _sourceRect;
    public DateTime LastCollisionTime { get; set; }

    public Enemy(Bitmap sheet, double x, double y, int width, int height)
    {
        _bitmap = sheet;
        _x = x;
        _y = y;
        _sourceRect = SplashKit.RectangleFrom(7, 300, width, height); // Adjust according to specific sprite coordinates
    }

    public void Update()
    {
        // Move down gradually to simulate coming towards the player
        _y += 3;
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
        // Get the position and radius of the bullet
        double bulletX = _x;
        double bulletY = _y;
        double bulletRadius = 2;

        // Get the position and radius of the enemy
        double enemyX = X;
        double enemyY = Y;
        double enemyRadius = Width / 2; // Assuming the enemy is a circle, its radius is half its width

        // Calculate the distance between the centers of the bullet and the enemy
        double distance = Math.Sqrt(Math.Pow(bulletX - enemyX, 2) + Math.Pow(bulletY - enemyY, 2));

        // If the distance is less than the sum of the radii, the circles intersect
        return distance < bulletRadius + enemyRadius;
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