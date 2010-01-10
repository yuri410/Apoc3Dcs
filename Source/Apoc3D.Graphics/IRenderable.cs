﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    public enum RenderPriority
    {
        First,
        Second,
        Third,
        Last
    }
    /// <summary>
    ///  定义一种通过 获取渲染操作(RenderOperation) 的方式 的一种便于管理的渲染方式
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        ///  获得渲染操作
        /// </summary>
        /// <returns></returns>
        RenderOperation[] GetRenderOperation();

        /// <summary>
        ///  获得特定LOD级别的渲染操作
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        RenderOperation[] GetRenderOperation(int level);


    }
}
