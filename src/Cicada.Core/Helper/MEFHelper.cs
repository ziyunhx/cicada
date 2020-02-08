using Cicada.Core.Enums;
using Cicada.Core.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Cicada.Core.Helper
{
    public static class ContainerConfigurationExtensions
    {
        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return WithAssembliesInPath(configuration, path, null, searchOption);
        }

        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, AttributedModelProvider conventions, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var assemblies = Directory
                .GetFiles(path, "*.dll", searchOption)
                .Select(AssemblyLoadContext.GetAssemblyName)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyName)
                .ToList();

            configuration = configuration.WithAssemblies(assemblies, conventions);

            return configuration;
        }
    }

    public class MEFHelper
    {
        private CompositionHost _container = null;

        ILog logger = LogManager.GetLogger(typeof(MEFHelper));

        /// <summary>
        /// Initializes a new instance of the <see cref="ImitateLogin.MEFHelper"/> class.
        /// </summary>
        /// <param name="path">Path.</param>
        public MEFHelper(string path = "")
        {
            try
            {
                var conventions = new ConventionBuilder();
                conventions
                    .ForTypesDerivedFrom<IMEFOperation>()
                    .Export<IMEFOperation>()
                    .Shared();

                var assemblies = new[] { typeof(IMEFOperation).GetTypeInfo().Assembly };

                var configuration = new ContainerConfiguration()
                    .WithAssemblies(assemblies, conventions);

                _container = configuration.CreateContainer();

                if (string.IsNullOrEmpty(path))
                    path = "Extensions";

                if (Directory.Exists(path))
                    configuration = new ContainerConfiguration().WithAssembliesInPath(path, conventions);
                else if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    configuration = new ContainerConfiguration().WithAssembliesInPath(Path.Combine(Directory.GetCurrentDirectory(), path), conventions);
                else
                    logger.Warn("No MEF extensions path has configured.");

                //Create the CompositionContainer with the parts in the catalog
                _container.SatisfyImports = new CompositionContainer(catalog);

                //Fill the imports of this object
                this._container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Operation the specified loginSite, imageUrl and image.
        /// </summary>
        /// <param name="checkExtensionName">the check extension name.</param>
        /// <param name="timeout">the milliseconds of timeout. 0 is no timeout.</param>
        /// <param name="param"></param>
        public CheckResult Operation(string checkExtensionName, double timeout = 0, params string[] param)
        {
            ILog logger = LogManager.GetLogger(typeof(MEFHelper));
            try
            {
                foreach (Lazy<IMEFOperation, IPluginData> i in operations)
                {
                    if (i.Metadata.PluginName.ToLower() == checkExtensionName.ToLower())
                    {
                        //create the check id and recode the check datetime.
                        string hashId = AllHelper.getMd5Hash(checkExtensionName+DateTime.Now.ToString());//2016-12-28 修改 生成HashId
                        DateTime checkDateTime = DateTime.Now;

                        CheckResult checkResult = ((IMEFOperation)i.Value.Clone()).Operate(hashId, checkDateTime, param);//2017-01-22 修改 插件初始化引用问题修复
                        checkResult.CheckId = hashId;
                        checkResult.CheckDateTime = checkDateTime;
                        long costTime = Convert.ToInt64((DateTime.Now - checkDateTime).TotalMilliseconds);//2017-01-16修改 取整数操作
                        checkResult.CostMillisecond = costTime;
                        if (timeout > 0 && checkResult.CostMillisecond > timeout)
                        {
                            if (checkResult.Status == 200 || checkResult.MessageLevel == EventLevel.Normal || checkResult.MessageLevel == EventLevel.Info)
                            {
                                checkResult.Status = 504;
                                checkResult.MessageLevel = EventLevel.Warn;
                            }
                            checkResult.MessageInfo = string.Format("504 timeout. The limit millisecond is {0}, but it cost {1}.", timeout, checkResult.CostMillisecond) + Environment.NewLine + checkResult.MessageInfo;
                        }

                        return checkResult;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Can't call the operation method." + Environment.NewLine + ex.ToString());
                return new CheckResult() { CheckId = Guid.NewGuid().ToString(), CheckDateTime = DateTime.Now, CostMillisecond = -1, MessageInfo = "Can't call the operation method.", MessageLevel = EventLevel.Critical, Status = 500 };
            }

            logger.Error("Operation Not Found!");
            return new CheckResult() { CheckId = Guid.NewGuid().ToString(), CheckDateTime = DateTime.Now, CostMillisecond = -1, MessageInfo = "Operation not found!", MessageLevel = EventLevel.Critical, Status = 404 };
        }

        /// <summary>
        /// Get All PluginNames
        /// </summary>
        /// <returns></returns>
        public string[] GetAllPluginNames()
        {
            List<string> Plugins = new List<string>();
            foreach(var i in operations)
            {
                Plugins.Add(i.Metadata.PluginName);
            }
            return Plugins.ToArray();
        }

        /// <summary>
        /// Get All Plugins
        /// </summary>
        /// <returns></returns>
        public IMEFOperation[] GetAllPlugins()
        {
            List<IMEFOperation> MefOperations = new List<IMEFOperation>();
            foreach(var i in operations)
            {
                MefOperations.Add(i.Value);
            }
            return MefOperations.ToArray();
        }

        /// <summary>
        /// Get Plugin By Name
        /// </summary>
        /// <param name="name">Plugin Name</param>
        /// <returns></returns>
        public IMEFOperation GetPlugin(string name)
        {
            var result= operations.Where(i => i.Metadata.PluginName == name).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            return (IMEFOperation)result.Value.Clone();
        }
    }
}
