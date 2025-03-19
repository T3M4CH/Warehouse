namespace Warehouse.Helpers;

public class OperationResult
{
    public bool IsSuccess { get; private set; }
    public string ErrorMessage { get; private set; }

    private OperationResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static OperationResult Success()
    {
        return new OperationResult(true, null);
    }

    public static OperationResult Failure(string errorMessage)
    {
        return new OperationResult(false, errorMessage);
    }
}

public class OperationResult<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public string ErrorMessage { get; private set; }

    private OperationResult(bool isSuccess, T data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>(true, data, null);
    }

    public static OperationResult<T> Failure(string errorMessage)
    {
        return new OperationResult<T>(false, default, errorMessage);
    }
}