using SplashKitSDK;
namespace CUSTOM_PROGRAM_TEST;
    public class RestartState : IGameState
    {
        public void EnterState(GameSingleton game)
        {
            game.ResetGame();
            game.ChangeState(new StartState());
        }

        public void Update(GameSingleton game)
        {
            // No update logic needed for Restart state
        }

        public void Draw(GameSingleton game)
        {
            // No drawing logic needed for Restart state
        }
    }
