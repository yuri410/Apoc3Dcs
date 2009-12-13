using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  可以快速查询Cluster的哈希表
    /// </summary>
    public class ClusterTable : ClusterTableBase<Block>
    {
        public ClusterTable(Block[] clusters)
            : base(clusters)
        {
        }
    }
}
