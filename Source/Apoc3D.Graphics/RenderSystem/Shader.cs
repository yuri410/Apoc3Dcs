using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.Graphics.Effects;
using Apoc3D.Vfs;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics
{
    ///// <summary>
    /////  表示Shader 代码中 Include 文件的文件操作处理器
    ///// </summary>
    //public interface Include
    //{
    //    void Close(Stream stream);
    //    void Open(string fileName, out Stream stream);
    //}

    ///// <summary>
    /////  表示 Shader 代码中的 Marco
    ///// </summary>
    //public struct Macro
    //{
    //    string name;
    //    string defination;

    //    public string Name
    //    {
    //        get { return name; }
    //    }
    //    public string Defination
    //    {
    //        get { return defination; }
    //    }
    //    public Macro(string name, string def)
    //    {
    //        this.name = name;
    //        this.defination = def;
    //    }
    //}

    /// <summary>
    ///  提供Shader的抽象接口
    /// </summary>
    public abstract class Shader : IDisposable
    {
        protected Shader(RenderSystem rs)
        {
            RenderSystem = rs;
        }

        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        public abstract int GetConstantIndex(string name);

        public abstract void SetValue(int index, ref Vector2 value);
        public abstract void SetValue(int index, ref Vector3 value);
        public abstract void SetValue(int index, ref Vector4 value);
        public abstract void SetValue(int index, ref Quaternion value);
        public abstract void SetValue(int index, ref Matrix value);

        public abstract void SetValue(int index, Vector2[] value);
        public abstract void SetValue(int index, Vector3[] value);
        public abstract void SetValue(int index, Vector4[] value);
        public abstract void SetValue(int index, Quaternion[] value);
        public abstract void SetValue(int index, Matrix[] value);

        public abstract void SetValue(int index, bool value);
        public abstract void SetValue(int index, float value);
        public abstract void SetValue(int index, int value);
        public abstract void SetValue(int index, bool[] value);
        public abstract void SetValue(int index, float[] value);
        public abstract void SetValue(int index, int[] value);

        public abstract void SetTexture(int index, Texture tex);
        public abstract void SetSamplerState(int index, ref ShaderSamplerState state);

        public abstract void SetTextureDirect(int index, Texture tex);
        public abstract void SetSamplerStateDirect(int index, ref ShaderSamplerState state);


        public abstract void SetValue(string paramName, ref Vector2 value);
        public abstract void SetValue(string paramName, ref Vector3 value);
        public abstract void SetValue(string paramName, ref Vector4 value);
        public abstract void SetValue(string paramName, ref Quaternion value);
        public abstract void SetValue(string paramName, ref Matrix value);

        public abstract void SetValue(string paramName, Vector2[] value);
        public abstract void SetValue(string paramName, Vector3[] value);
        public abstract void SetValue(string paramName, Vector4[] value);
        public abstract void SetValue(string paramName, Quaternion[] value);
        public abstract void SetValue(string paramName, Matrix[] value);

        public abstract void SetValue(string paramName, bool value);
        public abstract void SetValue(string paramName, float value);
        public abstract void SetValue(string paramName, int value);
        public abstract void SetValue(string paramName, bool[] value);
        public abstract void SetValue(string paramName, float[] value);
        public abstract void SetValue(string paramName, int[] value);

        public abstract void SetTexture(string paramName, Texture tex);
        public abstract void SetSamplerState(string paramName, ref  ShaderSamplerState state);

        public abstract void AutoSetParameters();

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public virtual void Dispose(bool disposing)
        {
            RenderSystem = null;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        ~Shader()
        {
            if (!Disposed)
            {
                Dispose(false);
                Disposed = true;
            }
        }
        #endregion
    }

    

    /// <summary>
    ///  表示顶点Shader
    /// </summary>
    public abstract class VertexShader : Shader
    {
        protected VertexShader(RenderSystem rs)
            : base(rs)
        {
        }
        //public static VertexShader FromResource(ObjectFactory fac, ResourceLocation rl, Macro[] macros, string funcName)
        //{
        //    ContentStreamReader sr = new ContentStreamReader(rl);

        //    string code = sr.ReadToEnd();
        //    sr.Close();

        //    return fac.CreateVertexShader(code, macros, IncludeHandler.Instance, "vs_2_0", funcName);
        //}
    }

    /// <summary>
    ///  表示像素Shader
    /// </summary>
    public abstract class PixelShader : Shader
    {
        protected PixelShader(RenderSystem rs)
            : base(rs)
        {
        }
        //public static PixelShader FromResource(ObjectFactory fac, ResourceLocation rl, Macro[] macros, string funcName)
        //{
        //    ContentStreamReader sr = new ContentStreamReader(rl);

        //    string code = sr.ReadToEnd();
        //    sr.Close();

        //    return fac.CreatePixelShader(code, macros, IncludeHandler.Instance, "ps_2_0", funcName);
        //}
    }
}
