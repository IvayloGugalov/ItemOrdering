using System.Collections.Generic;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public class GetUsersResponse
    {
        public IEnumerable<UserDto> Users;
    }
}
