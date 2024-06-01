using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;

public class FriendShip : PowerUp
{
    private bool _isActive = false;
    private double _speed = 5;
    private bool _movingRight = true;

    public FriendShip(GameSingleton game, double x, double y, double width = 50, double height = 50)
        : base(game, x, y, width, height)
    {
    }

    public override void Activate(Player player)
    {
        // Increase the player's number of lives
        player.NumberOfLives += 1; 
        Console.WriteLine("FriendShip Active");
        SplashKit.DrawText("FriendShip Active", Color.White,"Arial",12, X, Y);
    }

    public override void Draw(Window gameWindow)
    {
        gameWindow.FillRectangle(Color.Green, X, Y, Width, Height);
        gameWindow.DrawText("Activate FriendShip", Color.White,"Arial",12, X, Y);
    }

    public  bool Intersects(Player player)
    {
        if (base.Intersects(player))
        {
            _isActive = true;
            return true;
        }
        return false;
    }

    // public void Update()
    // {
    //     if (_isActive)
    //     {
    //         // Move the powerup left and right
    //         if (_movingRight)
    //         {
    //             X += _speed;
    //             if (X > GameWindow.Width - Width)
    //             {
    //                 _movingRight = false;
    //             }
    //         }
    //         else
    //         {
    //             X -= _speed;
    //             if (X < 0)
    //             {
    //                 _movingRight = true;
    //             }
    //         }
    //
    //         // Shoot bullets at enemies
    //         ShootBullets();
    //     }
    // }

    private void ShootBullets()
    {
        // Implement the logic to shoot bullets at enemies here
        // You might need to add a list of bullets to the FriendShip class and a method to create bullets
    }
}