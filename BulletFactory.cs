using oop_custom_program;

using SplashKitSDK;

namespace oop_custom_program
{
    public class BulletFactory
    {
        private int _playerRadius = 3;
        private int _enemyRadius = 8;
        int width = 2;
        int height = 2;
        public  Bullet Create(double x, double y, string direction)
        
        {
            Console.WriteLine("Player bullet c'd");
            return new Bullet(x, y, direction, _playerRadius); // Customize dimensions as necessary
        }
        
        public Bullet CreateEnemyBullet(double x, double y, string direction)
        {
            Console.WriteLine("Enemy bullet c'd");
            return new Bullet(x, y, direction, _enemyRadius);
        }
    }
}
