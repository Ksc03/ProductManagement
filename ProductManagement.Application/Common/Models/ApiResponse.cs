namespace ProductManagement.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message,
        int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> FailureResponse(
        string message,
        IEnumerable<string>? errors = null,
        int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };
    }
}