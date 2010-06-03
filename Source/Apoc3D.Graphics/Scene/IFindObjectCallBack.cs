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

namespace Apoc3D.Scene
{
    /// <summary>
    ///  场景查找物体回调
    /// </summary>
    public interface IObjectFilter
    {
        /// <summary>
        ///  检查物体
        ///  对于节点下的物体，引擎会调用这个函数检查物体是否符合条件。当返回true时表明条件符合。
        /// </summary>
        /// <param name="obj">要检查的物体</param>
        /// <returns>布尔值，true时表明条件符合</returns>
        bool Check(SceneObject obj);

        /// <summary>
        ///  检查节点
        ///  引擎会调用这个函数检查节点是否符合条件。当返回true时表明条件符合。
        /// </summary>
        /// <param name="node">要检查的节点</param>
        /// <returns>布尔值，true时表明条件符合</returns>
        bool Check(OctreeSceneNode node);
    }
}
