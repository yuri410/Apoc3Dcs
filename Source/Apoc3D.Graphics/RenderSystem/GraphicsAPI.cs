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
using Apoc3D.Collections;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  图形API管理器，包含所有支持的API。
    /// </summary>
    public class GraphicsAPIManager : Singleton
    {
        static volatile GraphicsAPIManager singleton;
        static object syncHelper = new object();

        public static GraphicsAPIManager Instance
        {
            get
            {
                if (singleton == null)
                {
                    lock (syncHelper)
                    {
                        if (singleton == null)
                        {
                            singleton = new GraphicsAPIManager();
                        }
                    }
                }
                return singleton;
            }
        }

        class Entry
        {
            GraphicsAPIFactory factory;
            int osMark;

            public Entry(GraphicsAPIFactory fac, int mark)
            {
                this.factory = fac;
                this.osMark = mark;
            }

            public GraphicsAPIFactory Factory
            {
                get { return factory; }
            }

            public int OSMark
            {
                get { return osMark; }
            }
        }

        Dictionary<string, FastList<Entry>> factories;
        ExistTable<string> registered;


        private GraphicsAPIManager()
        {
            factories = new Dictionary<string, FastList<Entry>>();
            registered = new ExistTable<string>();
        }

        public void RegisterGraphicsAPI(GraphicsAPIFactory fac)
        {
            if (registered.Exists(fac.Name))
            {
                throw new InvalidOperationException();
            }

            FastList<Entry> facList;

            PlatformCollection plats = fac.Description.SupportedPlatforms;

            for (int i = 0; i < plats.Count; i++)
            {
                if (!factories.TryGetValue(plats[i].PlatformName, out facList))
                {
                    facList = new FastList<Entry>();
                    factories.Add(plats[i].PlatformName, facList);
                }
                facList.Add(new Entry(fac, plats[i].Mark));
            }

            registered.Add(fac.Name);
        }
        public void UnregisterGraphicsAPI(string name)
        {
            if (!registered.Exists(name))
            {
                throw new InvalidOperationException();
            }

            Dictionary<string, FastList<Entry>>.ValueCollection vals = factories.Values;
            foreach (FastList<Entry> e in vals)
            {
                for (int i = 0; i < e.Count; i++)
                {
                    if (e[i].Factory.Name == name)
                    {
                        e.RemoveAt(i);
                        break;
                    }
                }
            }

            registered.Remove(name);
        }
        public void UnregisterGraphicsAPI(GraphicsAPIFactory fac)
        {
            if (!registered.Exists(fac.Name))
            {
                throw new InvalidOperationException();
            }

            FastList<Entry> facList;

            PlatformCollection plats = fac.Description.SupportedPlatforms;
            for (int i = 0; i < plats.Count; i++)
            {
                if (factories.TryGetValue(plats[i].PlatformName, out facList))
                {
                    for (int j = 0; j < facList.Count; j++)
                    {
                        if (facList[j].Factory == fac)
                        {
                            facList.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            registered.Remove(fac.Name);
        }

        int Comparison(Entry a, Entry b)
        {
            return a.OSMark.CompareTo(b.OSMark);
        }

        /// <summary>
        ///  创建最适合于当前系统的API的DeviceContent
        /// </summary>
        /// <returns></returns>
        public DeviceContent CreateDeviceContent()
        {
            string osName = string.Empty;

            OperatingSystem osInfo = Environment.OSVersion;

            switch (osInfo.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                case PlatformID.Win32S:
                    osName = "Windows";
                    break;
                case PlatformID.WinCE:
                    osName = "WinCE";
                    break;
                case PlatformID.Unix:
                    osName = "Linux";
                    break;
                case PlatformID.Xbox:
                    osName = "XBox";
                    break;
                default:
                    osName = "Mac OS X";
                    break;
            }

            FastList<Entry> facList;
            if (factories.TryGetValue(osName, out facList))
            {
                if (facList.Count > 0)
                {
                    facList.Sort(Comparison);
                    return facList[facList.Count - 1].Factory.CreateDeviceContent();
                }
                throw new NotSupportedException(osName);
            }
            throw new NotSupportedException(osName);
        }

        protected override void dispose()
        {
            factories.Clear();
            factories = null;
        }
    }

    /// <summary>
    ///  表示一种图形API的信息
    /// </summary>
    public struct GraphicsAPIDescription
    {
        string name;

        PlatformCollection supported;

        public GraphicsAPIDescription(string name, PlatformCollection supportedPlatforms)
        {
            this.name = name;
            this.supported = supportedPlatforms;
        }

        /// <summary>
        ///  获取这个图形API的名称
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        ///  获取一个集合，表示这个API支持的所有平台
        /// </summary>
        public PlatformCollection SupportedPlatforms
        {
            get { return supported; }
        }
    }

    /// <summary>
    ///  创建图形API的DeviceContent的抽象工厂
    /// </summary>
    public abstract class GraphicsAPIFactory
    {
        GraphicsAPIDescription description;

        protected GraphicsAPIFactory(GraphicsAPIDescription desc)
        {
            this.description = desc;
        }

        public GraphicsAPIDescription Description
        {
            get { return description; }
        }

        public string Name
        {
            get { return description.Name; }
        }

        /// <summary>
        ///  创建相应的API的DeviceContent
        /// </summary>
        /// <returns></returns>
        public abstract DeviceContent CreateDeviceContent();
    }
}
