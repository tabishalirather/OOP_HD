using System.Security.Cryptography.X509Certificates;
using SplashKitSDK;
public class Game
{
    private Window _gameWindow;
    private Bitmap _spriteSheet;
    private Player _player;
    private List<Enemy> _enemies;
    private List<Bullet> _bullets = new List<Bullet>();

    public Game()
    {
        _gameWindow = new Window("Maqsad", 800, 600);
        _spriteSheet = new Bitmap("SpriteSheet", @"V:\OOP Custom program\content\shipsheetparts.PNG");
        _player = new Player(_spriteSheet, 400, 550, 60, 50); // Player initialization
        _enemies = Enemy.Spawn(_spriteSheet, 5, _gameWindow); // Spawn 10 enemies
    }
    
     //TODO: `This is not working properly. It deletes enemies whenever a bullet is fired and goes crazy. 
public void CheckBulletEnemyCollisions()
{
    for (int i = _enemies.Count - 1; i >= 0; i--)
    {
        for (int j = _bullets.Count - 1; j >= 0; j--)
        {
            if (_enemies[i].Intersects(_bullets[j]))
            {
                Console.WriteLine($"Enemy hit at enemy position ({_enemies[i].X}, {_enemies[i].Y}) and bullet position ({_bullets[j].X}, {_bullets[j].Y})");
                break;
            }
        }
    }
}

    public void CheckCollisions()
{
    for (int i = _enemies.Count - 1; i >= 0; i--)
    {
        if (_player.Intersects(_enemies[i]))
        {
            // Check if the time since the last collision is greater than the collision cooldown
            if ((DateTime.Now - _enemies[i].LastCollisionTime) > _player.CollisionCooldown)
            {
                _enemies[i].LastCollisionTime = DateTime.Now;

                if (_player.NumberOfLives > 0)
                {
                    _player.NumberOfLives--;
                    Console.WriteLine("Number of lives left: " + _player.NumberOfLives);
                }
                else if(_player.NumberOfLives == 0)
                {
                    Console.WriteLine("Game Over!");
                }
                // Remove the enemy from the list
                _enemies.RemoveAt(i);
            }
        }
    }
    
    for(int i = _bullets.Count - 1; i >= 0; i--)
    {
            _bullets[i].UpdateBullet();
            if(_bullets[i].Y < 0)
            {
                _bullets.RemoveAt(i);
            }
    }
    
    
    // Collisions with enemies


    if (SplashKit.KeyTyped(KeyCode.SpaceKey))
    {
        _player.Shoot(_bullets, "up");
    }else if(SplashKit.KeyTyped(KeyCode.LeftShiftKey))
    {
        _player.Shoot(_bullets, "down");
    }
}
    

    public void Update()
    {
        // Update player
        _player.Update();
        CheckCollisions();
        CheckBulletEnemyCollisions();
        // Update enemies
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].Update();

            // If enemy is off screen, remove it and spawn a new one
            if (_enemies[i].IsOffScreen(_gameWindow))
            {
                _enemies.RemoveAt(i);
                _enemies.AddRange(Enemy.Spawn(_spriteSheet, 1, _gameWindow));
            }
        }
        
        for (int j = _bullets.Count - 1; j >= 0; j--)
        {
            _bullets[j].UpdateBullet();
            if (_bullets[j].Y < 0)
            {
                _bullets.RemoveAt(j);
            }
        }
    }

    public void Run()
    {
        while (!_gameWindow.CloseRequested)
        {
            SplashKit.ProcessEvents();

            // Call the Update method
            Update();
            // Drawing section
            _gameWindow.Clear(Color.Black);
            _player.Draw(_gameWindow);
            foreach (var enemy in _enemies)
            {
                enemy.Draw(_gameWindow);
            }
            foreach (var bullet in _bullets)
            {
                bullet.Draw(_gameWindow);
            }

            _gameWindow.Refresh(60);
        }

        _spriteSheet.Free();
        _gameWindow.Close();
    }
}