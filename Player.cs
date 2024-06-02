using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;
public class Player : GameObject, IMovable, IDrawable
{
    private readonly Bitmap _playerBitmap;
    private double _x, _y;
    private readonly Rectangle _sourceRect;
    private bool _shield;

    public override double Width
    {
        get { return _sourceRect.Width; }
    }
    public override double Height
    {
        get { return _sourceRect.Height; }
    }

    public override double X
    {
        get { return _x; }
        set { _x = value; }
    }
    public override double Y
    {
        get { return _y; }
        set { _y = value; }
    }
    public TimeSpan CollisionCooldown { get; set; } = TimeSpan.FromSeconds(1);

    private double _playerSpeed = 1.8;
    

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
        X = _x;
        Y = _y;
    }
    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                _x -= _playerSpeed;
                break;
            case Direction.Right:
                _x += _playerSpeed;
                break;
            case Direction.Up:
                _y -= _playerSpeed;
                break;
            case Direction.Down:
                _y += _playerSpeed;
                break;
            default:
                throw new ArgumentException("Invalid direction");
        }

        X = _x;
        Y = _y;
        KeepPlayerOnScreen();
    }

    public void KeepPlayerOnScreen()
    {
        _x = Math.Max(0, Math.Min(SplashKit.ScreenWidth() - _sourceRect.Width, _x));
         _y = Math.Max(0, Math.Min(SplashKit.ScreenHeight() - _sourceRect.Height, _y));
    }

    public void Draw(Window gameWindow)
    {
        gameWindow.DrawBitmap(_playerBitmap, (float)_x, (float)_y, SplashKit.OptionPartBmp(_sourceRect));
    } 
    public override bool IsOffScreen(Window gameWindow)
    {
        return false;
    }
}