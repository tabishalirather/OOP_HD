namespace CUSTOM_PROGRAM_TEST;

public interface IGameState
{
    void EnterState(GameSingleton game);
    void Update(GameSingleton game);
    void Draw(GameSingleton game);
}