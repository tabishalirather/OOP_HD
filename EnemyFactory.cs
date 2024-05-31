namespace oop_custom_program;
using SplashKitSDK;

public class EnemyFactory
{
    private Bitmap _spriteSheet;

    public EnemyFactory(Bitmap spriteSheet)
    {
        _spriteSheet = spriteSheet;
    }

    public Enemy Create(double x, double y)
    {
        return new Enemy(_spriteSheet, x, y, 60, 50); // Customize dimensions as necessary
    }
}

