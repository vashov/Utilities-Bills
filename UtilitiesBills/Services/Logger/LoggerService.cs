using NLog;
using NLog.Config;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace UtilitiesBills.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public void Initialize(Assembly assembly, string assemblyName)
        {
            using (Stream nlogConfigFile = GetEmbeddedResourceStream(assembly, "NLog.config"))
            {
                if (nlogConfigFile != null)
                {
                    using (XmlReader xmlReader = XmlReader.Create(nlogConfigFile))
                    {
                        LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
                    }
                }
            }
        }

        private Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourcePaths = assembly.GetManifestResourceNames()
              .Where(x => x.EndsWith(resourceFileName, StringComparison.OrdinalIgnoreCase))
              .ToList();
            if (resourcePaths.Count == 1)
            {
                return assembly.GetManifestResourceStream(resourcePaths.Single());
            }
            return null;
        }
    }
}
