namespace Identity.Domain.Interfaces
{
    public interface IRefreshTokenValidator
    {
        TokenValidationResult Validate(string refreshTokenValue);
    }
}