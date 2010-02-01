﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    public enum FontStyle
    {
        Normal,
        Bold,
        Italic,
    }

    /// <summary>
    /// Specifies formatting options for text rendering. 
    /// </summary>
    [Flags]
    public enum DrawTextFormat
    {
        /// <summary>
        /// Align the text to the bottom. 
        /// </summary>
        Bottom = 8,
        /// <summary>
        /// Align the text to the center. 
        /// </summary>
        Center = 1,
        /// <summary>
        /// Expand tab characters.
        /// </summary>
        ExpandTabs = 64,
        /// <summary>
        /// Align the text to the left.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Don't clip the text.
        /// </summary>
        NoClip = 256,
        /// <summary>
        /// Align the text to the right.
        /// </summary>
        Right = 2,
        /// <summary>
        /// Rendering the text in right-to-left reading order.
        /// </summary>
        RtlReading = 131072,
        /// <summary>
        /// Force all text to a single line.
        /// </summary>
        SingleLine = 32,
        /// <summary>
        /// Align the text to the top. 
        /// </summary>
        Top = 0,
        /// <summary>
        /// Vertically align the text to the center.
        /// </summary>
        VerticalCenter = 4,
        /// <summary>
        /// Allow word breaks.
        /// </summary>
        WordBreak = 16
    }

    public abstract unsafe class FontBase<T>
        where T : class
    {
        public const int Id = 'S' << 24 | 'F' << 16 | 'N' << 8 | 'T';

        protected struct BitmapChar
        {
            public char Char;
            public T Map;
            public int Width;
        }

        protected int codeStart;
        protected int codeEnd;

        protected int origWidth;
        protected int origHeight;

        protected BitmapChar[] chars;

        public int CodeStart
        {
            get { return codeStart; }
        }
        public int CodeEnd
        {
            get { return codeEnd; }
        }
        public int OriginalWidth
        {
            get { return origWidth; }
        }
        public int OriginalHeight
        {
            get { return origHeight; }
        }

        protected void ReadData(ResourceLocation rl)
        {
            ContentBinaryReader br = new ContentBinaryReader(rl);
            if (br.ReadInt32() == Id)
            {
                codeStart = br.ReadInt32();
                codeEnd = br.ReadInt32();

                origWidth = br.ReadInt32();
                origHeight = br.ReadInt32();

                chars = new BitmapChar[codeEnd - codeStart];

                for (int s = 0; s < chars.Length; s++)
                {
                    char ch = br.ReadChar();
                    int charWidth = br.ReadInt32();

                    T tex = LoadCharMap(br);

                    chars[s].Char = ch;
                    chars[s].Width = charWidth;
                    chars[s].Map = tex;
                }
            }
            else
            {
                throw new InvalidFormatException();
            }
        }
        protected void WriteData()
        {
            throw new NotSupportedException();
        }

        protected abstract T LoadCharMap(ContentBinaryReader br);
        protected abstract void SaveCharMap(ContentBinaryWriter bw, T tex);
    }

    public unsafe class Font : FontBase<Texture>, IDisposable
    {
        string fontName;
        ObjectFactory factory;

        public string FontName
        {
            get { return fontName; }
        }

        public static Font FromResource(RenderSystem rs, ResourceLocation rl, string name)
        {
            Font fnt = new Font(rs, name);
            fnt.ReadData(rl);
            return fnt;
        }

        private Font(RenderSystem rs, string name)
        {
            this.fontName = name;

            this.factory = rs.ObjectFactory;
        }

        protected override Texture LoadCharMap(ContentBinaryReader br)
        {
            Texture tex = factory.CreateTexture(origWidth, origHeight, 1, TextureUsage.StaticWriteOnly, ImagePixelFormat.Luminance8);

            DataRectangle rect = tex.Lock(0, LockMode.None);

            byte* dst = (byte*)rect.Pointer;

            for (int i = 0; i < origHeight; i++)
            {
                for (int j = 0; j < origWidth; j++)
                {
                    *dst++ = br.ReadByte();
                }
                dst += rect.Pitch - origWidth;
            }
            tex.Unlock(0);
            return tex;
        }
        protected override void SaveCharMap(ContentBinaryWriter bw, Texture tex)
        {
            throw new NotSupportedException();
        }

        public void DrawString(SceneSprite sprite, string text, int x, int y, float fontSize, DrawTextFormat format, int color)
        {
            float scale = fontSize / origHeight;
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch != '\n')
                {
                    if (ch >= codeStart && ch <= codeEnd)
                    {
                        sprite.Draw(chars[ch].Map, x, y, (int)ColorValue.White.PackedValue);
                        x += (int)(chars[ch].Width * scale);
                    }
                }
                else 
                {
                    y += origHeight;
                }
            }
        }


        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i].Map.Dispose();
                }
                chars = null;
                Disposed = true;
            }
            else 
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion
    }

    //public class Font : IDisposable
    //{
    //    System.Drawing.Font font;

    //    Texture texture;

    //    Size lastSize;
    //    string lastText;

    //    ObjectFactory factory;

    //    SD.Bitmap buffer;

    //    public RenderSystem RenderSystem
    //    {
    //        get;
    //        private set;
    //    }



    //    public Font(RenderSystem rs, System.Drawing.Font font)
    //    {
    //        this.font = font;
    //        this.RenderSystem = rs;
    //        this.factory = rs.ObjectFactory;

    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (font != null)
    //            {
    //                font.Dispose();
    //            }
    //        }
    //        font = null;
    //    }


    //    public bool Disposed
    //    {
    //        get;
    //        private set;
    //    }

    //    unsafe void UpdateTexture()
    //    {
    //        SDI.BitmapData bmpData = buffer.LockBits(new SD.Rectangle(0, 0, lastSize.Width, lastSize.Height), SDI.ImageLockMode.ReadOnly, SDI.PixelFormat.Format32bppArgb);

    //        DataRectangle rect = texture.Lock(0, LockMode.None);

    //        if (rect.Width * 4 == rect.Pitch)
    //        {
    //            byte* src = (byte*)bmpData.Scan0.ToPointer();
    //            byte* dst = (byte*)rect.Pointer.ToPointer();
    //            int rowSize = rect.Width * 4;

    //            for (int i = 0; i < rect.Height; i++)
    //            {
    //                Memory.Copy(src, dst, rect.MemorySize);
    //                src += rowSize;
    //                dst += rowSize;
    //            }
    //        }
    //        else
    //        {
    //            Memory.Copy(bmpData.Scan0, rect.Pointer, rect.MemorySize);
    //        }

    //        buffer.UnlockBits(bmpData);

    //        texture.Unlock(0);
    //    }
    //    void UpdateText(string text, DrawTextFormat format)
    //    {
    //        SD.Size size = TextRenderer.MeasureText(text, font);

    //        if (size.Width <= lastSize.Width && size.Height <= lastSize.Height)
    //        {
    //            SD.Graphics g = SD.Graphics.FromImage(buffer);

    //            TextRenderer.DrawText(g, text, font, SD.Point.Empty, SD.Color.White, SD.Color.Transparent, GetFlags(format));

    //            g.Dispose();


    //            UpdateTexture();
    //        }
    //        else
    //        {
    //            buffer = new SD.Bitmap(size.Width, size.Height, SDI.PixelFormat.Format32bppArgb);

    //            SD.Graphics g = SD.Graphics.FromImage(buffer);

    //            TextRenderer.DrawText(g, text, font, SD.Point.Empty, SD.Color.White, SD.Color.Transparent, GetFlags(format));

    //            g.Dispose();

    //            buffer.Dispose();

    //            if (texture != null)
    //            {
    //                texture.Dispose();
    //            }
    //            texture = factory.CreateTexture(buffer, TextureUsage.DynamicWriteOnly);
    //            lastSize = (Size)size;
    //        }

    //    }

    //    void UpdateText(string text, Rectangle rectangle, DrawTextFormat format)
    //    {
    //        SD.Size size = TextRenderer.MeasureText(text, font);

    //        if (size.Width <= lastSize.Width && size.Height <= lastSize.Height)
    //        {
    //            SD.Graphics g = SD.Graphics.FromImage(buffer);

    //            TextRenderer.DrawText(g, text, font, SD.Point.Empty, SD.Color.White, SD.Color.Transparent, GetFlags(format));

    //            g.Dispose();

    //            UpdateTexture();
    //        }
    //        else
    //        {
    //            buffer = new SD.Bitmap(size.Width, size.Height, SDI.PixelFormat.Format32bppArgb);

    //            SD.Graphics g = SD.Graphics.FromImage(buffer);

    //            TextRenderer.DrawText(g, text, font, new SD.Rectangle(0, 0, size.Width, size.Height), SD.Color.White, SD.Color.Transparent, GetFlags(format));

    //            g.Dispose();

    //            buffer.Dispose();

    //            if (texture != null)
    //            {
    //                texture.Dispose();
    //            }
    //            texture = factory.CreateTexture(buffer, TextureUsage.DynamicWriteOnly);
    //            lastSize = (Size)size;
    //        }

    //    }
    //    public void DrawString(Sprite sprite, string text, int x, int y, DrawTextFormat format, int color)
    //    {
    //        if (!string.IsNullOrEmpty(text))
    //        {
    //            if (text != lastText)
    //            {
    //                UpdateText(text, format);
    //            }
    //            if (texture != null)
    //            {
    //                sprite.Draw(texture, x, y, color);
    //            }
    //        }
    //        lastText = text;
    //    }
    //    public void DrawString(Sprite sprite, string text, Rectangle rectangle, DrawTextFormat format, int color)
    //    {
    //        if (!string.IsNullOrEmpty(text))
    //        {
    //            if (text != lastText)
    //            {
    //                UpdateText(text, rectangle, format);
    //            }
    //            if (texture != null)
    //            {
    //                sprite.Draw(texture, rectangle, color);
    //            }
    //        }
    //        lastText = text;
    //    }
    //    public Rectangle MeasureString(string text)
    //    {
    //        Size size = (Size)TextRenderer.MeasureText(text, font);
    //        return new Rectangle(0, 0, size.Width, size.Height);
    //    }
    //    public Rectangle MeasureString(string text, DrawTextFormat format, Size prefered)
    //    {
    //        Size size = (Size)TextRenderer.MeasureText(text, font, (System.Drawing.Size)prefered, GetFlags(format));
    //        return new Rectangle(0, 0, size.Width, size.Height);
    //    }

    //    static TextFormatFlags GetFlags(DrawTextFormat format)
    //    {
    //        TextFormatFlags result = TextFormatFlags.Default;
    //        if ((format & DrawTextFormat.Left) == DrawTextFormat.Left)
    //        {
    //            result |= TextFormatFlags.Left;
    //        }
    //        if ((format & DrawTextFormat.Right) == DrawTextFormat.Right)
    //        {
    //            result |= TextFormatFlags.Right;
    //        }
    //        if ((format & DrawTextFormat.Bottom) == DrawTextFormat.Bottom)
    //        {
    //            result |= TextFormatFlags.Bottom;
    //        }
    //        if ((format & DrawTextFormat.Top) == DrawTextFormat.Top)
    //        {
    //            result |= TextFormatFlags.Top;
    //        }
    //        if ((format & DrawTextFormat.Center) == DrawTextFormat.Center)
    //        {
    //            result |= TextFormatFlags.HorizontalCenter;
    //        }
    //        if ((format & DrawTextFormat.VerticalCenter) == DrawTextFormat.VerticalCenter)
    //        {
    //            result |= TextFormatFlags.VerticalCenter;
    //        }

    //        if ((format & DrawTextFormat.WordBreak) == DrawTextFormat.WordBreak)
    //        {
    //            result |= TextFormatFlags.WordBreak;
    //        }
    //        if ((format & DrawTextFormat.SingleLine) == DrawTextFormat.SingleLine)
    //        {
    //            result |= TextFormatFlags.SingleLine;
    //        }
    //        if ((format & DrawTextFormat.RtlReading) == DrawTextFormat.RtlReading)
    //        {
    //            result |= TextFormatFlags.RightToLeft;
    //        }
    //        if ((format & DrawTextFormat.NoClip) == DrawTextFormat.NoClip)
    //        {
    //            result |= TextFormatFlags.NoClipping;
    //        }
    //        if ((format & DrawTextFormat.ExpandTabs) == DrawTextFormat.ExpandTabs)
    //        {
    //            result |= TextFormatFlags.ExpandTabs;
    //        }

    //        return result;
    //    }

    //    #region IDisposable 成员

    //    public void Dispose()
    //    {
    //        if (!Disposed)
    //        {
    //            Dispose(true);
    //            Disposed = true;
    //        }
    //        else
    //        {
    //            throw new ObjectDisposedException(ToString());
    //        }
    //    }

    //    #endregion

    //    ~Font()
    //    {
    //        if (!Disposed)
    //        {
    //            Dispose(false);
    //            Disposed = true;
    //        }
    //    }
    //}
}
