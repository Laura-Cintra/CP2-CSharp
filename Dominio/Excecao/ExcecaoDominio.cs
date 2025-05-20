namespace Cp2Mottu.Domain.Exceptions;

public class ExcecaoDominio : Exception
{
    public ExcecaoDominio(string message, string item) : base(message)
    {
    }
}
