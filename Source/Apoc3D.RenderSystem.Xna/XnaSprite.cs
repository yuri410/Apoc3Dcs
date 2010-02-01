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

        public XnaSprite(XnaRenderSystem rs)
            : base(rs)
        {
            sprite = new XFG.SpriteBatch(rs.Device);
        }
        public override void Begin()
        {
            sprite.Begin(XFG.SpriteBlendMode.AlphaBlend, XFG.SpriteSortMode.Immediate, XFG.SaveStateMode.None);
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

        public override void End()
        {
            sprite.End();
        }
    }
}
