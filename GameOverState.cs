using SplashKitSDK;
namespace CUSTOM_PROGRAM_TEST;

    public class GameOverState : IGameState
    {
        public void EnterState(GameSingleton game)
        {
            // Code to execute when entering GameOver state
        }

        public void Update(GameSingleton game)
        {
            if (SplashKit.KeyTyped(KeyCode.RKey))
            {
                game.ChangeState(new RestartState());
            }
        }

        public void Draw(GameSingleton game)
        {
            game.GameWindow.Clear(Color.Black);
            game.DrawTextOnScreen("Game Over! Press R to Restart", 200, 300, 40);
            game.GameWindow.Refresh(60);
        }
    }
