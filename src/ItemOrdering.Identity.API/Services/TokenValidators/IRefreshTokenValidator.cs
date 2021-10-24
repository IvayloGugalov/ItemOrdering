namespace ItemOrdering.Identity.API.Services.TokenValidators
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshTokenValue);
    }
}