namespace CUSTOM_PROGRAM_TEST;
using SplashKitSDK;
public abstract class GameEntity : IMovable, IDrawable
{
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    protected Bitmap Texture { get; set; }

    protected GameEntity(Bitmap texture, float x, float y, int width, int height)
    {
        Texture = texture;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public abstract void Draw(Window gameWindow);

    public virtual void Move()
    {
        
    }
}
