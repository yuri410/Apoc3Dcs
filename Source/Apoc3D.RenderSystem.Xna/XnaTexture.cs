using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaTexture : Texture
    {
        XG.Texture2D tex2D;
        XG.Texture3D tex3D;
        XG.TextureCube cube;

        #region 内部构造函数
        internal XnaTexture(XnaRenderSystem rs, XG.Texture2D tex2d)
            : base(rs, tex2d.Width, tex2d.Height, 1, tex2d.LevelCount, 
                   XnaUtils.ConvertEnum(tex2d.Format), XnaUtils.ConvertEnum(tex2d.TextureUsage))
        {
            this.tex2D = tex2d;
        }
        internal XnaTexture(XnaRenderSystem rs, XG.Texture3D tex3d)
            : base(rs, tex3d.Width, tex3d.Height, tex3d.Depth, tex3d.LevelCount,
                   XnaUtils.ConvertEnum(tex3d.Format), XnaUtils.ConvertEnum(tex3d.TextureUsage))
        {
            this.tex3D = tex3d;
        }
        internal XnaTexture(XnaRenderSystem rs, XG.TextureCube texCube)
            : base(rs, texCube.Size, texCube.LevelCount,
                   XnaUtils.ConvertEnum(texCube.TextureUsage), XnaUtils.ConvertEnum(texCube.Format))
        {
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
            cube = new XG.TextureCube(rs.device, length, level, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(format));
        }

        public override void Save(Stream stm)
        {
            throw new NotImplementedException();
        }

        protected override DataRectangle @lock(int surface, LockMode mode, Rectangle rect)
        {
            if (Type == TextureType.Texture2D || Type == TextureType.Texture1D)
            {
                throw new NotImplementedException();
            }
            else 
            {
                throw new InvalidOperationException();
            }
        }

        protected override DataBox @lock(int surface, LockMode mode, Box box)
        {
            if (Type == TextureType.Texture3D)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }
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
            throw new NotImplementedException();
        }

        protected override void unload()
        {
            throw new NotImplementedException();
        }
    }
}