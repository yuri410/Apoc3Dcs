using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Apoc3D.Ide
{
    public interface IPlugin : Apoc3D.Core.IPlugin
    {


        Icon PluginIcon
        {
            get;
        }

        bool IsListed
        {
            get;
        }
    }
}
