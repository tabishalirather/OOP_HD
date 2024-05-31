using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;

public class PowerUp : GameObject, IDrawable, IMovable
{
    public enum PowerUpType { 
        Speed, 
        Shield, 
        ExtraBullets 
    }

    public PowerUpType Type { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    // public bool IsActive { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Speed { get; set; }
    public double PowerUpDirection { get; set; }   

    public PowerUp(PowerUpType type, double x, double y, double width, double height)
    {
        Type = type;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Speed = 2;
        PowerUpDirection = SplashKit.Rnd(-2, 2);
        // IsActive = false;
    }

    public void Draw(Window gameWindow)
    {
        if (Type == PowerUpType.Shield)
        {
            gameWindow.FillRectangle(Color.Blue, X, Y, Width, Height);
            gameWindow.DrawText("Activate shield", Color.White,"Arial",12, X, Y);
        }
    }
    
    public void Move()
    {
        X += Speed * PowerUpDirection; // Move the x position
        Y += Speed; // Move the y position to move downward

        // If the PowerUp goes off the screen, reset its position and direction
        if (Y > SplashKit.ScreenHeight() || X < 0 || X > SplashKit.ScreenWidth())
        {
            Y = 0;
            X = SplashKit.Rnd(0, SplashKit.ScreenWidth());
            PowerUpDirection = SplashKit.Rnd(-2, 2);
        }
    }
    public void Activate(Player player)
    {
        switch (Type)
        {
            case PowerUpType.Shield:
                player.Shield = true; // Activate the player's shield
                Console.WriteLine("Shield Active");
                SplashKit.DrawText("Shield Active", Color.White,"Arial",12, X, Y);
                break;
            case PowerUpType.ExtraBullets:
                player.NumberOfLives += 1; // Give the player an extra life
                break;
            case PowerUpType.Speed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}