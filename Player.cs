using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;

public class Player
{
    private Bitmap _playerBitmap;
    private double _x, _y;
    private Rectangle _sourceRect;
    private int numberOfLives = 3;
    public TimeSpan CollisionCooldown { get; set; } = TimeSpan.FromSeconds(1);

    public int NumberOfLives
    {
        get { return numberOfLives; }
        set { numberOfLives = value; }
    }

    public Player(Bitmap sheet, double x, double y, int width, int height)
    {
        _playerBitmap = sheet;
        _x = x;
        _y = y;
        _sourceRect = SplashKit.RectangleFrom(7, 0, width, height); 
    }

    public void Update()
    {
        // Move forward automatically


        // Respond to arrow keys
        if (SplashKit.KeyDown(KeyCode.UpKey))
        {
            _y -= 3;
        }

        if (SplashKit.KeyDown(KeyCode.LeftKey))
        {
            _x -= 5;
        }

        if (SplashKit.KeyDown(KeyCode.RightKey))
        {
            _x += 5;
        }

        if (SplashKit.KeyDown(KeyCode.DownKey))
        {
            _y += 5;
        }

        // Keep the player within window bounds
        _x = Math.Max(0, Math.Min(SplashKit.ScreenWidth() - _sourceRect.Width, _x));
        _y = Math.Max(0, Math.Min(SplashKit.ScreenHeight() - _sourceRect.Height, _y));
    }

    public void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_playerBitmap, (float)_x, (float)_y, SplashKit.OptionPartBmp(_sourceRect));
    }

    public bool Intersects(Enemy enemy)
    {
        return _x < enemy.X + enemy.Width &&
               _x + _sourceRect.Width > enemy.X &&
               _y < enemy.Y + enemy.Height &&
               _y + _sourceRect.Height > enemy.Y;
    }

    public void Shoot(List<Bullet> bullets, string direction)
    {
        bullets.Add(new Bullet(_x + _sourceRect.Width / 2, _y, direction));
    }
}