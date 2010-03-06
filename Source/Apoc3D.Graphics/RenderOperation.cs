using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Graphics;

namespace Apoc3D.Graphics
{
    /// <summary>
    /// 一个渲染操作由下列三个元素组成:
    ///     变换
    ///     材质
    ///     几何信息
    /// </summary>
    public struct RenderOperation
    {
        /// <summary>
        ///  此渲染操作的世界变换矩阵
        /// </summary>
        public Matrix Transformation;

        /// <summary>
        ///  此渲染操作的材质
        /// </summary>
        public Material Material;

        /// <summary>
        ///  此渲染操作的几何数据
        /// </summary>
        public GeomentryData Geomentry;

        /// <summary>
        ///  此渲染操作的源对象
        /// </summary>
        public object Sender;
    }

    /// <summary>
    ///  表示几何数据
    /// </summary>
    public class GeomentryData
    {
        public int VertexSize
        {
            get;
            set;
        }
        public VertexDeclaration VertexDeclaration
        {
            get;
            set;
        }

        public VertexBuffer VertexBuffer
        {
            get;
            set;
        }
        public IndexBuffer IndexBuffer
        {
            get;
            set;
        }
        public bool UseIndices
        {
            get { return IndexBuffer != null; }
        }
       
        /// <summary>
        ///  获取或设置几何数据的图元类型
        /// </summary>
        public RenderPrimitiveType PrimitiveType
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置几何数据的图元数量
        /// </summary>
        public int PrimCount
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置几何数据的顶点数量
        /// </summary>
        public int VertexCount
        {
            get;
            set;
        }

        public GeomentryData()
        {
            //if (sender != null)
            //    throw new InvalidOperationException();
            //sender = obj;
        }

        public int BaseVertex
        {
            get;
            set;
        }
        public int BaseIndexStart
        {
            get;
            set;
        }
    }
}
