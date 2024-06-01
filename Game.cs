using SplashKitSDK;

namespace CUSTOM_PROGRAM_TEST;

public class GameSingleton
{
    private static GameSingleton _instance;
    public Window GameWindow { get; private set; }
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
    private ScoreManager _scoreManager;
    private ScoreDisplay _scoreDisplay;
    private IGameState _currentState;
    private int _directionChangeCounter = 0;
    private const int DirectionChangeDelay = 2; 

    private string HighScoreFile { get; set; } = @"V:\OOP Custom program\content\highscore.txt";

    public static GameSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameSingleton();
            }
            return _instance;
        }
    }

    private GameSingleton()
    {
        GameWindow = new Window("Maqsad", 800, 600);
        _spriteSheet = new Bitmap("SpriteSheet", @"V:\OOP Custom program\content\shipsheetparts.PNG");
        _player = new Player(_spriteSheet, 240, 550, 60, 50);
        _enemyFactory = new EnemyFactory(_spriteSheet);
        _bulletFactory = new BulletFactory();
        _powerUpFactory = new PowerUpFactory();
        _enemies = SpawnEnemies(5);
        _scoreManager = new ScoreManager();
        _scoreDisplay = new ScoreDisplay(_scoreManager, GameWindow);
        _powerUps.Add(_powerUpFactory.Create(this,  PowerUpType.Shield, _scoreManager, 100, 100));
        _powerUps.Add(_powerUpFactory.Create(this, PowerUpType.ScoreBoost, _scoreManager,120, 60));


        ChangeState(new StartState());
    }

    public void ChangeState(IGameState newState)
    {
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void UpdateGameObjects()
    {
        UpdatePlayer();
        UpdatePowerUps();
        UpdatePlayerBullets();
        UpdateEnemies();
    }

    public void CheckCollisions()
    {
        CheckBulletEnemyCollisions();
        CheckPlayerEnemyCollisions();
        CheckPlayerHitByBullet();
        HandlePowerUp();
        HandleEnemyShooting();
        HandlePlayerShooting();
    }

    public void UpdatePlayer()
    {
        if (SplashKit.KeyDown(KeyCode.LeftKey))
        {
            _player.Move(Direction.Left);
        }
        if (SplashKit.KeyDown(KeyCode.RightKey))
        {
            _player.Move(Direction.Right);
        }
        if (SplashKit.KeyDown(KeyCode.UpKey))
        {
            _player.Move(Direction.Up);
        }
        if (SplashKit.KeyDown(KeyCode.DownKey))
        {
            _player.Move(Direction.Down);
        }
    }
    
    public Direction RandomDirection()
    {
        Random random = new Random();
        int randomInt = random.Next(4);
        return (Direction)randomInt;
    }
public void UpdatePowerUps()
{
    Random random = new Random();

    // Only change direction when the counter reaches the delay value
    if (_directionChangeCounter >= DirectionChangeDelay)
    {
        foreach (PowerUp powerUp in _powerUps)
        {
            // int randomInt = random.Next(4);
            Direction randomDirection = RandomDirection();
            powerUp.Move(randomDirection);
        }

        // Reset the counter
        _directionChangeCounter = 0;
    }
    else
    {
        // Increment the counter
        _directionChangeCounter++;
    }
}

    public void UpdateEnemyBullets()
    {
        for (int i = _enemyBullets.Count - 1; i >= 0; i--)
        {
            _enemyBullets[i].Move();
            if (_enemyBullets[i].Y > GameWindow.Height)
            {
                _enemyBullets.RemoveAt(i);
            }
        }
    }

    public void UpdatePlayerBullets()
    {
        for (int i = _playerBullets.Count - 1; i >= 0; i--)
        {
            _playerBullets[i].Move();
            if (_playerBullets[i].Y < 0 || _playerBullets[i].Y > GameWindow.Height)
            {
                _playerBullets.RemoveAt(i);
            }
        }
    }

    public void UpdateEnemies()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            _enemies[i].Move(Direction.Down);
            if (_enemies[i].IsOffScreen(GameWindow))
            {
                _enemies.RemoveAt(i);
            }
        }

        if (_enemies.Count == 0)
        {
            _enemies.AddRange(SpawnEnemies(5));
        }
    }

    public List<Enemy> SpawnEnemies(int count)
    {
        List<Enemy> enemies = new List<Enemy>();
        for (int i = 0; i < count; i++)
        {
            double x = SplashKit.Rnd(GameWindow.Width);
            double y = -SplashKit.Rnd(GameWindow.Height);
            enemies.Add(_enemyFactory.Create(x, y));
        }
        return enemies;
    }

    public void HandlePlayerShooting()
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

    public void HandleEnemyShooting()
    {
        if (SplashKit.Rnd() < 0.01)
        {
            foreach (Enemy enemy in _enemies)
            {
                _enemyBullets.Add(_bulletFactory.CreateEnemyBullet(enemy.X + enemy.Width / 2, enemy.Y, "down"));
            }
        }

        UpdateEnemyBullets();
    }

    public void HandlePowerUp()
    {
        for (int i = _powerUps.Count - 1; i >= 0; i--)
        {
            if (_player.Intersects(_powerUps[i]))
            {
                switch (_powerUps[i])
                {
                    case Shield shield:
                        shield.Activate(_player);
                        break;
                    case FriendShip friendShip:
                        friendShip.Activate(_player);
                        // ActivateFriendShip();
                        break;
                    case ScoreBoost scoreBoost:
                        scoreBoost.Activate(_player);
                        // ActivateFriendShip();
                        break;
                }
            _powerUps.RemoveAt(i);
            }
        }
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
                        _scoreManager.Score += 10;
                    }
                    else if (_playerBullets[j].Direction == "down")
                    {
                        _scoreManager.Score += 20;
                        _displayBonusHit = true;
                        _bonusHitCounter = 0;
                    }
                    _playerBullets.RemoveAt(j);
                    break;
                }
            }
        }
    }
    
//     public void MoveInRandomDirection()
// {
//     // Create a new Random instance
//     Random random = new Random();
//
//     foreach (PowerUp powerUp in _powerUps)
//     {
//
//         int randomInt = random.Next(4);
//         
//         Direction randomDirection = (Direction)randomInt;
//
//
//         powerUp.Move(randomDirection);
//     }
// }

    public void CheckPlayerEnemyCollisions()
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

    public void CheckPlayerHitByBullet()
    {
        for (int i = _enemyBullets.Count - 1; i >= 0; i--)
        {
            // Console.WriteLine("Checking for bulle tcollison");
            if (_player.Intersects(_enemyBullets[i]))
            {
                Console.WriteLine("Player hit by bullet");
                HandlePlayerHit();
                _enemyBullets.RemoveAt(i);
                break;
            }
        }
    }

    public void HandlePlayerHit()
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
                ChangeState(new GameOverState());
            }
        }
    }

    public void ResetGame()
    {
        _player.NumberOfLives = 3;
        _enemies.Clear();
        _playerBullets.Clear();
        _enemyBullets.Clear();
        _powerUps.Clear();
        _enemies.AddRange(SpawnEnemies(5));
    }

    public void DrawGameObjects()
    {
        _player.Draw(GameWindow);
        foreach (Enemy enemy in _enemies)
        {
            enemy.Draw(GameWindow);
        }

        foreach (Bullet bullet in _playerBullets)
        {
            bullet.Draw(GameWindow);
        }

        foreach (Bullet bullet in _enemyBullets)
        {
            bullet.Draw(GameWindow);
        }

        foreach (PowerUp powerUp in _powerUps)
        {
            powerUp.Draw(GameWindow);
        }
    }

    public void DrawUI()
    {
        var texts = new List<(string Text, int Y)>
        {
            ($"Lives: {_player.NumberOfLives}", 10),
            ($"Score: {_scoreManager.Score}", 30),
            ($"High Score: {HighScore}", 50)
        };

        foreach (var (text, y) in texts)
        {
            DrawTextOnScreen(text, 10, y);
        }
    }

    public void DrawTextOnScreen(string text, float x, float y, int fontSize = 20)
    {
        GameWindow.DrawText(text, Color.White, "Arial", fontSize, x, y);
    }

    public void DisplayBonusHitText()
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
        while (!GameWindow.CloseRequested)
        {
            SplashKit.ProcessEvents();
            _currentState.Update(this);
            _currentState.Draw(this);
        }

        _spriteSheet.Free();
        GameWindow.Close();
    }

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
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}
public enum PowerUpType
{
    Shield,
    FriendShip,
    ScoreBoost
    
}