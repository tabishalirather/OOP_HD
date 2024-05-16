using oop_custom_program;
using SplashKitSDK;

public class PowerUp : GameObject
{
    public enum PowerUpType { Speed, Shield, ExtraLife }
    public PowerUpType Type { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public bool IsActive { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public PowerUp(PowerUpType type, double x, double y, double width, double height)
    {
        Type = type;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        IsActive = false;
    }

    public void Draw(Window gameWindow)
    {
        if (Type == PowerUpType.Shield)
        {
            gameWindow.FillRectangle(Color.Blue, X, Y, Width, Height);
        }
    }
    public void Activate(Player player)
    {
        switch (Type)
        {
            case PowerUpType.Shield:
                player.Shield = true; // Activate the player's shield
                Console.WriteLine("Shield is up");
                break;
            case PowerUpType.ExtraLife:
                player.NumberOfLives += 1; // Give the player an extra life
                break;
            case PowerUpType.Speed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}