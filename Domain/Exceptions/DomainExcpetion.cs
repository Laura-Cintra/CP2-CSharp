namespace Cp2Mottu.Domain.Exceptions;

public class DomainExcpetion : Exception
{
    public DomainExcpetion(string message, string item) : base(message)
    {
    }
}
