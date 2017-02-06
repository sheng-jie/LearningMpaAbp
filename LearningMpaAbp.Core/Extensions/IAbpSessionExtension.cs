using Abp.Runtime.Session;

namespace LearningMpaAbp.Extensions
{
    public interface IAbpSessionExtension : IAbpSession
    {
        string Email { get; }
    }
}