namespace Authentication.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ResultError? Error { get; }

    protected Result(
        bool isSuccess,
        ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(
        string code,
        string message)
    {
        return new Result(
            false,
            new ResultError(code, message));
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(
        T value)
        : base(true, null)
    {
        Value = value;
    }

    private Result(
        ResultError error)
        : base(false, error)
    {
        Value = default;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(
        string code,
        string message)
    {
        return new Result<T>(
            new ResultError(code, message));
    }
}