using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.FunctionalTests.Mocks
{
    public static class MockUserManager
    {
        public static Mock<UserManager<TUser>> GetMock<TUser>()
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mockedUserManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);

            mockedUserManager.Object.UserValidators.Add(new UserValidator<TUser>());
            mockedUserManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            //mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
            //    .ReturnsAsync(IdentityResult.Success)
            //    .Callback<TUser, string>((x, y) => ls.Add(x));

            mockedUserManager.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockedUserManager.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            mockedUserManager.Setup(x => x.DeleteAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            return mockedUserManager;
        }
    }
}
