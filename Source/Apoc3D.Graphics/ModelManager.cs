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
using Apoc3D.Core;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    public class ModelManager : ResourceManager
    {
        static ModelManager singleton;

        public static ModelManager Instance
        {
            get
            {
                return singleton;
            }
        }

        public static void Initialize()
        {
            if (singleton == null)
            {
                singleton = new ModelManager(1048576 * 92);
                EngineConsole.Instance.Write("模型管理器初始化完毕。内存使用上限" + Math.Round(singleton.TotalCacheSize / 1048576.0, 2).ToString() + "MB。", ConsoleMessageType.Information);
            }
        }

        public ModelManager() { }
        public ModelManager(int cacheSize)
            : base(cacheSize)
        {
        }
        public ResourceHandle<ModelData> CreateInstance(RenderSystem rs, ResourceLocation rl)
        {
            Resource retrived = base.Exists(rl.Name);
            if (retrived == null)
            {
                ModelData mdl = new ModelData(rs, rl);
                retrived = mdl;
                base.NotifyResourceNew(mdl);
            }
            else
            {
                retrived.Use();
            }
            return new ResourceHandle<ModelData>((ModelData)retrived);
        }

    }
}
