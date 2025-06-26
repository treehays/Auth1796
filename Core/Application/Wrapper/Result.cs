namespace Auth1796.Core.Application.Wrapper;

public class Result : IResult
{
    public Result()
    {
    }

    public string Message { get; set; } = string.Empty;

    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }

    public static IResult Fail(int statusCode = 400)
    {
        return new Result { Succeeded = false, StatusCode = statusCode };
    }

    public static IResult Fail(string message, int statusCode = 400)
    {
        return new Result { Succeeded = false, Message = message, StatusCode = statusCode };
    }

    /*public static IResult Fail(List<string> messages)
    {
        return new Result { Succeeded = false, Messages = messages };
    }*/

    public static Task<IResult> FailAsync(int statusCode = 400)
    {
        return Task.FromResult(Fail(statusCode));
    }

    public static Task<IResult> FailAsync(string message, int statusCode = 400)
    {
        return Task.FromResult(Fail(message, statusCode));
    }

    /*public static Task<IResult> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }*/

    public static IResult Success(int statusCode = 200)
    {
        return new Result { Succeeded = true, StatusCode = statusCode };
    }

    public static IResult Success(string message, int statusCode = 200)
    {
        return new Result { Succeeded = true, Message = message, StatusCode = statusCode };
    }

    /*public static IResult Success(List<string> messages)
    {
        return new Result { Succeeded = true, Messages = messages };
    }*/

    public static Task<IResult> SuccessAsync(int statusCode = 200)
    {
        return Task.FromResult(Success(statusCode));
    }

    public static async Task<IResult> SuccessAsync(string message, int statusCode = 200)
    {
        return await Task.FromResult(Success(message, statusCode));
    }

    /*public static Task<IResult> SuccessAsync(List<string> messages)
    {
        return Task.FromResult(Success(messages));
    }*/
}

public class ErrorResult<T> : Result<T>
{
    public string Source { get; set; }

    public string Exception { get; set; }

    public string ErrorId { get; set; }
    public string SupportMessage { get; set; }
    public int StatusCode { get; set; }
}

public class Result<T> : Result, IResult<T>
{
    public Result()
    {
    }

    public T Data { get; set; }

    public new static Result<T> Fail(int statusCode = 400)
    {
        return new() { Succeeded = false, StatusCode = statusCode };
    }

    public new static Result<T> Fail(string message, int statusCode = 400)
    {
        return new() { Succeeded = false, Message = message, StatusCode = statusCode };
    }

    public static ErrorResult<T> ReturnError(string message, int statusCode = 500)
    {
        return new() { Succeeded = false, Message = message, StatusCode = statusCode };
    }

    /*public new static Result<T> Fail(List<string> messages)
    {
        return new() { Succeeded = false, Messages = messages };
    }*/

    /*public static ErrorResult<T> ReturnError(List<string> messages)
    {
        return new() { Succeeded = false, Messages = messages, StatusCode = 500 };
    }*/

    public new static Task<Result<T>> FailAsync(int statusCode = 400)
    {
        return Task.FromResult(Fail(statusCode));
    }

    public new static Task<Result<T>> FailAsync(string message, int statusCode = 400)
    {
        return Task.FromResult(Fail(message, statusCode));
    }

    public static Task<ErrorResult<T>> ReturnErrorAsync(string message, int statusCode = 500)
    {
        return Task.FromResult(ReturnError(message, statusCode));
    }

    /*public new static Task<Result<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }*/

    /*public static Task<ErrorResult<T>> ReturnErrorAsync(List<string> messages)
    {
        return Task.FromResult(ReturnError(messages));
    }*/

    public new static Result<T> Success(int statusCode = 200)
    {
        return new() { Succeeded = true, StatusCode = statusCode };
    }

    public new static Result<T> Success(string message, int statusCode = 200)
    {
        return new() { Succeeded = true, Message = message, StatusCode = statusCode };
    }

    /*public new static Result<T> Success(List<string> messages)
    {
        return new() { Succeeded = true, Messages = messages };
    }*/

    public static Result<T> Success(T data, int statusCode = 200)
    {
        return new() { Succeeded = true, Data = data, StatusCode = statusCode };
    }

    public static Result<T> Success(T data, string message, int statusCode = 200)
    {
        return new() { Succeeded = true, Data = data, Message = message, StatusCode = statusCode };
    }

    /*public static Result<T> Success(T data, List<string> messages)
    {
        return new() { Succeeded = true, Data = data, Messages = messages };
    }*/

    public new static Task<Result<T>> SuccessAsync(int statusCode = 200)
    {
        return Task.FromResult(Success(statusCode));
    }

    //public new static Task<Result<T>> SuccessAsync(string message)
    //{
    //    return Task.FromResult(Success(message));
    //}

    public new static Task<Result<T>> SuccessAsync(string message, int statusCode = 200)
    {
        return Task.FromResult(Success(message, statusCode));
    }

    /*public new static Task<Result<T>> SuccessAsync(List<string> messages)
    {
        return Task.FromResult(Success(messages));
    }*/

    public static Task<Result<T>> SuccessAsync(T data, int statusCode = 200)
    {
        return Task.FromResult(Success(data, statusCode));
    }

    //public static Task<Result<T>> SuccessAsync(T data, string message)
    //{
    //    return Task.FromResult(Success(data, message));
    //}

    public static Task<Result<T>> SuccessAsync(T data, string message, int statusCode = 200)
    {
        return Task.FromResult(Success(data, message, statusCode));
    }
    /*public static Task<Result<T>> SuccessAsync(T data, List<string> messages)
    {
        return Task.FromResult(Success(data, messages));
    }*/
}