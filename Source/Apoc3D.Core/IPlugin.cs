using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Apoc3D
{
    public interface IPlugin
    {
        /// <summary>
        /// 
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        void Load();

        /// <summary>
        /// 
        /// </summary>
        void Unload();
    }
}
