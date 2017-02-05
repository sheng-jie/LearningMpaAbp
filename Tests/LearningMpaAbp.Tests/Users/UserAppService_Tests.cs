using System.Data.Entity;
using System.Threading.Tasks;
using LearningMpaAbp.Users;
using LearningMpaAbp.Users.Dto;
using Shouldly;
using Xunit;

namespace LearningMpaAbp.Tests.Users
{
    public class UserAppService_Tests : LearningMpaAbpTestBase
    {
        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        private readonly IUserAppService _userAppService;

        [Fact]
        public async Task CreateUser_Test()
        {
            //Act
            await _userAppService.CreateUser(
                new CreateUserInput
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = "john.nash"
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            //Act
            var output = await _userAppService.GetUsersAsync();

            //Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}