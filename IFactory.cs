namespace oop_custom_program;

public interface IFactory<T>
{
    abstract T Create(double x, double y);
}
