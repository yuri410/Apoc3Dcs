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
using Apoc3D.Graphics.Collections;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaTextureWrapCollection : TextureWrapCollection
    {
        XG.GraphicsDevice device;

        internal XG.RenderState xnaState
        {
            get { return device.RenderState; }
        }

        internal XnaTextureWrapCollection(XG.GraphicsDevice device)
        {
            this.device = device;
        }

        public override TextureWrapCoordinates this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return XnaUtils.ConvertEnum(xnaState.Wrap0);
                    case 1: return XnaUtils.ConvertEnum(xnaState.Wrap1);
                    case 2: return XnaUtils.ConvertEnum(xnaState.Wrap2);
                    case 3: return XnaUtils.ConvertEnum(xnaState.Wrap3);
                    case 4: return XnaUtils.ConvertEnum(xnaState.Wrap4);
                    case 5: return XnaUtils.ConvertEnum(xnaState.Wrap5);
                    case 6: return XnaUtils.ConvertEnum(xnaState.Wrap6);
                    case 7: return XnaUtils.ConvertEnum(xnaState.Wrap7);
                    case 8: return XnaUtils.ConvertEnum(xnaState.Wrap8);
                    case 9: return XnaUtils.ConvertEnum(xnaState.Wrap9);
                    case 10: return XnaUtils.ConvertEnum(xnaState.Wrap10);
                    case 11: return XnaUtils.ConvertEnum(xnaState.Wrap11);
                    case 12: return XnaUtils.ConvertEnum(xnaState.Wrap12);
                    case 13: return XnaUtils.ConvertEnum(xnaState.Wrap13);
                    case 14: return XnaUtils.ConvertEnum(xnaState.Wrap14);
                    case 15: return XnaUtils.ConvertEnum(xnaState.Wrap15);
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (index)
                {
                    case 0: xnaState.Wrap0 = XnaUtils.ConvertEnum(value); break;
                    case 1: xnaState.Wrap1 = XnaUtils.ConvertEnum(value); break;
                    case 2: xnaState.Wrap2 = XnaUtils.ConvertEnum(value); break;
                    case 3: xnaState.Wrap3 = XnaUtils.ConvertEnum(value); break;
                    case 4: xnaState.Wrap4 = XnaUtils.ConvertEnum(value); break;
                    case 5: xnaState.Wrap5 = XnaUtils.ConvertEnum(value); break;
                    case 6: xnaState.Wrap6 = XnaUtils.ConvertEnum(value); break;
                    case 7: xnaState.Wrap7 = XnaUtils.ConvertEnum(value); break;
                    case 8: xnaState.Wrap8 = XnaUtils.ConvertEnum(value); break;
                    case 9: xnaState.Wrap9 = XnaUtils.ConvertEnum(value); break;
                    case 10: xnaState.Wrap10 = XnaUtils.ConvertEnum(value); break;
                    case 11: xnaState.Wrap11 = XnaUtils.ConvertEnum(value); break;
                    case 12: xnaState.Wrap12 = XnaUtils.ConvertEnum(value); break;
                    case 13: xnaState.Wrap13 = XnaUtils.ConvertEnum(value); break;
                    case 14: xnaState.Wrap14 = XnaUtils.ConvertEnum(value); break;
                    case 15: xnaState.Wrap15 = XnaUtils.ConvertEnum(value); break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
    }
}