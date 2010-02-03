using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using XF = Microsoft.Xna.Framework;
using XFG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaSprite : Sprite
    {
        XFG.SpriteBatch sprite;
        bool begun;

        public XnaSprite(XnaRenderSystem rs)
            : base(rs)
        {
            sprite = new XFG.SpriteBatch(rs.Device);
        }
        public override void Begin()
        {
            sprite.Begin(XFG.SpriteBlendMode.AlphaBlend, XFG.SpriteSortMode.Immediate, XFG.SaveStateMode.None);
            begun = true;

        }
        public override void Draw(Texture texture, Rectangle rect, ColorValue color)
        {
            XnaTexture xt = (XnaTexture)texture;
            XFG.Texture2D tex2D = xt.tex2D;
            if (tex2D != null)
            {
                XF.Rectangle xrect;
                xrect.X = rect.X;
                xrect.Y = rect.Y;
                xrect.Width = rect.Width;
                xrect.Height = rect.Height;

                XFG.Color clr = new XFG.Color(color.R, color.G, color.B, color.A);

                sprite.Draw(tex2D, xrect, clr);
            }
        }
        public override void Draw(Texture texture, Vector2 pos, ColorValue color)
        {
            XnaTexture xt = (XnaTexture)texture;
            XFG.Texture2D tex2D = xt.tex2D;
            if (tex2D != null)
            {
                XF.Vector2 xpos;
                xpos.X = pos.X;
                xpos.Y = pos.Y;

                XFG.Color clr = new XFG.Color(color.R, color.G, color.B, color.A);

                sprite.Draw(tex2D, xpos, clr);
            }
        }
        public override void Draw(Texture texture, Rectangle dstRect, Rectangle? srcRect, ColorValue color)
        {
            XnaTexture xt = (XnaTexture)texture;
            XFG.Texture2D tex2D = xt.tex2D;
            if (tex2D != null)
            {
                XF.Rectangle xrect;
                xrect.X = dstRect.X;
                xrect.Y = dstRect.Y;
                xrect.Width = dstRect.Width;
                xrect.Height = dstRect.Height;

                XF.Rectangle srect = new XF.Rectangle();

                if (srcRect != null) 
                {
                    Rectangle srcRectVal = srcRect.Value;

                    srect.X = srcRectVal.X;
                    srect.Y = srcRectVal.Y;
                    srect.Width = srcRectVal.Width;
                    srect.Height = srcRectVal.Height;
                }

                XFG.Color clr = new XFG.Color(color.R, color.G, color.B, color.A);

                sprite.Draw(tex2D, xrect, srcRect == null ? 
                    null : new Nullable<XF.Rectangle>(srect), clr);
            }
        }

        public override void Draw(Texture texture, int x, int y, ColorValue color)
        {
            XnaTexture xt = (XnaTexture)texture;
            XFG.Texture2D tex2D = xt.tex2D;
            if (tex2D != null)
            {
                XF.Vector2 xpos;
                xpos.X = x;
                xpos.Y = y;

                XFG.Color clr = new XFG.Color(color.R, color.G, color.B, color.A);

                sprite.Draw(tex2D, xpos, clr);
            }
        }
        public override void SetTransform(Matrix matrix)
        {
            if (begun) 
            {
                sprite.End();

                XF.Matrix mat;
                mat.M11 = matrix.M11; mat.M12 = matrix.M12; mat.M13 = matrix.M13; mat.M14 = matrix.M14;
                mat.M21 = matrix.M21; mat.M22 = matrix.M22; mat.M23 = matrix.M23; mat.M24 = matrix.M24;
                mat.M31 = matrix.M31; mat.M32 = matrix.M32; mat.M33 = matrix.M33; mat.M34 = matrix.M34;
                mat.M41 = matrix.M41; mat.M42 = matrix.M42; mat.M43 = matrix.M43; mat.M44 = matrix.M44;

                sprite.Begin(XFG.SpriteBlendMode.AlphaBlend, XFG.SpriteSortMode.Immediate, XFG.SaveStateMode.None, mat);
            }
        }
        public override void End()
        {
            sprite.End();
            begun = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                sprite.Dispose();
            }
            sprite = null;
        }
    }
}
