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

        XG.GraphicsDevice device;

        #region 内部构造函数
        internal XnaTexture(XnaRenderSystem rs, XG.Texture2D tex2d)
            : base(rs, tex2d.Width, tex2d.Height, 1, tex2d.LevelCount, 
                   XnaUtils.ConvertEnum(tex2d.Format), XnaUtils.ConvertEnum(tex2d.TextureUsage))
        {
            this.device = rs.device;
            this.tex2D = tex2d;
        }
        internal XnaTexture(XnaRenderSystem rs, XG.Texture3D tex3d)
            : base(rs, tex3d.Width, tex3d.Height, tex3d.Depth, tex3d.LevelCount,
                   XnaUtils.ConvertEnum(tex3d.Format), XnaUtils.ConvertEnum(tex3d.TextureUsage))
        {
            this.device = rs.device;
            this.tex3D = tex3d;
        }
        internal XnaTexture(XnaRenderSystem rs, XG.TextureCube texCube)
            : base(rs, texCube.Size, texCube.LevelCount,
                   XnaUtils.ConvertEnum(texCube.TextureUsage), XnaUtils.ConvertEnum(texCube.Format))
        {
            this.device = rs.device;
            this.cube = texCube;
        }
        #endregion

        /// <summary>
        ///  读取资源创建一个纹理
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="rl"></param>
        /// <param name="usage"></param>
        public XnaTexture(XnaRenderSystem rs, ResourceLocation rl, TextureUsage usage)
            : base(rs, rl, usage)
        {
            this.device = rs.device;

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
            this.device = rs.device;

            if (Type == TextureType.Texture2D || Type == TextureType.Texture1D)
            {
                tex2D = new XG.Texture2D(rs.device, width, height, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
            }
            else if (Type == TextureType.Texture3D) 
            {
                tex3D = new XG.Texture3D(rs.device, width, height, depth, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
            }
            else
            {
                throw new NotSupportedException("不可能出现这种情况");
            }            
        }

        public XnaTexture(XnaRenderSystem rs, int length, int level, ImagePixelFormat format, TextureUsage usage)
            : base(rs, length, level, usage, format)
        {
            this.device = rs.device;

            cube = new XG.TextureCube(rs.device, length, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
        }

        public override void Save(Stream stm)
        {
            throw new NotImplementedException();
        }

        protected override DataRectangle @lock(int surface, LockMode mode, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        protected override DataBox @lock(int surface, LockMode mode, Box box)
        {
            throw new NotImplementedException();
        }

        protected override DataRectangle @lock(int surface, CubeMapFace cubemapFace, LockMode mode, Rectangle rect)
        {
            if (Type == TextureType.CubeTexture)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override void unlock(int surface)
        {
            if (Type != TextureType.CubeTexture)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override void unlock(CubeMapFace cubemapFace, int surface)
        {
            if (Type == TextureType.CubeTexture)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override void load()
        {
            TextureData data = new TextureData();
            data.Load(ResourceLocation);

            UpdateInfo(ref data);

            switch (data.Type) 
            {
                case TextureType.Texture1D:
                case TextureType.Texture2D:
                    ResourceInterlock.EnterAtomicOp();
                    tex2D = new XG.Texture2D(device, Width, Height, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));
                    ResourceInterlock.ExitAtomicOp();

                    int startPos = 0;
                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        ResourceInterlock.EnterAtomicOp();
                        tex2D.SetData(i, null, data.Content, startPos, data.LevelSize[i], XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();

                        startPos += data.LevelSize[i];
                    }
                    break;
                case TextureType.CubeTexture:
                    ResourceInterlock.EnterAtomicOp();
                    cube = new XG.TextureCube(device, Width, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));
                    ResourceInterlock.ExitAtomicOp();

                    startPos = 0;
                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        int levelSize = data.LevelSize[i] / 6;
                        ResourceInterlock.EnterAtomicOp();
                        cube.SetData(XG.CubeMapFace.NegativeX, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();
                        startPos += levelSize;

                        ResourceInterlock.EnterAtomicOp();
                        cube.SetData(XG.CubeMapFace.NegativeY, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();
                        startPos += levelSize;

                        ResourceInterlock.EnterAtomicOp();
                        cube.SetData(XG.CubeMapFace.NegativeZ, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();
                        startPos += levelSize;

                        ResourceInterlock.EnterAtomicOp();
                        cube.SetData(XG.CubeMapFace.PositiveX, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.EnterAtomicOp();
                        startPos += levelSize;

                        ResourceInterlock.ExitAtomicOp();
                        cube.SetData(XG.CubeMapFace.PositiveY, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();
                        startPos += levelSize;

                        ResourceInterlock.EnterAtomicOp();
                        cube.SetData(XG.CubeMapFace.PositiveZ, i, null, data.Content, startPos, levelSize, XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();
                        startPos += levelSize;
                    } 
                    break;
                case TextureType.Texture3D:
                    ResourceInterlock.EnterAtomicOp();
                    tex3D = new XG.Texture3D(device, Width, Height, Depth, SurfaceCount, XnaUtils.ConvertEnum(Usage), XnaUtils.ConvertEnum(Format));
                    ResourceInterlock.ExitAtomicOp();

                    startPos = 0;
                    for (int i = 0; i < SurfaceCount; i++)
                    {
                        ResourceInterlock.EnterAtomicOp();
                        tex3D.SetData(i, 0, 0, 0, 0, 0, 0, data.Content, startPos, data.LevelSize[i], XG.SetDataOptions.None);
                        ResourceInterlock.ExitAtomicOp();

                        startPos += data.LevelSize[i];
                    }
                    break;
            }
        }

        protected override void unload()
        {
            if (tex2D != null)
            {
                ResourceInterlock.EnterAtomicOp();
                tex2D.Dispose();
                ResourceInterlock.ExitAtomicOp();
                tex2D = null;
            }
            if (cube != null)
            {
                ResourceInterlock.EnterAtomicOp();
                cube.Dispose();
                ResourceInterlock.ExitAtomicOp();
                cube = null;
            }
            if (tex3D != null)
            {
                ResourceInterlock.EnterAtomicOp();
                tex3D.Dispose();
                ResourceInterlock.ExitAtomicOp();
                tex3D = null;
            }
            
        }
    }
}