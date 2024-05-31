namespace CUSTOM_PROGRAM_TEST;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}
