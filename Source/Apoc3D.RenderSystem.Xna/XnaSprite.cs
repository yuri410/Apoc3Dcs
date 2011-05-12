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
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using XF = Microsoft.Xna.Framework;
using XFG = Microsoft.Xna.Framework.Graphics;
using Apoc3D.Graphics.Effects;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaSprite : Sprite
    {
        XFG.SpriteBatch sprite;
        XnaRenderSystem xnaRs;

        bool begun;

        public XnaSprite(XnaRenderSystem rs)
            : base(rs)
        {
            sprite = new XFG.SpriteBatch(rs.Device);
            xnaRs = rs;
        }
        public override void Begin(bool alphaBlend)
        {
            sprite.Begin(alphaBlend ? XFG.SpriteBlendMode.AlphaBlend : XFG.SpriteBlendMode.None, XFG.SpriteSortMode.Immediate, XFG.SaveStateMode.None);
            begun = true;

        }
        public override void Begin()
        {
            sprite.Begin(XFG.SpriteBlendMode.AlphaBlend, XFG.SpriteSortMode.Immediate, XFG.SaveStateMode.None);
            xnaRs.Device.RenderState.AlphaDestinationBlend = XFG.Blend.InverseDestinationColor;
            xnaRs.Device.RenderState.AlphaSourceBlend = XFG.Blend.SourceColor;
            begun = true;

        }
        public override void DrawQuad(GeomentryData quad, PostEffect effect)
        {
            //End();

            effect.Begin();

            xnaRs.RenderSimpleBlend(quad);
            effect.End();

            //Begin();
        }
        public override void Draw(Texture texture, Rectangle rect, ColorValue color)
        {
            XnaTexture xt = (XnaTexture)texture;
            XFG.Texture2D tex2D = xt.tex2D;
            if (tex2D != null)
            {
#if !XBOX
                XF.Rectangle xrect;
#else
                XF.Rectangle xrect = new XF.Rectangle();
#endif
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
#if !XBOX
                XF.Vector2 xpos;
#else
                XF.Vector2 xpos = new XF.Vector2();
#endif
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
#if !XBOX
                XF.Rectangle xrect;
#else
                XF.Rectangle xrect = new XF.Rectangle();
#endif
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
#if !XBOX
                XF.Vector2 xpos;
#else
                XF.Vector2 xpos = new XF.Vector2();
#endif
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

#if !XBOX
                XF.Matrix mat;
#else
                XF.Matrix mat = new XF.Matrix();
#endif

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
