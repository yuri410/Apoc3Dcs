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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Config;
using Apoc3D.Core;
using Apoc3D.Vfs;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  管理游戏中所有物体类型，可从二进制数据反序列化物体
    /// </summary>
    public class ObjectTypeManager : IDisposable
    {
        #region 字段

        Dictionary<string, SceneObjectFactory> factories;

        #endregion

        public ObjectTypeManager()
        {
            factories = new Dictionary<string, SceneObjectFactory>();
        }

        public void RegisterObjectType(SceneObjectFactory fac)
        {
            factories.Add(fac.TypeTag, fac);
            EngineConsole.Instance.Write("找到物体类型 " + fac.TypeTag, ConsoleMessageType.Information);
        }

        public void UnregisterObjectType(SceneObjectFactory fac)
        {
            factories.Remove(fac.TypeTag);
        }

        public SceneObject DeserializeObject(string typeTag, BinaryDataReader data, SceneDataBase sceneData)
        {
            SceneObjectFactory fac;
            if (factories.TryGetValue(typeTag, out fac))
            {
                return fac.Deserialize(data, sceneData);
            }

            string msg = "无法加载物体：{0}。不支持这种类型。";
            EngineConsole.Instance.Write(string.Format(msg, typeTag), ConsoleMessageType.Error);
            throw new NotSupportedException(typeTag);
        }


        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion
    }
}
