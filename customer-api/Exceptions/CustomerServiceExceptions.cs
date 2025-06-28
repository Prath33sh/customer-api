namespace CustomerApi.Exceptions
{
    public class CustomerAlreadyExistsException(string message) : Exception(message)
    {
    }

    public class DBOperationException(string message) : Exception(message)
    {
    }

    public class NotFoundException(string message) : Exception(message)
    {
    }
}