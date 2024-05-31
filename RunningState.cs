using  CUSTOM_PROGRAM_TEST;
using SplashKitSDK;


public class RunningState : IGameState
    {
        public void EnterState(GameSingleton game)
        {
            // Code to execute when entering Running state
        }

        public void Update(GameSingleton game)
        {
            if (SplashKit.KeyTyped(KeyCode.PKey))
            {
                game.ChangeState(new PausedState());
                return;
            }

            if (SplashKit.KeyTyped(KeyCode.RKey))
            {
                game.ChangeState(new RestartState());
            }
            game.UpdatePlayer();
            game.UpdateGameObjects();
            game.CheckCollisions();
        }

        public void Draw(GameSingleton game)
        {
            game.GameWindow.Clear(Color.Black);
            game.DisplayBonusHitText();
            game.DrawGameObjects();
            game.DrawUI();
            game.GameWindow.Refresh(60);
        }
    }