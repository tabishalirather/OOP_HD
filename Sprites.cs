using SplashKitSDK;

public class Sprite
{
    private Bitmap _sheet;
    private Rectangle _sourceRect;

    public Sprite(Bitmap sheet, double x, double y, int width, int height)
    {
        _sheet = sheet;
        // Create a rectangle from the given dimensions
        _sourceRect = SplashKit.RectangleFrom(x, y, width, height);
    }

    public void Draw(Window window, float x, float y)
    {
        // Ensure to use the correct method signature and pass the Rectangle correctly
        window.DrawBitmap(_sheet, x, y, SplashKit.OptionPartBmp(_sourceRect));
    }
}
