namespace Cp2Mottu.Domain.Exceptions;

public class DomainExcepetion : Exception
{
    public DomainExcepetion(string message, string item) : base(message)
    {
    }
}
