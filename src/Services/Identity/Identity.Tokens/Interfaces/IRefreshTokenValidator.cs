namespace Identity.Tokens.Interfaces
{
    public interface IRefreshTokenValidator
    {
        TokenValidationResult Validate(string refreshTokenValue);
    }
}
