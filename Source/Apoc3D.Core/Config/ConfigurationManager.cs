/*
-----------------------------------------------------------------------------
This source file is part of Apoc3D Engine

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Apoc3D.Vfs;

namespace Apoc3D.Config
{
    /// <summary>
    ///  定义不同配置文件格式的读取器接口
    /// </summary>
    public abstract class ConfigurationFormat
    {
        /// <summary>
        ///  扩展名过滤器
        /// </summary>
        public abstract string[] Filters { get; }

        /// <summary>
        ///  从资源中读取配置
        /// </summary>
        /// <param name="rl">表示资源的位置的<see cref="ResourceLocation"/></param>
        /// <returns>一个<see cref="Configuration"/>，表示创建好的配置</returns>
        public abstract Configuration Load(ResourceLocation rl);

        /// <summary>
        ///  从文件中读取xml配置
        /// </summary>
        /// <param name="rl">表示资源的位置的<see cref="ResourceLocation"/></param>
        /// <returns>一个<see cref="Configuration"/>，表示创建好的配置</returns>
        public Configuration Load(string file)
        {
            return Load(new FileLocation(file));
        }
    }

    /// <summary>
    ///  配置管理器。
    ///  负责从文件创建配置(即<see cref="Configuration"/>)，并管理
    /// </summary>
    public class ConfigurationManager : Singleton
    {
        static ConfigurationManager singleton;

        /// <summary>
        ///  获取唯一的实例
        /// </summary>
        public static ConfigurationManager Instance
        {
            get
            {
                return singleton;
            }
        }

        /// <summary>
        ///  对单件进行显式初始化
        /// </summary>
        public static void Initialize()
        {
            if (singleton == null)
            {
                singleton = new ConfigurationManager();
            }
        }

        Dictionary<string, ConfigurationFormat> formats = 
            new Dictionary<string, ConfigurationFormat>(CaseInsensitiveStringComparer.Instance);

        private ConfigurationManager() { }

        /// <summary>
        ///  注册一个配置文件格式
        /// </summary>
        /// <param name="fmt"></param>
        public void Register(ConfigurationFormat fmt)
        {
            string[] exts = fmt.Filters;
            for (int i = 0; i < exts.Length; i++)
            {
                formats.Add(exts[i], fmt);
            }
        }

        /// <summary>
        ///  从文件创建一个配置(<see cref="Configuration"/>)
        /// </summary>
        /// <param name="fl">表示文件位置的<see cref="FileLocation"/></param>
        /// <returns>一个<see cref="Configuration"/>，表示创建好的配置</returns>
        public Configuration CreateInstance(FileLocation fl)
        {
            string ext = Path.GetExtension(fl.Path);

            ConfigurationFormat fmt;
            if (formats.TryGetValue(ext, out fmt))
            {
                return fmt.Load(fl);
            }
            throw new NotSupportedException(ext);
        }

        /// <summary>
        ///  从文件创建一个配置(<see cref="Configuration"/>)
        /// </summary>
        /// <param name="file">文件的路径</param>
        /// <returns>一个<see cref="Configuration"/>，表示创建好的配置</returns>
        public Configuration CreateInstance(string file)
        {
            string ext = Path.GetExtension(file);

            ConfigurationFormat fmt;
            if (formats.TryGetValue(ext, out fmt))
            {
                return fmt.Load(file);
            }
            throw new NotSupportedException(ext);
        }

        /// <summary>
        ///  以指定的类型创建一个配置
        /// </summary>
        /// <param name="rl">指向配置的<see cref="ResourceLocation"/></param>
        /// <param name="type">指定的类型，该值必须与<see cref="ConfigurationFormat"/>中Filter的值一致，不区分大小写</param>
        /// <returns></returns>
        public Configuration CreateInstance(ResourceLocation rl, string type)
        {           
            ConfigurationFormat fmt;
            if (formats.TryGetValue(type, out fmt))
            {
                return fmt.Load(rl);
            }
            throw new NotSupportedException(type);
        }

        protected override void dispose()
        {
        }
    }
}
