using AutoMapper;

namespace LearningMpaAbp
{
    /// <summary>
    ///     实现该接口以进行映射规则创建
    /// </summary>
    internal interface IDtoMapping
    {
        void CreateMapping(IMapperConfigurationExpression mapperConfig);
    }
}