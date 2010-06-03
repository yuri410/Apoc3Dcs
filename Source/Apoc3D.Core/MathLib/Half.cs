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

namespace Apoc3D.MathLib
{
    public struct Half
    {
        public static unsafe ushort Convert(float value)
        {
            uint num5 = *((uint*)&value);
            uint num3 = (uint)((num5 & 0x80000000) >> 0x10);
            uint num = num5 & 0x7fffffff;
            if (num > 0x47ffefff)
            {
                return (ushort)(num3 | 0x7fff);
            }
            if (num < 0x38800000)
            {
                uint num6 = (num & 0x7fffff) | 0x800000;
                int num4 = 0x71 - ((int)(num >> 0x17));
                num = (num4 > 0x1f) ? 0x0 : (num6 >> num4);
                return (ushort)(num3 | (((num + 0xfff) + ((num >> 0xd) & 0x1)) >> 0xd));
            }
            return (ushort)(num3 | ((((num + 0xc8000000) + 0xfff) + ((num >> 0xd) & 0x1)) >> 0xd));

        }
        public static unsafe float Convert(ushort value)
        {
            uint num3;
            if ((value & 0xffff7c00) == 0)
            {
                if ((value & 0x3ff) != 0)
                {
                    uint num2 = 0xfffffff2;
                    uint num = (uint)(value & 0x3ff);
                    while ((num & 0x400) == 0)
                    {
                        num2--;
                        num = num << 0x1;
                    }
                    num &= 0xfffffbff;
                    num3 = (((uint)value & 0x8000) << 0x10) | ((num2 + 0x7f) << 0x17) | (num << 0xd);
                }
                else
                {
                    num3 = (uint)((value & 0x8000) << 0x10);
                }
            }
            else
            {
                num3 = (uint)((((value & 0x8000) << 0x10) | (((value >> 0xa) & 0x1f) - 0xf + 0x7f) << 0x17) | ((value & 0x3ff) << 0xd));
            }
            return *(float*)&num3;
        }


        ushort value;

        public Half(float value)
        {
            this.value = Convert(value);
        }
        public Half(ushort value)
        {
            this.value = value;
        }
        public static float[] ConvertToSingle(Half[] values)
        {
            float[] result = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = Half.Convert(values[i].InternalValue);
            }
            return result;
        }
        public static Half[] ConvertToHalf(float[] floats)
        {
            Half[] result = new Half[floats.Length];
            for (int i = 0; i < floats.Length; i++)
            {
                result[i] = new Half(floats[i]);
            }
            return result;
        }

        public ushort InternalValue 
        {
            get { return value; }
            set { this.value = value; }
        }

        public float ToSingle()
        {
            return Convert(value);
        }

        
    }
}
