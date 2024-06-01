using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;

public abstract class PowerUp : GameObject, IDrawable, IMovable
{
    // public override double X
    // {
    //     get { return base.X; }
    //     set { base.X = value; }
    // }
    // public override double Y
    // {
    //     get { return base.Y; }
    //     set { base.Y = value; }
    // }
    // public override double Width
    // {
    //     get { return base.Width; }
    //     set { base.Width = value; }
    // }
    //
    // public override double Height
    // {
    //     get { return base.Height; }
    //     set { base.Height = value; }
    // }
    public double Speed { get; set; }
    public double PowerUpDirection { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    protected GameSingleton _game;

    protected PowerUp(GameSingleton game, double x, double y, double width, double height)
    {
        _game = game;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Speed = 2;
        PowerUpDirection = SplashKit.Rnd(-2, 2);
        VelocityX = SplashKit.Rnd(-1, 1);
        VelocityY = SplashKit.Rnd(-1, 1);
    }

    public void Move(Direction direction)
    {
        X += Speed * PowerUpDirection; // Update the x position
        Y += 3*Speed; // Update the y position to move downward

        // If the PowerUp goes off the screen, reset its position and direction
        if (Y > SplashKit.ScreenHeight() || X < 0 || X > SplashKit.ScreenWidth())
        {
            Y = 0;
            X = SplashKit.Rnd(0, SplashKit.ScreenWidth());
            PowerUpDirection = SplashKit.Rnd(-2, 2);
        }
    }

    public abstract void Draw(Window gameWindow);
    public abstract void Activate(Player player);
}