using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示渲染窗口的事件处理对象
    /// </summary>
    public interface IRenderWindowHandler
    {
        /// <summary>
        ///  处理初始化事件
        /// </summary>
        void Initialize();
        void finalize();

        void Load();
        void Unload();
        void Update(GameTime time);
        void Draw();
    }
}
