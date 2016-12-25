using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LearningMpaAbp.Users.Dto;

namespace LearningMpaAbp.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsersAsync();

        Task CreateUser(CreateUserInput input);

        ListResultDto<UserListDto> GetUsers();
    }
}