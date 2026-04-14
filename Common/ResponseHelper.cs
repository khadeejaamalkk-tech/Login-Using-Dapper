namespace LoginRegistrationUsingDapper.Common;

public class ResponseHelper
{
    public static ApiResponse<T> Success<T>(T data, string message, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Status = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }
    public static ApiResponse<T> Failure<T>(string message, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Status = false,
            StatusCode = statusCode,
            Message = message,
            Data = default(T)
        };
    }
}
