using System;
using System.Collections.Generic;
using System.Text;

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
