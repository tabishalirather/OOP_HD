using SplashKitSDK;
namespace CUSTOM_PROGRAM_TEST;

    public class PausedState : IGameState
    {
        public void EnterState(GameSingleton game)
        {
            // Code to execute when entering Paused state
        }

        public void Update(GameSingleton game)
        {
            if (SplashKit.KeyTyped(KeyCode.PKey))
            {
                game.ChangeState(new RunningState());
            }
        }

        public void Draw(GameSingleton game)
        {
            game.GameWindow.Clear(Color.Black);
            game.DrawGameObjects();
            game.DrawTextOnScreen("Game Paused - Press P to Resume", 200, 300, 40);
            game.GameWindow.Refresh(60);
        }
    }
