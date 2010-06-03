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
using Apoc3D.MathLib;
using Apoc3D.Scene;

namespace Apoc3D.Graphics.Effects
{
    public static class EffectParams
    {
        static EffectParams()
        {
            DepthMap = new Texture[5];
        }

        public static Vector3 LightDir = new Vector3(-1, 0, 0);
        public static Color4F LightAmbient = new Color4F(1, .5f, .5f, .5f);
        public static Color4F LightDiffuse = new Color4F(1f, .8f, .8f, .8f);
        public static Color4F LightSpecular = new Color4F(1f, 1f, 1f, 1f);

        //public static ShadowMap ShadowMap
        //{
        //    get;
        //    set;
        //}

        public static Texture[] DepthMap;

        public static Atmosphere Atmosphere
        {
            get;
            set;
        }

        public static ICamera CurrentCamera
        {
            get;
            set;
        }

        public static Matrix DepthViewProj;

        public static Matrix InvView;
    }
}
