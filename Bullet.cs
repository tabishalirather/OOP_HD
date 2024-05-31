using oop_custom_program;
using SplashKitSDK;
public class Bullet : GameObject
{
    private double _x, _y;
    private string _direction;
    private const double Speed = 10.0;
    private int _width, _height;
    
    public double Width { get; set; }
    public double Height { get; set; }

    public Bullet(double x, double y, string direction, int width = 2,int height = 2) 
    {
        _x = x;
        _y = y;
        _direction = direction;
        Width = width;
        Height = height;
    }

    public void UpdateBullet()
    {
        // Move the bullet upwards
        // _y -=  Speed;
        if(_direction == "up")
        {
            _y -= (Speed);
        }else if (_direction == "down")
        {
            _y +=  (Speed);
            // Console.WriteLine("Shooting down");
        }
    }

    public void Draw(Window gameWindow)
    {
        gameWindow.DrawCircle(Color.White, (float)_x, (float)_y, 2);
    }
    


    public double X { get { return _x; } }
    public double Y { get { return _y; } }
    
    public string Direction{ get { return _direction; } }
}
