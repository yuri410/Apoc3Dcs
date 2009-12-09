using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Apoc3D.Ide
{
    public interface IPlugin : Apoc3D.Core.IPlugin
    {

        /// <summary>
        ///  获取插件的图标
        /// </summary>
        Icon PluginIcon
        {
            get;
        }

        /// <summary>
        ///  表示是否在“插件列表”中显示
        /// </summary>
        bool IsListed
        {
            get;
        }
    }
}
