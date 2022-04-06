namespace Identity.API.Endpoints.AccountEndpoint
{
    public record UserAuthenticatedDto(string AccessToken, string Roles);
}