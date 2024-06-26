﻿using SplashKitSDK;

namespace oop_custom_program;

public class Player : GameObject
{
    private readonly Bitmap _playerBitmap;
    private double _x, _y;
    private readonly Rectangle _sourceRect;
    private bool _shield;
    private double Width { get; set; }
    private double Height { get; set; }
    public TimeSpan CollisionCooldown { get; set; } = TimeSpan.FromSeconds(1);
    

    public int NumberOfLives { get; set; } = 3;

    public bool Shield { get; set; }

    public Player(Bitmap sheet, double x, double y, int width, int height)
    {
        _playerBitmap = sheet;
        _x = x;
        _y = y;
        Width = width;
        Height = height;
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
        if(_shield)
        {
            return false;
        }
        return _x < enemy.X + enemy.Width &&
               _x + _sourceRect.Width > enemy.X &&
               _y < enemy.Y + enemy.Height &&
               _y + _sourceRect.Height > enemy.Y;
    }
    
    public bool Intersects(PowerUp powerUp)
    {
        return _x < powerUp.X + powerUp.Width &&
               _x + _sourceRect.Width > powerUp.X &&
               _y < powerUp.Y + powerUp.Height &&
               _y + _sourceRect.Height > powerUp.Y;
    }
    
    public bool Intersects(Bullet bullet)
    {
    
        // if(Shield)
        // {
        //     return false;
        // }

        return _x < bullet.X + bullet.Width &&
               _x + Width > bullet.X &&
               _y < bullet.Y + bullet.Height &&
               _y + Height > bullet.Y;
    }


    public void Shoot(List<Bullet> bullets, string direction)
    {
        bullets.Add(new Bullet(_x + _sourceRect.Width / 2, _y, direction));
    }
    
    private double _transparency = 1.0; // Full opacity initially
    // private int _hitCount = 0;
    // private int _maxHits = 3; // Number of hits after which the game is over

    // Constructor and existing methods...

    public void DecreaseTransparency()
    {
        _transparency -= 0.33; // Decrease opacity by about 33% per hit
        if (_transparency < 0) _transparency = 0; // Ensure transparency does not go negative
    }

    // public void Draw(Window gameWindow)
    // {
    //     // Use the transparency level in the drawing options
    //     DrawingOptions options = new DrawingOptions();
    //     options.WithAlpha((byte)(_transparency * 255));
    //     gameWindow.DrawBitmap(_playerBitmap, (float)_x, (float)_y, options);
    // }
}