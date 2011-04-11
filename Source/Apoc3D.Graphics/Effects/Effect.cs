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
using Apoc3D.Scene;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics.Effects
{
    public abstract class EffectFactory
    {
        public abstract Effect CreateInstance();

        public abstract void DestroyInstance(Effect fx);
    }

    public abstract class Effect
    {
        public static readonly string DefaultTechnique = "Default";

        bool begun;
        protected RenderMode mode;

        protected Effect(bool supportsInstancing, string name)
        {
            Name = name;
            SupportsInstancing = supportsInstancing;
        }

        protected VertexShader LoadVertexShader(RenderSystem rs, ResourceLocation vs)
        {
            ObjectFactory fac = rs.ObjectFactory;
            return fac.CreateVertexShader(vs);
        }
        protected PixelShader LoadPixelShader(RenderSystem rs, ResourceLocation ps)
        {
            ObjectFactory fac = rs.ObjectFactory;
            return fac.CreatePixelShader(ps);
        }

        #region 属性

        public virtual bool SupportsTechnique(string name)
        {
            return true;
        }
        public virtual bool SupportsMode(RenderMode mode)
        {
            return true;
        }
        public bool SupportsInstancing
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            private set;
        }
        protected bool EnableAutoParameter
        {
            get;
            set;
        }
        #endregion

        protected void SetAutoParameter(PixelShader pixShader, VertexShader vtxShader)
        {
            if (pixShader != null)
            {
                pixShader.AutoSetParameters();
            }
            if (vtxShader != null)
            {
                pixShader.AutoSetParameters();
            }
        }

        protected abstract int begin();
        protected abstract void end();
        public abstract void BeginPass(int passId);
        public abstract void EndPass();

        //public abstract void BeginShadowPass();
        //public abstract void EndShadowPass();

        public int Begin(RenderMode mode)
        {
            if (!begun)
            {
                begun = true;
                this.mode = mode;
                return begin();
            }
            return -1;
        }

        public void End()
        {
            if (begun)
            {
                end();
                begun = false;
            }
        }

        protected virtual int beginInst()
        {
            throw new NotSupportedException();
        }
        protected virtual void endInst()
        {
            throw new NotSupportedException();
        }
        public virtual void BeginPassInst(int passId)
        {
            throw new NotSupportedException();
        }
        public virtual void EndPassInst()
        {
            throw new NotSupportedException();
        }

        public int BeginInst()
        {
            if (!begun)
            {
                begun = true;
                return beginInst();
            }
            return -1;
        }
        public void EndInst()
        {
            if (begun)
            {
                endInst();
                begun = false;
            }
        }


        public virtual void SetupInstancing(Material mat)
        {
            throw new NotSupportedException();
        }

        public abstract void Setup(Material mat, ref RenderOperation op);
        //public abstract void SetupShadowPass(Material mat, ref RenderOperation op);

 
        #region IDisposable 成员
        public bool Disposed
        {
            get;
            private set;
        }
        protected abstract void Dispose(bool disposing);


        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(ToString());
        }

        #endregion

        ~Effect()
        {
            if (!Disposed)
                Dispose();
        }
    }
}
