
namespace CUSTOM_PROGRAM_TEST;

    public class PowerUpFactory
    {
        public PowerUp Create(PowerUp.PowerUpType type, double x, double y, double width = 50, double height = 50)
        {
            return new PowerUp(type, x, y, width, height); // Customize dimensions as necessary
        }
    }
