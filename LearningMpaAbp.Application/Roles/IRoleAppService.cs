using System.Threading.Tasks;
using Abp.Application.Services;
using LearningMpaAbp.Roles.Dto;

namespace LearningMpaAbp.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
