using System;
using System.Threading.Tasks;
using Identity.API.Extensions;
using Identity.API.Models;
using MongoDB.Driver;

namespace Identity.API.Services.Deprecated
{
    [Obsolete]
    public class UserRepositoryObsolete
    {
        [Obsolete("Another way of saving the user to a MongoDB without using Identity from ASP .NET CORE")]
        private interface IUserRepository
        {
            Task<User> GetByIdAsync(Guid userId);
            Task<User> GetByEmailAsync(string email);
            Task<User> GetByUserNameAsync(string username);
            Task CreateAsync(User user);
            Task<DeleteResult> DeleteAsync(Guid userId);
        }

        private class UserRepository : IUserRepository
        {
            private readonly IMongoCollection<User> userCollection;

            public UserRepository(IMongoDatabaseSettings settings)
            {
                this.userCollection = MongoExtension.GetCollection<User>(settings, settings.UsersCollectionName);
            }

            public async Task<User> GetByIdAsync(Guid userId)
            {
                return await this.userCollection.Find(user => user.Id == userId)
                    .SingleOrDefaultAsync();
            }

            public async Task<User> GetByEmailAsync(string email)
            {
                return await this.userCollection.Find(user => user.Email == email)
                    .SingleOrDefaultAsync();
            }

            public async Task<User> GetByUserNameAsync(string username)
            {
                return await this.userCollection.Find(user => user.UserName == username)
                    .SingleOrDefaultAsync();
            }

            public async Task CreateAsync(User user)
            {
                await this.userCollection.InsertOneAsync(user);
            }

            public async Task<DeleteResult> DeleteAsync(Guid userId)
            {
                var existingUser = await this.GetByIdAsync(userId);

                if (existingUser is null) throw new Exception("User does not exist is DB.");

                var result = await this.userCollection.DeleteOneAsync(user => user.Id == userId);

                return result;
            }
        }

        /// <summary>
        /// Implementation of http calls for creating, deleting, refreshing an account
        /// using MongoDB instead of Identity Core
        /// </summary>
        private class AccountController
        {
            #region Register
            //[HttpPost(RegisterRequest.ROUTE)]
            //[AllowAnonymous]
            //[ProducesResponseType(StatusCodes.Status201Created)]
            //[ProducesResponseType(StatusCodes.Status400BadRequest)]
            //public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
            //{
            //    if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            //    if (request.Password != request.ConfirmPassword) return BadRequest(new ErrorResponse("Password does not match confirm password."));

            //    var existingUserByEmail = await this.userRepository.GetByEmailAsync(request.Email);
            //    if (existingUserByEmail != null) return Conflict(new ErrorResponse("Email already exists."));

            //    var existingUserByUsername = await this.userRepository.GetByUserNameAsync(request.Username);
            //    if (existingUserByUsername != null) return Conflict(new ErrorResponse("UserName already exists."));

            //    var registrationUser = new User(
            //        firstName: request.FirstName,
            //        lastName: request.LastName,
            //        email: request.Email,
            //        username: request.Username,
            //        passwordHash: request.Password.HashPassword(),
            //        address: request.Address);

            //    await this.userRepository.CreateAsync(registrationUser);

            //    return Ok();
            //}
            #endregion

            #region Login
            //[HttpPost(LoginRequest.ROUTE)]
            //[AllowAnonymous]
            //public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest loginRequest)
            //{
            //    if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            //    var user = await this.userRepository.GetByUserNameAsync(loginRequest.Username);
            //    if (user is null) return Unauthorized(new ErrorResponse("UserName does not exist."));

            //    if (!loginRequest.Password.VerifyPassword(user.PasswordHash)) return Unauthorized(new ErrorResponse("Password is incorrect."));

            //    var response = await this.authenticator.AuthenticateUserAsync(user);

            //    return Ok(response);
            //}
            #endregion

            #region RefreshToken
            //[HttpPost(RefreshRequest.ROUTE)]
            //public async Task<ActionResult> RefreshTokenAsync([FromBody] RefreshRequest refreshRequest)
            //{
            //    if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            //    if (!this.refreshTokenValidator.Validate(refreshRequest.RefreshTokenValue)) return this.BadRequest(new ErrorResponse("Invalid refresh token"));

            //    // TODO: Expired? Invalid signature? Handle cases
            //    var refreshToken = await this.refreshTokenRepository.GetByTokenValue(refreshRequest.RefreshTokenValue);
            //    if (refreshToken is null) return NotFound(new ErrorResponse("Invalid refresh token"));

            //    // Invalidate the refresh token
            //    await this.refreshTokenRepository.DeleteAsync(refreshToken.Id);

            //    var user = await this.userRepository.GetByIdAsync(refreshToken.UserId);
            //    if (user is null) return NotFound(new ErrorResponse("User not found"));

            //    var response = await this.authenticator.AuthenticateUserAsync(user);
            //    return Ok(response);
            //}
            #endregion

            #region DeleteUser
            //[HttpDelete(DeleteRequest.ROUTE)]
            //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            //public async Task<IActionResult> DeleteAsync()
            //{
            //    var unparsedUserId = HttpContext.User.FindFirstValue("id");

            //    if (!Guid.TryParse(unparsedUserId, out var userId)) return Unauthorized();

            //    var deleteUserResult = await this.userRepository.DeleteAsync(userId);
            //    if (!deleteUserResult.IsAcknowledged) return BadRequest(new ErrorResponse("Unable to delete user"));

            //    await this.refreshTokenRepository.DeleteAllForUserAsync(userId);

            //    return Ok("Deleted");
            //}
            #endregion

        }
    }
}