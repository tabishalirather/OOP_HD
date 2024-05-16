using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Net.NetworkInformation;
using oop_custom_program;
using SplashKitSDK;

public class Game
{
    private Window _gameWindow;
    private Bitmap _spriteSheet;
    private Player _player;
    private List<Enemy> _enemies;
    private List<Bullet> _playerBullets = new List<Bullet>();
    private List<Bullet> _enemyBullets = new List<Bullet>();
    private List<PowerUp> _powerUps = new List<PowerUp>();
    private bool _displayBonusHit;
    private int _bonusHitCounter;
    private int CurrentScore { get; set; } = 0;
    string HighScoreFile { get; set;  } = @"V:\OOP Custom program\content\highscore.txt";


    public enum GameState
    {
        Running,
        GameOver,
        Restart
    }

    public GameState State { get; set; } = GameState.Running;

   public int HighScore
{
    get
    {
        if (File.Exists(HighScoreFile))
        {
            string fileContent = File.ReadAllText(HighScoreFile);
            return string.IsNullOrEmpty(fileContent) ? 0 : int.Parse(fileContent);
        }
        else
        {
            return 0;
        }
    }
    set
    {
        File.WriteAllText(HighScoreFile, value.ToString());
    }
}

    public Game()
    {
        _gameWindow = new Window("Maqsad", 800, 600);
        _spriteSheet = new Bitmap("SpriteSheet", @"V:\OOP Custom program\content\shipsheetparts.PNG");
        _player = new Player(_spriteSheet, 240, 550, 60, 50); // Player initialization
        _enemies = Enemy.Spawn(_spriteSheet, 5, _gameWindow); // Spawn 10 enemies

        _powerUps.Add(new PowerUp(PowerUp.PowerUpType.Shield, 100, 100, 50, 50));
    }


    //TODO: `This is not working properly. It deletes enemies whenever a bullet is fired and goes crazy. -- Resolved
    public void CheckBulletEnemyCollisions()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            for (int j = _playerBullets.Count - 1; j >= 0; j--)
            {
                if (_enemies[i].Intersects(_playerBullets[j]))
                {
                    Console.WriteLine(
                        $"Enemy hit at enemy position ({_enemies[i].X}, {_enemies[i].Y}) and bullet position ({_playerBullets[j].X}, {_playerBullets[j].Y})");
                    _enemies.RemoveAt(i); // Remove the hit enemy
                    
                    if(_playerBullets[j].Direction == "up")
                    {
                        CurrentScore += 10; // Increase the score by 10
                    }
                    else if (_playerBullets[j].Direction == "down")
                    {
                        CurrentScore += 20;
                        _displayBonusHit = true; // Set the flag to display the bonus hit text
                        _bonusHitCounter = 0;
                    }
                    _playerBullets.RemoveAt(j); // Remove the bullet that hit the enemy
                    // CurrentScore += 10; // Increase the score by 10
                    GameState state = GameState.Running;
                    break; // Exit the inner loop since the enemy is already removed
                }
            }
        }
    }


    public void Update()
    {
        // Handle player input and movements
        UpdatePlayer();

        // Update all bullets and handle their lifecycle
        UpdatePlayerBullets();

        // Update enemy positions and check for off-screen enemies
        UpdateEnemies();

        // Check for collisions between bullets and enemies
        CheckBulletEnemyCollisions();

        // Check for collisions between the player and enemies
        CheckPlayerEnemyCollisions();

        CheckPlayerHitByBullet();

        CheckPlayerPowerUpCollisions();

        HandlePowerUpCollision();

        HandleEnemyShooting();
    }

    private void UpdatePlayer()
    {
        _player.Update();
        HandlePlayerShooting();
    }

    private void UpdateEnemyBullets()
    {
        for (int i = _enemyBullets.Count - 1; i >= 0; i--)
        {
            _enemyBullets[i].UpdateBullet();
            if (_enemyBullets[i].Y > _gameWindow.Height)
            {
                _enemyBullets.RemoveAt(i); // Remove bullets that go off-screen
            }
        }
    }
    private void UpdatePlayerBullets()
    {
        for (int i = _playerBullets.Count - 1; i >= 0; i--)
        {
            _playerBullets[i].UpdateBullet();
            if (_playerBullets[i].Y < 0 || _playerBullets[i].Y > _gameWindow.Height)
            {
                _playerBullets.RemoveAt(i); // Remove bullets that go off-screen
            }
        }
    }

    private void CheckPlayerPowerUpCollisions()
    {
        for (int i = _powerUps.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_powerUps[i]))
            {
                _powerUps[i].Activate(_player);
                _powerUps.RemoveAt(i);
            }
        }
    }
    private void UpdateEnemies()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].Update();
            if (_enemies[i].IsOffScreen(_gameWindow))
            {
                _enemies.RemoveAt(i); // Remove enemies that go off-screen
            }
        }

        // If all enemies have moved off the screen, spawn new enemies
        if (_enemies.Count == 0)
        {
            _enemies.AddRange(Enemy.Spawn(_spriteSheet, 5, _gameWindow)); // Spawn new enemies
        }
    }

    private void HandlePlayerShooting()
    {
        if (SplashKit.KeyTyped(KeyCode.SpaceKey))
        {
            _player.Shoot(_playerBullets, "up");
        }

        if (SplashKit.KeyTyped(KeyCode.LeftShiftKey))
        {
            _player.Shoot(_playerBullets, "down");
        }
    }

    private void HandleEnemyShooting()
    {
        // Make enemies shoot bullets periodically
        if (SplashKit.Rnd() < 0.01) // Adjust this value to control the shooting frequency
        {
            foreach (var enemy in _enemies)
            {
                enemy.Shoot(_enemyBullets);
            }
        }

        // Update enemy bullets and handle their lifecycle
        UpdateEnemyBullets();

        // Check for collisions between the player and enemy bullets
        // CheckPlayerBulletCollisions();
    }

    private void HandlePowerUpCollision()
    {
        for (int i = _powerUps.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_powerUps[i]))
            {
                _powerUps[i].Activate(_player);
                _powerUps.RemoveAt(i);
                
                DrawTextOnScreen("Shield is up", 60, 70);
            }
        }
    }

    private void CheckPlayerEnemyCollisions()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_enemies[i]))
            {
                Console.WriteLine("Player hit by enemy in CheckPlayerEnemyCollisions!");
                HandlePlayerHit();
                _enemies.RemoveAt(i);
            }
        }
    }

    private void CheckPlayerHitByBullet()
    {
        for (int i = _enemyBullets.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_enemyBullets[i]))
            {
                Console.WriteLine("Player hit by bullet");
                HandlePlayerHit();
                _enemyBullets.RemoveAt(i); // Remove the bullet that hit the player
                break; // Exit the loop since the player is already hit
            }
        }
    }


    private void HandlePlayerHit()
    {
        // If the player has a shield, deactivate it and do not decrease lives
        if (_player.Shield)
        {
            _player.Shield = false;
        }
        else
        {
            // If the player does not have a shield, decrease lives
            if (_player.NumberOfLives > 0)
            {
                _player.NumberOfLives--;
            }
            else
            {
                Console.WriteLine("Game Over!");
                State = GameState.GameOver;
            }
        }
    }
    private void ResetGame()
    {
        // Reset player's lives
        _player.NumberOfLives = 3;

        // Clear all enemies, bullets, and power-ups
        _enemies.Clear();
        _playerBullets.Clear();
        _enemyBullets.Clear();
        _powerUps.Clear();

        // Spawn new enemies
        _enemies.AddRange(Enemy.Spawn(_spriteSheet, 5, _gameWindow));
    }
    

    public void DrawTextOnScreen(string text, float x, float y, int fontSize = 20)
    {
        _gameWindow.DrawText(text, Color.White, "Arial", fontSize, x, y);
    }

    public void DisplayBonusHitText()
    {
        if (_displayBonusHit)
        {
            DrawTextOnScreen("Bonus Hit!", 60, 70); 
            _bonusHitCounter++; // Increase the counter

            if (_bonusHitCounter >= 100) // small delay
            {
                _displayBonusHit = false; // Stop displaying the bonus hit text
            }
        }
    }


    public void Run()
    {
        while (!_gameWindow.CloseRequested)
        {
            SplashKit.ProcessEvents();
            
            if (State == GameState.GameOver)
            {
                string gameOverText = "Game Over! Press R to restart.";
                float xText = (_gameWindow.Width - gameOverText.Length) / 2;
                float yText = (_gameWindow.Height - gameOverText.Length) / 2;
                DrawTextOnScreen(gameOverText, xText, yText, 50);

                if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    State = GameState.Restart;
                }

                if (CurrentScore > HighScore)
                {
                    HighScore = CurrentScore;
                }

                CurrentScore = 0;
            }
            else if (State == GameState.Restart)
            {
                State = GameState.Running;
                ResetGame();
            }
            else if (State == GameState.Running)
            {
                _gameWindow.Clear(Color.Black);
               DisplayBonusHitText();
                Update();
            }

            // Drawing section
            // _gameWindow.Clear(Color.Black);
            _player.Draw(_gameWindow);
            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(_gameWindow);
            }

            foreach (Bullet bullet in _playerBullets)
            {
                bullet.Draw(_gameWindow);
            }

            foreach (Bullet bullet in _enemyBullets)
            {
                bullet.Draw(_gameWindow);
            }

            foreach (PowerUp powerUp in _powerUps)
            {
                powerUp.Draw(_gameWindow);
            }

            var texts = new List<(string Text, int Y)>
            {
                ($"Lives: {_player.NumberOfLives}", 10),
                ($"Score: {CurrentScore}", 30),
                ($"High Score: {HighScore}", 50)
            };

            foreach (var (text, y) in texts)
            {
                DrawTextOnScreen(text, 10, y);
            }

            _gameWindow.Refresh(60);
        }

        _spriteSheet.Free();
        _gameWindow.Close();
    }
}