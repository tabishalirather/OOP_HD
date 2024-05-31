using oop_custom_program;

using SplashKitSDK;

namespace oop_custom_program
{
    public class ScoreDisplay : IObserver
    {
        private ScoreManager _scoreManager;
        private Window _window;

        public ScoreDisplay(ScoreManager scoreManager, Window window)
        {
            _scoreManager = scoreManager;
            _window = window;
            _scoreManager.Attach(this);
        }

        public void Update()
        {
            Draw();
        }

        public void Draw()
        {
            _window.DrawText($"Score: {_scoreManager.Score}", Color.White, "Arial", 20, 10, 30);
        }
    }
}
