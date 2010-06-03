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
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    public class FontManager : Singleton
    {
        static volatile FontManager singleton;
        static volatile object syncHelper = new object();

        public static FontManager Instance 
        {
            get
            {
                if (singleton == null)
                {
                    lock (syncHelper)
                    {
                        if (singleton == null)
                        {
                            singleton = new FontManager();
                        }
                    }
                }
                return singleton;
            }
        }
        
        Dictionary<string, Font> loadedFonts;

        private FontManager()
        {
            loadedFonts = new Dictionary<string, Font>();
        }

        public Font GetFont(string name) 
        {
            return loadedFonts[name];
        }
        public Font CreateInstance(RenderSystem rs, ResourceLocation rl, string name) 
        {
            Font result;
            if (!loadedFonts.TryGetValue(name, out result)) 
            {
                result = Font.FromResource(rs, rl, name);
                loadedFonts.Add(name, result);
            }
            return result;
        }
        
        //public Font CreateInstance(RenderSystem rs, System.Drawing.Font font)
        //{
        //    Font gfont;
        //    FontDescription tmp = new FontDescription(font.Name, font.Size, font.Style);
        //    if (!buffered.TryGetValue(tmp, out gfont))
        //    {
        //        gfont = new Font(rs, font);
        //    }

        //    return gfont;
        //}

        //public Font CreateInstance(RenderSystem rs, string name, float fontSize, System.Drawing.FontStyle style)
        //{
        //    Font font;
        //    FontDescription tmp = new FontDescription(name, fontSize, style);
        //    if (!buffered.TryGetValue(tmp, out font))
        //    {
        //        font = new Font(rs, new System.Drawing.Font(name, fontSize, style));
        //    }

        //    return font;
        //}

        protected override void dispose()
        {
            Dictionary<string, Font>.ValueCollection vals = loadedFonts.Values;
            foreach (Font e in vals)
            {
                e.Dispose();
            }
            loadedFonts.Clear();
        }
    }
}
