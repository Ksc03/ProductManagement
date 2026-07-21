namespace ProductManagement.Domain.Exceptions;

public sealed class AppValidationException : AppException
{
    public IEnumerable<string> Errors { get; }

    public AppValidationException(
        IEnumerable<string> errors)
        : base("Validation failed.", 400)
    {
        Errors = errors;
    }
}