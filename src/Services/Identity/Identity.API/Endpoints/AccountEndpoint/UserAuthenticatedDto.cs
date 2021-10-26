namespace Identity.API.Endpoints.AccountEndpoint
{
    public record UserAuthenticatedDto(string AccessTokenValue, string RefreshTokenValue);
}
