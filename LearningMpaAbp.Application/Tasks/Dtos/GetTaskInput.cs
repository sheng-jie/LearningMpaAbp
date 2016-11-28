using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class GetTaskInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string FilterText { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}