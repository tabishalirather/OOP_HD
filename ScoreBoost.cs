using CUSTOM_PROGRAM_TEST;
using SplashKitSDK;

public class ScoreBoost : PowerUp
{
    private int _scoreIncrease;
    private ScoreManager _scoreManager;

    public ScoreBoost(GameSingleton game, ScoreManager scoreManager, double x, double y, double width = 50,
        double height = 50, int scoreIncrease = 100)
        : base(game, x, y, width, height)
    {
        _scoreIncrease = scoreIncrease;
        _scoreManager = scoreManager;
    }

    public override void Draw(Window gameWindow)
    {
        gameWindow.FillRectangle(Color.Yellow, X, Y, Width, Height);
        gameWindow.DrawText("Score Boost", Color.Black, "Arial", 12, X, Y);
    }

    public override void Activate(Player player)
    {
        _scoreManager.Score += _scoreIncrease;
        Console.WriteLine("Score Boost Activated");
        SplashKit.DrawText("Score Boost Activated", Color.White, "Arial", 12, X, Y);

        // No need to wait, so we remove the delay and the call to Deactivate
    }
}