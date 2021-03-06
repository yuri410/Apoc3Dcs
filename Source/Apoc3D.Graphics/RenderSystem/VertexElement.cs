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

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  顶点元素，即组成一个<see cref="VertexDeclaration"/>的元素
    /// </summary>
    public struct VertexElement
    {
        #region 字段

        /// <summary>
        ///  表示顶点元素的起始偏移量
        /// </summary>
        int offset;
        /// <summary>
        ///  表示顶点元素的格式
        /// </summary>
        VertexElementFormat type;
        /// <summary>
        ///  表示顶点元素的实际用途
        /// </summary>
        VertexElementUsage semantic;
        /// <summary>
        ///  顶点元素的索引，仅适用于一些类型的顶点元素例如纹理坐标
        /// </summary>
        int index;

        #endregion Fields

        #region 构造函数

        /// <summary>
        ///   
        /// </summary>
        /// <param name="offset">The offset in the buffer that this element starts at.</param>
        /// <param name="type">The type of element.</param>
        /// <param name="semantic">The meaning of the element.</param>
        public VertexElement(int offset, VertexElementFormat type, VertexElementUsage semantic)
            : this(offset, type, semantic, 0)
        {
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="offset">The offset in the buffer that this element starts at.</param>
        /// <param name="type">The type of element.</param>
        /// <param name="semantic">The meaning of the element.</param>
        /// <param name="index">Index of the item, only applicable for some elements like texture coords.</param>
        public VertexElement(int offset, VertexElementFormat type, VertexElementUsage semantic, int index)
        {
            //this.source = source;
            this.offset = offset;
            this.type = type;
            this.semantic = semantic;
            this.index = index;
        }

        #endregion

        #region Methods

        public static bool Compare(VertexElement[] e1, VertexElement[] e2)
        {
            if (e1.Length == e2.Length) 
            {
                for (int i = 0; i < e1.Length; i++)
                {
                    if (e1[i] != e2[i]) 
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        /// <summary>
        ///  计算顶点元素的大小
        /// </summary>
        /// <param name="type">顶点元素的类型</param>
        public static int GetTypeSize(VertexElementFormat type)
        {

            switch (type)
            {
                case VertexElementFormat.Rg32:
                case VertexElementFormat.Rgba32:
                case VertexElementFormat.Color:
                    return sizeof(int);

                case VertexElementFormat.Single:
                    return sizeof(float);

                case VertexElementFormat.Vector2:
                    return sizeof(float) * 2;

                case VertexElementFormat.Vector3:
                    return sizeof(float) * 3;

                case VertexElementFormat.Vector4:
                    return sizeof(float) * 4;

                case VertexElementFormat.NormalizedShort2:
                case VertexElementFormat.Short2:
                    return sizeof(short) * 2;

                case VertexElementFormat.NormalizedShort4:
                case VertexElementFormat.Short4:
                    return sizeof(short) * 4;

                case VertexElementFormat.Byte4:
                    return sizeof(byte) * 4;
                case VertexElementFormat.HalfVector2:
                    return sizeof(ushort) * 2;
                case VertexElementFormat.HalfVector4:
                    return sizeof(ushort) * 4;

            } // end switch

            // keep the compiler happy
            return 0;
        }

        /// <summary>
        ///     Utility method which returns the count of values in a given type.
        /// </summary>
        public static int GetTypeCount(VertexElementFormat type)
        {
            switch (type)
            {
                case VertexElementFormat.Rg32:
                case VertexElementFormat.Rgba32:
                case VertexElementFormat.Color:
                    return 1;

                case VertexElementFormat.Single:
                    return 1;

                case VertexElementFormat.Vector2:
                    return 2;

                case VertexElementFormat.Vector3:
                    return 3;

                case VertexElementFormat.Vector4:
                    return 4;

                case VertexElementFormat.NormalizedShort2:
                case VertexElementFormat.Short2:
                    return 2;

                case VertexElementFormat.NormalizedShort4:
                case VertexElementFormat.Short4:
                    return 4;

                case VertexElementFormat.Byte4:
                    return 4;

                case VertexElementFormat.HalfVector2:
                    return 2;
                case VertexElementFormat.HalfVector4:
                    return 4;
            } // end switch			

            // keep the compiler happy
            return 0;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the offset into the buffer where this element starts.
        /// </summary>
        public int Offset
        {
            get { return offset; }
        }

        /// <summary>
        ///     Gets the data format of this element.
        /// </summary>
        public VertexElementFormat Type
        {
            get { return type; }
        }

        /// <summary>
        ///     Gets the meaning of this element.
        /// </summary>
        public VertexElementUsage Semantic
        {
            get { return semantic; }
        }

        /// <summary>
        ///     Gets the index of this element, only applicable for repeating elements (like texcoords).
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        ///     Gets the size of this element in bytes.
        /// </summary>
        public int Size
        {
            get { return GetTypeSize(type); }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is VertexElement)
            {
                return (VertexElement)obj == this;
            }
            return false;
        }
         

        public static bool operator ==(VertexElement left, VertexElement right)
        {
            return left.index == right.index &&
                left.offset == right.offset &&
                left.semantic == right.semantic &&
                left.type == right.type;
        }

        public static bool operator !=(VertexElement left, VertexElement right)
        {
            return left.index != right.index ||
                left.offset != right.offset ||
                left.semantic != right.semantic ||
                left.type != right.type;
        }

        public override int GetHashCode()
        {
            return index.GetHashCode() + offset.GetHashCode() + semantic.GetHashCode() + type.GetHashCode();
        }
    }
}
