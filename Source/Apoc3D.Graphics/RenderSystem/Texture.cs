﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.Core;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示纹理贴图。可以是1-3维的或者是立方贴图
    /// </summary>
    public abstract class Texture : Resource, IDisposable
    {
        static TextureType[] convTable;

        static Texture()
        {
            convTable = new TextureType[4];
            convTable[(int)ImageType.CubeImage] = TextureType.CubeTexture;
            convTable[(int)ImageType.Image1D] = TextureType.Texture1D;
            convTable[(int)ImageType.Image2D] = TextureType.Texture2D;
            convTable[(int)ImageType.Image3D] = TextureType.Texture3D;
        }

        protected static TextureType ConvertEnum(ImageType type)
        {
            return convTable[(int)type];
        }

        #region 属性

        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        protected ResourceLocation ResourceLocation
        {
            get;
            private set;
        }
        public TextureType Type
        {
            get;
            private set;
        }
        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public int Depth
        {
            get;
            private set;
        }

        //public int BytesPerPixel
        //{
        //    get;
        //    private set;
        //}
        public int ContentSize
        {
            get;
            private set;
        }

        public int SurfaceCount
        {
            get;
            private set;
        }

        public TextureUsage Usage
        {
            get;
            private set;
        }

        public ImagePixelFormat Format
        {
            get;
            private set;
        }

        public bool IsLocked
        {
            get;
            private set;
        }
        #endregion

        #region 构造函数
        protected Texture(RenderSystem rs, ResourceLocation rl)
            : base(TextureManager.Instance, rl.Name)
        {
            this.RenderSystem = rs;
            this.ResourceLocation = rl;
            this.Usage = TextureUsage.Static;
        }
        protected Texture(RenderSystem rs, ResourceLocation rl, TextureUsage usage)
            : base(TextureManager.Instance, rl.Name)
        {
            this.RenderSystem = rs;
            this.ResourceLocation = rl;
            this.Usage = usage;
        }

        protected Texture(RenderSystem rs, int width, int height, int depth, int surfaceCount, ImagePixelFormat format, TextureUsage usage)
        {
            this.RenderSystem = rs;
            this.SurfaceCount = surfaceCount;
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            this.Usage = usage;
            this.Format = format;

            if (depth == 1)
            {
                if (width == 1 || height == 1)
                {
                    this.Type = TextureType.Texture1D;
                }
                else
                {
                    this.Type = TextureType.Texture2D;
                }
            }
            else
            {
                this.Type = TextureType.Texture3D;
            }
            this.ContentSize = PixelFormat.GetMemorySize(width, height, 1, format);
        }

        protected Texture(RenderSystem rs, int length, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            this.RenderSystem = rs;
            this.SurfaceCount = levelCount;
            this.Width = length;
            this.Height = length;
            this.Depth = 1;
            this.Usage = usage;
            this.Format = format;
            this.Type = TextureType.CubeTexture;

            this.ContentSize = 6 * PixelFormat.GetMemorySize(length, length, 1, format);
        }
        #endregion

        protected void UpdateInfo(ref TextureData data)
        {
            this.ContentSize = data.ContentSize;
            this.Depth = data.Depth;
            this.Format = data.Format;
            this.Height = data.Height;
            this.SurfaceCount = data.LevelCount;
            this.Type = data.Type;
            this.Width = data.Width;
        }

        public override int GetSize()
        {
            return ContentSize;
        }

        public abstract void Save(Stream stm);

        #region locks
        /// <summary>
        ///  相应lock的api相关实现
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="mode"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected abstract DataRectangle @lock(int surface, LockMode mode, Rectangle rect);
        /// <summary>
        ///  相应lock的api相关实现
        /// </summary>
        protected abstract DataBox @lock(int surface, LockMode mode, Box box);
        /// <summary>
        ///  相应lock的api相关实现
        /// </summary>
        protected abstract DataRectangle @lock(int surface, CubeMapFace cubemapFace, LockMode mode, Rectangle rect);

        /// <summary>
        ///  锁定立方体纹理
        /// </summary>
        /// <param name="surface">要锁定的纹理层</param>
        /// <param name="mode"></param>
        /// <param name="cubemapFace">要锁定的cubemap表面</param>
        /// <param name="rect">表面上要锁定的区域</param>
        /// <returns>返回一个DataRectangle表示锁定的区域，如果资源没有加载，返回DataRectangle.Empty</returns>
        /// <exception cref="System.InvalidOperationException">如果资源已经锁定，或者纹理不是立方体纹理，则引发这个异常</exception>
        public DataRectangle Lock(int surface, LockMode mode, CubeMapFace cubemapFace, Rectangle rect)
        {
            if (!IsLocked)
            {
                if (Type == TextureType.CubeTexture)
                {
                    if (!IsLocked)
                    {
                        return DataRectangle.Empty;
                    }
                    else
                    {
                        DataRectangle res = @lock(surface, cubemapFace, mode, rect);
                        IsLocked = true;
                        return res;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        ///  锁定立方体纹理
        /// </summary>
        /// <param name="surface">要锁定的纹理层</param>
        /// <param name="mode"></param>
        /// <param name="cubemapFace">要锁定的cubemap表面</param>
        /// <returns>返回一个DataRectangle表示锁定的区域，如果资源没有加载，返回DataRectangle.Empty</returns>
        /// <exception cref="System.InvalidOperationException">如果资源已经锁定，或者纹理不是立方体纹理，则引发这个异常</exception>
        public DataRectangle Lock(int surface, LockMode mode, CubeMapFace cubemapFace)
        {
            if (!IsLocked)
            {
                if (Type == TextureType.CubeTexture)
                {
                    if (!IsLocked)
                    {
                        return DataRectangle.Empty;
                    }
                    else
                    {
                        DataRectangle res = @lock(surface, cubemapFace, mode, new Rectangle(0, 0, Width, Width));
                        IsLocked = true;
                        return res;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        ///  锁定2D纹理
        /// </summary>
        /// <param name="surface">要锁定的纹理层</param>
        /// <param name="mode"></param>
        /// <param name="rect">要锁定的区域</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">如果资源已经锁定，或者纹理不是2D纹理，则引发这个异常</exception>
        public DataRectangle Lock(int surface, LockMode mode, Rectangle rect)
        {
            if (!IsLocked)
            {
                if (Type == TextureType.Texture2D || Type == TextureType.Texture1D)
                {
                    if (!IsLoaded)
                    {
                        return DataRectangle.Empty;
                    }
                    else
                    {
                        DataRectangle res = @lock(surface, mode, rect);
                        IsLocked = true;
                        return res;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        ///  锁定2D纹理
        /// </summary>
        /// <param name="surface">要锁定的纹理层</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">如果资源已经锁定，或者纹理不是2D纹理，则引发这个异常</exception>
        public DataRectangle Lock(int surface, LockMode mode)
        {
            if (!IsLocked)
            {
                if (Type == TextureType.Texture2D || Type == TextureType.Texture1D)
                {
                    if (!IsLoaded)
                    {
                        return DataRectangle.Empty;
                    }
                    else
                    {
                        DataRectangle res = @lock(surface, mode, new Rectangle(0, 0, Width, Height));
                        IsLocked = true;
                        return res;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        ///  锁定3D纹理
        /// </summary>
        /// <param name="surface">要锁定的纹理层</param>
        /// <param name="mode"></param>
        /// <param name="box">要锁定的空间区域</param>
        /// <returns>返回一个DataBox表示锁定的区域，如果资源没有加载，返回DataBox.Empty</returns>
        /// <exception cref="System.InvalidOperationException">如果资源已经锁定，或者纹理不是3D纹理，则引发这个异常</exception>
        public DataBox Lock(int surface, LockMode mode, Box box)
        {
            if (!IsLocked)
            {
                if (Type == TextureType.Texture3D)
                {
                    if (!IsLoaded)
                    {
                        return DataBox.Empty;
                    }
                    else
                    {
                        DataBox res = @lock(surface, mode, box);
                        IsLocked = true;
                        return res;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        ///  相应unlock的api相关实现
        /// </summary>
        protected abstract void unlock(int surface);
        /// <summary>
        ///  相应unlock的api相关实现
        /// </summary>
        protected abstract void unlock(CubeMapFace cubemapFace, int surface);

        public void Unlock(int surface)
        {
            if (IsLocked)
            {
                unlock(surface);
                IsLocked = false;
            }
            throw new InvalidOperationException();
        }
        public void Unlock(CubeMapFace cubemapFace, int surface)
        {
            if (IsLocked)
            {
                unlock(cubemapFace, surface);
                IsLocked = false;
            }
            throw new InvalidOperationException();
        }

        #endregion
    }
}
