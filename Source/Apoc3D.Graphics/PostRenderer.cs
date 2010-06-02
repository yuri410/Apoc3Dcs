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
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  定义可以渲染场景的接口，供后期效果渲染器使用。
    /// </summary>
    public interface ISceneRenderer
    {
        /// <summary>
        ///  正常渲染场景
        /// </summary>
        /// <param name="target">场景渲染目标</param>
        void RenderScene(RenderTarget target, RenderMode mode);
    }

    /// <summary>
    ///  定义后期效果渲染器的接口
    /// </summary>
    public interface IPostSceneRenderer : IDisposable
    {
        /// <summary>
        ///  进行全部渲染，形成最总图像（带后期渲染）
        /// </summary>
        /// <param name="renderer">实现ISceneRenderer可以渲染场景的对象</param>
        /// <param name="screenTarget">渲染目标</param>
        void RenderFullScene(ISceneRenderer renderer, RenderTarget screenTarget, RenderMode mode);
    }

    public class DefaultPostRenderer : IPostSceneRenderer 
    {

        #region IPostSceneRenderer 成员

        public void RenderFullScene(ISceneRenderer renderer, RenderTarget screenTarget, RenderMode mode)
        {
            renderer.RenderScene(screenTarget, mode);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            
        }

        #endregion
    }

}
