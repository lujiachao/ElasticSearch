using System;
using System.Collections.Generic;
using System.Text;

namespace Artemis
{
    public static class ArtemisOption
    {
        /// <summary>
        /// 单节点多节点处理方案
        /// </summary>
        public static EnumConnectionType connectionType { get; set;}

        /// <summary>
        /// 单节点配置
        /// </summary>
        public static Uri singleUri { get; set; }

        /// <summary>
        /// 集群处理Uri集合1
        /// </summary>
        public static Uri[] poolUri { get; set; }

        /// <summary>
        /// 索引方式
        /// </summary>
        public static string IndexType { get; set; }
    }
}
