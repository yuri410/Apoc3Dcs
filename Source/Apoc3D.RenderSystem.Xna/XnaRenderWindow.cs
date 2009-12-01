using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderWindow : RenderWindow
    {
        X.Game game;

        public XnaRenderWindow(X.Game game, XnaRenderSystem rs, PresentParameters pm, XnaRenderTarget rt)
            : base(rs, pm, rt)
        {
            this.game = game;
        }

        protected override RenderTarget CreateRenderTarget(Apoc3D.Graphics.RenderSystem rs, PresentParameters pm)
        {
            throw new NotSupportedException("XNAͼ����Ⱦ��ϵͳ���ܽ��������RenderControl");
        }

        public override void Run()
        {
            game.Run();
        }
    }
}