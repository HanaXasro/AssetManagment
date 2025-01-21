namespace Application.Exceptions;

public class UnprocessableException: Exception
{
    public UnprocessableException(string name, object key) : base($"{name} ({key})") { }
}