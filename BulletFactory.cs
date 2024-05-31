using oop_custom_program;

using SplashKitSDK;

namespace oop_custom_program
{
    public class BulletFactory
    {
        public  Bullet Create(double x, double y, string direction, int width = 2, int height = 2)
        
        {
            return new Bullet(x, y, direction, width, height); // Customize dimensions as necessary
        }
    }
}
