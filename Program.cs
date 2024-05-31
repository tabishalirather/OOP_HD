using oop_custom_program;

public class Program
{
    public static void Main() 
    {
        GameSingleton game = GameSingleton.Instance;
        game.Run();
    }
}