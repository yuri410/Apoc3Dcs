﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  渲染设备的渲染目标参数
    /// </summary>
    public struct PresentParameters
    {
        bool isWindowed;
        bool isFullForm;

        int bbWidth;
        int bbHeight;

        PresentInterval presentInterval;

        DepthFormat depthBufferFormat;

        ImagePixelFormat colorBufferFormat;

        public object Target
        {
            get;
            set;
        }

        public ImagePixelFormat BackBufferFormat
        {
            get { return colorBufferFormat; }
            set { colorBufferFormat = value; }
        }

        public DepthFormat DepthFormat
        {
            get { return depthBufferFormat; }
            set { depthBufferFormat = value; }
        }

        public PresentInterval PresentInterval
        {
            get { return presentInterval; }
            set { presentInterval = value; }
        }

        public bool IsWindowed
        {
            get { return isWindowed; }
            set { isWindowed = value; }
        }

        public bool IsFullForm
        {
            get { return isFullForm; }
            set { isFullForm = value; }
        }

        public int BackBufferWidth
        {
            get { return bbWidth; }
            set { bbWidth = value; }
        }

        public int BackBufferHeight
        {
            get { return bbHeight; }
            set { bbHeight = value; }
        }


    }

    /// <summary>
    ///  表示图形API的渲染设备，可以创建渲染目标
    /// </summary>
    public abstract class DeviceContent
    {
        FastList<RenderControl> renderVps;

        public bool SupportsRenderControl
        {
            get;
            private set;
        }

        protected DeviceContent(bool supportsRenderCtrl)
        {
            renderVps = new FastList<RenderControl>();
            SupportsRenderControl = supportsRenderCtrl;
        }

        protected abstract RenderControl create(PresentParameters pm);
     
        /// <summary>
        ///  创建一个渲染控件，渲染到一个控件中。
        ///  根据参数的不同，可以使用现有的控件或者由渲染子系统建立一个新的。
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public RenderControl Create(PresentParameters pm)
        {
            RenderControl rc = create(pm);
            renderVps.Add(rc);
            return rc;
        }

        public void Destroy(RenderControl rc)
        {
            renderVps.Remove(rc);
            rc.Dispose();
        }

        /// <summary>
        ///  获取该渲染设备对应的渲染系统
        /// </summary>
        public abstract RenderSystem RenderSystem
        {
            get;
        }
    }
}
