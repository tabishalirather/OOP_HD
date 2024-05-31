namespace CUSTOM_PROGRAM_TEST;

public interface IFactory<T>
{
    abstract T Create(double x, double y);
}
