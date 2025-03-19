namespace Warehouse;

public class ContainerOverweightException : Exception
{
    public ContainerOverweightException(string message) : base(message)
    {
    }
}