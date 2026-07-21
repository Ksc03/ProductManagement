namespace ProductManagement.Application.DTOs.Authentication;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpiresOn { get; set; }
}