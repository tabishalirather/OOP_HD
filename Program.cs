using CUSTOM_PROGRAM_TEST;

public class Program
{
    public static void Main() 
    {
        GameSingleton game = GameSingleton.Instance;
        game.Run();
    }
}