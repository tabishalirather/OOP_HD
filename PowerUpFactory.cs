using CUSTOM_PROGRAM_TEST;

public class PowerUpFactory
{
    public PowerUp Create(GameSingleton game, PowerUpType type, double x, double y, double width = 50, double height = 50)
    {
        switch (type)
        {
            case PowerUpType.Shield:
                return new Shield(game, x, y, width, height);
            case PowerUpType.FriendShip:
                return new FriendShip(game, x, y, width, height);
            // Add cases for other powerup types here...
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}