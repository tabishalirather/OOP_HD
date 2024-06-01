using SplashKitSDK;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CUSTOM_PROGRAM_TEST;

public class FriendShip : PowerUp
{
    private bool _isActive = false;
    private double _speed = 5;
    private bool _movingRight = true;
    private Timer _timer;
    private List<Bullet> _bullets = new List<Bullet>();

    public FriendShip(GameSingleton game, double x, double y, double width = 50, double height = 50)
        : base(game, x, y, width, height)
    {
        _timer = new Timer(10000); // Set the timer to 10 seconds
        _timer.Elapsed += OnTimerElapsed;
    }

    public override void Draw(Window gameWindow)
    {
        gameWindow.FillRectangle(Color.Green, X, Y, Width, Height);
        gameWindow.DrawText("Activate FriendShip", Color.White,"Arial",12, X, Y);
    }

    public override void Activate(Player player)
    {
        _isActive = true;
        _timer.Start(); // Start the timer when the power-up is activated
        Console.WriteLine("FriendShip Active");
        SplashKit.DrawText("FriendShip Active", Color.White,"Arial",12, X, Y);
    }

    public void Update()
    {
        if (_isActive)
        {
            // Move the FriendShip object left and right
            if (_movingRight)
            {
                X += _speed;
                if (X > SplashKit.ScreenWidth() - Width)
                {
                    _movingRight = false;
                }
            }
            else
            {
                X -= _speed;
                if (X < 0)
                {
                    _movingRight = true;
                }
            }

            // Shoot bullets at enemies
            ShootBullets();
        }
    }

    private void ShootBullets()
    {
        // Create a new bullet and add it to the list of bullets
        _bullets.Add(new Bullet(X, Y, "up",5));
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _isActive = false; // Stop the FriendShip object from shooting bullets after 10 seconds
        _timer.Stop();
    }
}