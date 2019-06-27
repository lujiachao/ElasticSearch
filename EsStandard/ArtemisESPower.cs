using Microsoft.Extensions.DependencyInjection;
using System;

namespace Artemis
{
    public abstract class ArtemisESPower
    {
        /// <summary>
        /// 单节点注册方案
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceCollection Register(IServiceCollection serviceCollection, Uri httpUri, string defaultIndex = "default")
        {
            ArtemisOption.connectionType = EnumConnectionType.Single;
            ArtemisOption.singleUri = httpUri;
            ArtemisOption.IndexType = defaultIndex;
            return serviceCollection;
        }

        ///<summary>
        ///多节点解决方案
        /// </summary>
        public static IServiceCollection Register(IServiceCollection serviceCollection, Uri[] httpUris, string defaultIndex = "default")
        {
            ArtemisOption.connectionType = EnumConnectionType.Single;
            ArtemisOption.poolUri = httpUris;
            ArtemisOption.IndexType = defaultIndex;
            return serviceCollection;
        }

        /// <summary>
        /// 单节点注册方案
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static void Register(Uri httpUri, string defaultIndex = "default")
        {
            ArtemisOption.connectionType = EnumConnectionType.Single;
            ArtemisOption.singleUri = httpUri;
            ArtemisOption.IndexType = defaultIndex;
        }

        ///<summary>
        ///多节点解决方案
        /// </summary>
        public static void Register(Uri[] httpUris, string defaultIndex = "default")
        {
            ArtemisOption.connectionType = EnumConnectionType.Pool;
            ArtemisOption.poolUri = httpUris;
            ArtemisOption.IndexType = defaultIndex;
        }
    }
}
