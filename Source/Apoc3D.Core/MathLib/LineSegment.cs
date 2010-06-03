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
    /// <summary>
    ///  定义线段
    /// </summary>
    public struct LineSegment
    {
        public LineSegment(Vector3 start, Vector3 end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        ///  获取或设置线段起点
        /// </summary>
        public Vector3 Start;
        /// <summary>
        ///  获取或设置线段终点
        /// </summary>
        public Vector3 End;

        /// <summary>
        ///  获得线段的System.String表达形式
        /// </summary>
        /// <returns>线段的System.String表达形式</returns>
        public override string ToString()
        {
            return "Start:" + Start.ToString() + " End:" + End.ToString();
        }
    }
}
