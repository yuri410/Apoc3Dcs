using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    public interface IRenderWindowHandler
    {
        void Initialize();
        void Load();
        void Unload();
        void Update(GameTime time);
        void Draw();
    }
}
