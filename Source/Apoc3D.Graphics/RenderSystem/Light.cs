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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    public enum LightType : int
    {
        /// Point light sources give off light equally in all directions, so require only position not direction
        Point = 1,
        /// Directional lights simulate parallel light beams from a distant source, hence have direction but no position
        Directional = 3,
        /// Spotlights simulate a cone of light from a source so require position and direction, plus extra values for falloff
        Spotlight = 2
    }

    /// <summary>
    /// Defines a set of lighting properties.
    /// </summary>
    public class Light
    {
        Vector3 position;
        Vector3 direction;
        LightType ltType;
        Color4F ambient;
        Color4F diffuse;
        Color4F specular;

        public float SpotOuter
        {
            get { return spotOuter; }
            set { spotOuter = value; }
        }
        public float SpotInner
        {
            get { return spotInner; }
            set { spotInner = value; }
        }
        public float SpotFalloff
        {
            get { return spotFalloff; }
            set { spotFalloff = value; }
        }
        public float Range
        {
            get { return range; }
            set { range = value; }
        }
        public float AttenuationConst
        {
            get { return attenuationConst; }
            set { attenuationConst = value; }
        }
        public float AttenuationLinear
        {
            get { return attenuationLinear; }
            set { attenuationLinear = value; }
        }
        public float AttenuationQuad
        {
            get { return attenuationQuad; }
            set { attenuationQuad = value; }
        }


        float spotOuter;
        float spotInner;
        float spotFalloff;
        float range;
        float attenuationConst;
        float attenuationLinear;
        float attenuationQuad;
        float power;

        public float Power
        {
            get { return power; }
            set { power = value; }
        }
        /// <summary>
        /// Gets or sets the position of the light in world space. This member does not affect directional lights.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// Gets or sets the direction of the light. This vector need not be normalized, but should have a non-zero length.
        /// </summary>
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        /// <summary>
        /// Gets or sets the ambient color of the light.
        /// </summary>
        public Color4F Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }
        /// <summary>
        /// Gets or sets the diffuse color of the light.
        /// </summary>
        public Color4F Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }
        /// <summary>
        /// Gets or sets the specular color of the light.
        /// </summary>
        public Color4F Specular
        {
            get { return specular; }
            set { specular = value; }
        }
        /// <summary>
        /// Gets or sets the type of the light source.
        /// </summary>
        public LightType Type
        {
            get { return ltType; }
            set { ltType = value; }
        }
    }
}
