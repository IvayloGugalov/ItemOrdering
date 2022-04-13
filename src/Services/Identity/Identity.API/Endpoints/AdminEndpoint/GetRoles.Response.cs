using System.Collections.Generic;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public class GetRolesResponse
    {
        public IEnumerable<string> Roles { get; init; }
    }
}
