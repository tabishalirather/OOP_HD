using SplashKitSDK;
using System.Collections.Generic;
using System.IO;

using oop_custom_program;

    public class Game
    {
        private Window _gameWindow;
        private Bitmap _spriteSheet;
        private Player _player;
        private List<Enemy> _enemies;
        private EnemyFactory _enemyFactory;
        private BulletFactory _bulletFactory;
        private PowerUpFactory _powerUpFactory;
        private List<Bullet> _playerBullets = new List<Bullet>();
        private List<Bullet> _enemyBullets = new List<Bullet>();
        private List<PowerUp> _powerUps = new List<PowerUp>();
        private bool _displayBonusHit;
        private int _bonusHitCounter;
        private int CurrentScore { get; set; }
        private string HighScoreFile { get; set; } = @"V:\OOP Custom program\content\highscore.txt";

        public enum GameState
        {
            Running,
            GameOver,
            Restart
        }

        private GameState State { get; set; } = GameState.Running;

        private int HighScore
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
            _player = new Player(_spriteSheet, 240, 550, 60, 50);
            _enemyFactory = new EnemyFactory(_spriteSheet);
            _bulletFactory = new BulletFactory();
            _powerUpFactory = new PowerUpFactory();
            _enemies = SpawnEnemies(5);
            _powerUps.Add(_powerUpFactory.Create(PowerUp.PowerUpType.Shield, 100, 100));
        }

        private List<Enemy> SpawnEnemies(int count)
        {
            List<Enemy> enemies = new List<Enemy>();
            for (int i = 0; i < count; i++)
            {
                double x = SplashKit.Rnd(_gameWindow.Width);
                double y = -SplashKit.Rnd(_gameWindow.Height);
                enemies.Add(_enemyFactory.Create(x, y));
            }
            return enemies;
        }

        public void CheckBulletEnemyCollisions()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                for (int j = _playerBullets.Count - 1; j >= 0; j--)
                {
                    if (_enemies[i].Intersects(_playerBullets[j]))
                    {
                        Console.WriteLine($"Enemy hit at enemy position ({_enemies[i].X}, {_enemies[i].Y}) and bullet position ({_playerBullets[j].X}, {_playerBullets[j].Y})");
                        _enemies.RemoveAt(i);

                        if (_playerBullets[j].Direction == "up")
                        {
                            CurrentScore += 10;
                        }
                        else if (_playerBullets[j].Direction == "down")
                        {
                            CurrentScore += 20;
                            _displayBonusHit = true;
                            _bonusHitCounter = 0;
                        }
                        _playerBullets.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        public void Update()
        {
            UpdatePlayer();
            UpdatePowerUps();
            UpdatePlayerBullets();
            UpdateEnemies();
            CheckBulletEnemyCollisions();
            CheckPlayerEnemyCollisions();
            CheckPlayerHitByBullet();
            HandlePowerUp();
            HandleEnemyShooting();
            HandlePlayerShooting();
        }

        private void UpdatePlayer()
        {
            _player.Move();
        }

        private void UpdatePowerUps()
        {
            _powerUps.ForEach(powerUp => powerUp.Move());
        }

        private void UpdateEnemyBullets()
        {
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                _enemyBullets[i].Move();
                if (_enemyBullets[i].Y > _gameWindow.Height)
                {
                    _enemyBullets.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayerBullets()
        {
            for (int i = _playerBullets.Count - 1; i >= 0; i--)
            {
                _playerBullets[i].Move();
                if (_playerBullets[i].Y < 0 || _playerBullets[i].Y > _gameWindow.Height)
                {
                    _playerBullets.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemies()
        {
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Move();
                if (_enemies[i].IsOffScreen(_gameWindow))
                {
                    _enemies.RemoveAt(i);
                }
            }

            if (_enemies.Count == 0)
            {
                _enemies.AddRange(SpawnEnemies(5));
            }
        }

        private void HandlePlayerShooting()
        {
            if (SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                _playerBullets.Add(_bulletFactory.Create(_player.X + _player.Width / 2, _player.Y, "up"));
            }

            if (SplashKit.KeyTyped(KeyCode.LeftShiftKey))
            {
                _playerBullets.Add(_bulletFactory.Create(_player.X + _player.Width / 2, _player.Y, "down"));
            }
        }

        private void HandleEnemyShooting()
        {
            if (SplashKit.Rnd() < 0.01)
            {
                foreach (var enemy in _enemies)
                {
                    _enemyBullets.Add(_bulletFactory.Create(enemy.X + enemy.Width / 2, enemy.Y, "down"));
                }
            }

            UpdateEnemyBullets();
        }

        private void HandlePowerUp()
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
                    _enemyBullets.RemoveAt(i);
                    break;
                }
            }
        }

        private void HandlePlayerHit()
        {
            if (_player.Shield)
            {
                _player.Shield = false;
            }
            else
            {
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
            _player.NumberOfLives = 3;
            _enemies.Clear();
            _playerBullets.Clear();
            _enemyBullets.Clear();
            _powerUps.Clear();
            _enemies.AddRange(SpawnEnemies(5));
        }

        private void DrawTextOnScreen(string text, float x, float y, int fontSize = 20)
        {
            _gameWindow.DrawText(text, Color.White, "Arial", fontSize, x, y);
        }

        private void DisplayBonusHitText()
        {
            if (_displayBonusHit)
            {
                DrawTextOnScreen("Bonus Hit!", 60, 70);
                _bonusHitCounter++;

                if (_bonusHitCounter >= 100)
                {
                    _displayBonusHit = false;
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
