namespace CUSTOM_PROGRAM_TEST;

using SplashKitSDK;

    public class StartState : IGameState
    {
        public void EnterState(GameSingleton game)
        {
            // Code to execute when entering Start state
        }

        public void Update(GameSingleton game)
        {
            if (SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                game.ChangeState(new RunningState());
            }
        }

        public void Draw(GameSingleton game)
        {
            game.GameWindow.Clear(Color.Black);
            game.DrawTextOnScreen("Press SPACE to Start", 300, 300, 40);
            game.GameWindow.Refresh(60);
        }
    }
