using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;

public class Shield : PowerUp
{
    public Shield(GameSingleton game, double x, double y, double width = 50, double height = 50)
        : base(game, x, y, width, height)
    {
    }

    public override void Activate(Player player)
    {
        player.Shield = true; // Activate the player's shield
        Console.WriteLine("Shield Active");
        SplashKit.DrawText("Shield Active", Color.White,"Arial",12, X, Y);
    }

    public override void Draw(Window gameWindow)
    {
        gameWindow.FillRectangle(Color.Blue, X, Y, Width, Height);
        gameWindow.DrawText("Activate shield", Color.White,"Arial",12, X, Y);
    }
}