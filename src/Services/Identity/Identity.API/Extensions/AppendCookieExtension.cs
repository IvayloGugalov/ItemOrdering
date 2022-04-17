using Microsoft.AspNetCore.Http;

namespace Identity.API.Extensions
{
    public static class AppendCookieExtension
    {
        public const string REFRESH_TOKEN_NAME = "refresh_token";

        public static void AppendRefreshToken(this IResponseCookies cookies, string refreshToken)
        {
            cookies.Append(
                REFRESH_TOKEN_NAME,
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }
    }
}
