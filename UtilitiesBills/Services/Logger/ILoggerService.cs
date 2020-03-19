using System.Reflection;

namespace UtilitiesBills.Services.Logger
{
    public interface ILoggerService
    {
        void Initialize(Assembly assembly, string assemblyName);
    }
}
