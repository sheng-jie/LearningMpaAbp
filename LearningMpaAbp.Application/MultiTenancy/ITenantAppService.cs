using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LearningMpaAbp.MultiTenancy.Dto;

namespace LearningMpaAbp.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
