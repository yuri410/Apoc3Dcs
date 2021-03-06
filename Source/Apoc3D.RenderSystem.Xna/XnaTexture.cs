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
using System.IO;
using System.Text;
using Apoc3D.Core;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    unsafe class XnaTexture : Texture
    {
        internal XG.Texture2D tex2D;
        internal XG.Texture3D tex3D;
        internal XG.TextureCube cube;
        XnaRenderSystem renderSys;

        struct LockInfo 
        {
            public X.Rectangle lockRect;
            public Box lockBox;
            public LockMode curLockMode;
            public byte[] buffer;
            public int lockSize;
        }

        LockInfo[] lockInfo;

        #region 内部构造函数
        internal XnaTexture(XnaRenderSystem rs, XG.Texture2D tex2d)
            : base(rs, tex2d.Width, tex2d.Height, 1, tex2d.LevelCount,
                   XnaUtils.ConvertEnum(tex2d.Format), XnaUtils.ConvertEnum(tex2d.TextureUsage))
        {
            this.renderSys = rs;
            this.tex2D = tex2d;
            this.lockInfo = new LockInfo[tex2d.LevelCount];
        }
        internal XnaTexture(XnaRenderSystem rs, XG.Texture3D tex3d)
            : base(rs, tex3d.Width, tex3d.Height, tex3d.Depth, tex3d.LevelCount,
                   XnaUtils.ConvertEnum(tex3d.Format), XnaUtils.ConvertEnum(tex3d.TextureUsage))
        {
            this.renderSys = rs;
            this.tex3D = tex3d;
            this.lockInfo = new LockInfo[tex3d.LevelCount];
        }
        internal XnaTexture(XnaRenderSystem rs, XG.TextureCube texCube)
            : base(rs, texCube.Size, texCube.LevelCount,
                   XnaUtils.ConvertEnum(texCube.TextureUsage), XnaUtils.ConvertEnum(texCube.Format))
        {
            this.renderSys = rs;
            this.cube = texCube;
            this.lockInfo = new LockInfo[texCube.LevelCount];
        }
        #endregion

        /// <summary>
        ///  读取资源创建一个纹理
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="rl"></param>
        /// <param name="usage"></param>
        public XnaTexture(XnaRenderSystem rs, ResourceLocation rl, TextureUsage usage, bool managed)
            : base(rs, rl, usage, managed)
        {
            this.renderSys = rs;
            if (!managed)
            {
                load();
            }
        }

        /// <summary>
        ///  创建一个空白的纹理
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="level"></param>
        /// <param name="format"></param>
        /// <param name="usage"></param>
        public XnaTexture(XnaRenderSystem rs, int width, int height, int depth, int level, ImagePixelFormat format, TextureUsage usage)
            : base(rs, width, height, depth, level, format, usage)
        {
            this.renderSys = rs;

            if (Type == TextureType.Texture2D || Type == TextureType.Texture1D)
            {
                this.tex2D = new XG.Texture2D(rs.Device, width, height, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
            }
            else if (Type == TextureType.Texture3D)
            {
                this.tex3D = new XG.Texture3D(rs.Device, width, height, depth, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
            }
            else
            {
                throw new NotSupportedException("不可能出现这种情况");
            }
            this.lockInfo = new LockInfo[level];
        }

        public XnaTexture(XnaRenderSystem rs, int length, int level, ImagePixelFormat format, TextureUsage usage)
            : base(rs, length, level, usage, format)
        {
            this.renderSys = rs;

            this.cube = new XG.TextureCube(rs.Device, length, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
            this.lockInfo = new LockInfo[level];
        }

        public override void Save(Stream stm)
        {
            throw new NotSupportedException();
            //TextureData data = new TextureData();
            //data.ContentSize = ContentSize;
            ////data.Depth = Depth;
            //data.Format = Format;
            ////data.Height = Height;
            //data.LevelCount = SurfaceCount;
            //data.Type = Type;
            ////data.Width = Width;

            //data.Levels = new TextureLevelData[SurfaceCount];
            //for (int i = 0; i < SurfaceCount; i++)
            //{
            //    switch (Type)
            //    {
            //        case TextureType.CubeTexture:
            //            int faceSize = PixelFormat.GetMemorySize(Width, Width, 1, Format);

            //            cube.GetData<byte>(XG.CubeMapFace.NegativeX, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;
            //            cube.GetData<byte>(XG.CubeMapFace.NegativeY, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;
            //            cube.GetData<byte>(XG.CubeMapFace.NegativeZ, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;
            //            cube.GetData<byte>(XG.CubeMapFace.PositiveX, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;
            //            cube.GetData<byte>(XG.CubeMapFace.PositiveY, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;
            //            cube.GetData<byte>(XG.CubeMapFace.PositiveZ, i, null, buffer, startPos, faceSize);
            //            startPos += faceSize;

            //            data.Levels[i].LevelSize = faceSize * 6;
            //            break;
            //        case TextureType.Texture1D:
            //        case TextureType.Texture2D:
            //            int lvlSize = PixelFormat.GetMemorySize(Width, Width, 1, Format);


            //            break;
            //        case TextureType.Texture3D:

            //            break;
            //    }

                
            //}

            
            //data.Save(stm);
        }

        public override void SetData(byte[] data)
        {
            if (tex2D != null)
                tex2D.SetData<byte>(data);
        }

        protected override DataRectangle @lock(int surface, LockMode mode, Rectangle rect)
        {
            LockInfo info = new LockInfo();

            info.lockSize = PixelFormat.GetMemorySize(rect.Width, rect.Height, 1, Format);
            info.buffer = new byte[info.lockSize];
            info.lockRect = new X.Rectangle(rect.X, rect.Y, rect.Width, rect.Height); ;
            info.curLockMode = mode;

            if (Usage != TextureUsage.WriteOnly)
            {
                tex2D.GetData<byte>(surface, info.lockRect, info.buffer, 0, info.lockSize);
            }
            void* result;
            fixed (byte* src = &info.buffer[0])
            {
                result = src;
            }
            lockInfo[surface] = info;
            return new DataRectangle(PixelFormat.GetMemorySize(rect.Width, 1, 1, Format), 
                new IntPtr(result), rect.Width, rect.Height, Format);
        }

        protected override DataBox @lock(int surface, LockMode mode, Box box)
        {
            LockInfo info = new LockInfo(); 
            info.lockSize = PixelFormat.GetMemorySize(box.Width, box.Height, box.Depth, Format);
            info.buffer = new byte[info.lockSize];
            info.lockBox = box;
            info.curLockMode = mode;

            if (Usage != TextureUsage.WriteOnly)
            {
                tex3D.GetData<byte>(surface,
                    box.Left, box.Top, box.Right, box.Bottom, box.Front, box.Back, info.buffer, 0, info.lockSize);
            }
            void* result;
            fixed (byte* src = &info.buffer[0])
            {
                result = src;
            }

            int rowPitch = PixelFormat.GetMemorySize(box.Width, 1, 1, Format);
            int slicePitch = PixelFormat.GetMemorySize(box.Width, box.Height, 1, Format);

            lockInfo[surface] = info;
            return new DataBox(rowPitch, slicePitch,
                new IntPtr(result), box.Width, box.Height, box.Depth, Format);
        }

        protected override DataRectangle @lock(int surface, CubeMapFace cubemapFace, LockMode mode, Rectangle rect)
        {
            LockInfo info = new LockInfo();
            info.lockSize = PixelFormat.GetMemorySize(rect.Width, rect.Height, 1, Format);
            info.buffer = new byte[info.lockSize];
            info.lockRect = new X.Rectangle(rect.X, rect.Y, rect.Width, rect.Height); ;
            info.curLockMode = mode;

            if (Usage != TextureUsage.WriteOnly)
            {
                cube.GetData<byte>(XnaUtils.ConvertEnum(cubemapFace),
                    surface, info.lockRect, info.buffer, 0, info.lockSize);
            }
            void* result;
            fixed (byte* src = &info.buffer[0])
            {
                result = src;
            }

            lockInfo[surface] = info; 
            return new DataRectangle(PixelFormat.GetMemorySize(rect.Width, 1, 1, Format),
                new IntPtr(result), rect.Width, rect.Height, Format);
        }

        protected override void unlock(int surface)
        {
            LockInfo info = lockInfo[surface];
            switch (Type) 
            {
                case TextureType.Texture1D:
                case TextureType.Texture2D:
                    switch (info.curLockMode)
                    {
                        case LockMode.ReadOnly:
                            // 不管
                            break;
                        case LockMode.NoOverwrite:
                            tex2D.SetData<byte>(surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.NoOverwrite);
                            break;
                        case LockMode.None:
                            tex2D.SetData<byte>(surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.None);
                            break;
                        case LockMode.Discard:
                            tex2D.SetData<byte>(surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.Discard);
                            break;
                    }
                    break;
                case TextureType.Texture3D:
                    switch (info.curLockMode)
                    {
                        case LockMode.ReadOnly:
                            // 不管
                            break;
                        case LockMode.NoOverwrite:
                            tex3D.SetData<byte>(surface,
                                info.lockBox.Left, info.lockBox.Top, info.lockBox.Right, 
                                info.lockBox.Bottom, info.lockBox.Front, info.lockBox.Back,
                                info.buffer, 0, info.lockSize, XG.SetDataOptions.NoOverwrite);
                            break;
                        case LockMode.None:
                            tex3D.SetData<byte>(surface,
                               info.lockBox.Left, info.lockBox.Top, info.lockBox.Right, 
                               info.lockBox.Bottom, info.lockBox.Front, info.lockBox.Back,
                               info.buffer, 0, info.lockSize, XG.SetDataOptions.None);
                            break;
                        case LockMode.Discard:
                            tex3D.SetData<byte>(surface,
                                info.lockBox.Left, info.lockBox.Top, info.lockBox.Right, 
                                info.lockBox.Bottom, info.lockBox.Front, info.lockBox.Back,
                              info.buffer, 0, info.lockSize, XG.SetDataOptions.Discard);
                            break;
                    }
                    break;
            }

            info.buffer = null;
        }
        protected override void unlock(CubeMapFace cubemapFace, int surface)
        {
            LockInfo info = lockInfo[surface];

            switch (info.curLockMode)
            {
                case LockMode.ReadOnly:
                    // 不管
                    break;
                case LockMode.NoOverwrite:
                    cube.SetData<byte>(XnaUtils.ConvertEnum(cubemapFace),
                        surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.NoOverwrite);
                    break;
                case LockMode.None:
                    cube.SetData<byte>(XnaUtils.ConvertEnum(cubemapFace),
                        surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.None);
                    break;
                case LockMode.Discard:
                    cube.SetData<byte>(XnaUtils.ConvertEnum(cubemapFace),
                        surface, info.lockRect, info.buffer, 0, info.lockSize, XG.SetDataOptions.Discard);
                    break;
            }

            info.buffer = null;
        }

        protected override void load()
        {
            TextureData data = new TextureData();
            data.Load(ResourceLocation);

            lockInfo = new LockInfo[data.LevelCount];
            UpdateInfo(ref data);

            switch (data.Type)
            {
                case TextureType.Texture1D:
                case TextureType.Texture2D:
                    tex2D = new XG.Texture2D(renderSys.Device, Width, Height, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));

                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        tex2D.SetData(i, null, data.Levels[i].Content, 0, data.Levels[i].LevelSize, XG.SetDataOptions.None);                       
                    }


                    //FileLocation fl = ResourceLocation as FileLocation;
                    //if (fl != null)
                    //{
                    //    tex2D.Save(Path.Combine(@"E:\Desktop\dump", Path.GetFileNameWithoutExtension(fl.Path) + ".png"), Microsoft.Xna.Framework.Graphics.ImageFileFormat.Png);
                    //}
                    break;
                case TextureType.CubeTexture:

                    cube = new XG.TextureCube(renderSys.Device, Width, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));


                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        int startPos = 0;
                        int levelSize = data.Levels[i].LevelSize / 6;
                        cube.SetData(XG.CubeMapFace.PositiveX, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;

                        cube.SetData(XG.CubeMapFace.NegativeX, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;

                        cube.SetData(XG.CubeMapFace.PositiveY, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;

                        cube.SetData(XG.CubeMapFace.NegativeY, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;

                        cube.SetData(XG.CubeMapFace.PositiveZ, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;

                        cube.SetData(XG.CubeMapFace.NegativeZ, i, null, data.Levels[i].Content, startPos, levelSize, XG.SetDataOptions.None);
                        startPos += levelSize;
                    }

                    //FileLocation fl2 = ResourceLocation as FileLocation;
                    //if (fl2 != null)
                    //{
                    //    cube.Save(Path.Combine(@"E:\Desktop\dump", Path.GetFileNameWithoutExtension(fl2.Path) + ".dds"), Microsoft.Xna.Framework.Graphics.ImageFileFormat.Dds);
                    //}
                    //cube.Save(@"E:\Desktop\sss.dds", Microsoft.Xna.Framework.Graphics.ImageFileFormat.Dds);
                    break;
                case TextureType.Texture3D:
                    tex3D = new XG.Texture3D(renderSys.Device, Width, Height, Depth, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));

                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        tex3D.SetData(i, 0, 0, 0, 0, 0, 0, data.Levels[i].Content, 0, data.Levels[i].LevelSize, XG.SetDataOptions.None);
                    }
                    break;
            }
        }

        protected override void unload()
        {
            if (tex2D != null)
            {
                //ResourceInterlock.EnterAtomicOp();
                tex2D.Dispose();
                //ResourceInterlock.ExitAtomicOp();
                tex2D = null;
            }
            if (cube != null)
            {
                //ResourceInterlock.EnterAtomicOp();
                cube.Dispose();
                //ResourceInterlock.ExitAtomicOp();
                cube = null;
            }
            if (tex3D != null)
            {
                //ResourceInterlock.EnterAtomicOp();
                tex3D.Dispose();
                //ResourceInterlock.ExitAtomicOp();
                tex3D = null;
            }

        }
    }
}