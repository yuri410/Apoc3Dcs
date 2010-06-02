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
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    public class TextureManager : ResourceManager
    {
        static volatile TextureManager singleton;
        static volatile object syncHelper = new object();

        public static void Initialize(int cacheSize)
        {
            singleton = new TextureManager(cacheSize);
        }

        public static TextureManager Instance
        {
            get
            {
                return singleton;
            }
        }

        private TextureManager(int cbSize)
            : base(cbSize)
        {
            CreationUsage = TextureUsage.Static;
        }

        public FileLocation Redirect
        {
            get;
            set;
        }
        public ObjectFactory Factory
        {
            get;
            set;
        }
        public TextureUsage CreationUsage
        {
            get;
            set;
        }

        /// <remarks>以这种方式创建的资源不会被管理</remarks>
        public Texture CreateInstance(int width, int height, int surfaceCount, ImagePixelFormat format)
        {
            if (Factory == null)
            {
                throw new InvalidOperationException();
            }

            return Factory.CreateTexture(width, height, surfaceCount, CreationUsage, format);
        }
        /// <remarks>以这种方式创建的资源不会被管理</remarks>
        public Texture CreateInstance(int width, int height, int depth, int surfaceCount, ImagePixelFormat format)
        {
            if (Factory == null)
            {
                throw new InvalidOperationException();
            }

            return Factory.CreateTexture(width, height, depth, surfaceCount, CreationUsage, format);
        }
        /// <remarks>以这种方式创建的资源不会被管理</remarks>
        public Texture CreateInstance(int length, int surfaceCount, ImagePixelFormat format)
        {
            if (Factory == null)
            {
                throw new InvalidOperationException();
            }
            return Factory.CreateTexture(length, surfaceCount, CreationUsage, format);
        }
        public Texture CreateInstanceUnmanaged(FileLocation fl)
        {
            if (Factory == null)
            {
                throw new InvalidOperationException();
            }
            if (Redirect != null)
                fl = Redirect;
            return Factory.CreateTexture(fl, CreationUsage, false);
        }

        public ResourceHandle<Texture> CreateInstance(FileLocation fl)
        {
            if (Factory == null)
            {
                throw new InvalidOperationException();
            }
            if (Redirect != null)
                fl = Redirect;

            Resource retrived = base.Exists(fl.Name);
            if (retrived == null)
            {
                Texture tex = Factory.CreateTexture(fl, CreationUsage, true);
                retrived = tex;
                base.NotifyResourceNew(tex);
            }

            retrived.Use();

            return new ResourceHandle<Texture>((Texture)retrived);
        }

    }
}
