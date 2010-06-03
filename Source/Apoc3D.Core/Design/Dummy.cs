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
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Apoc3D.Design
{
#if XBOX
    /// <summary>
    /// .net CF��֧��û��������ͣ���ʵ����;��Ϊ���ñ�����ͨ���á�����ʹ��Macro��ά����
    /// </summary>
    public class ExpandableObjectConverter : TypeConverter { }
   
    /// <summary>
    /// .net CF��֧��û��������ͣ���ʵ����;��Ϊ���ñ�����ͨ���á�����ʹ��Macro��ά����
    /// </summary>
    public class UITypeEditor { }
   
    /// <summary>
    /// .net CF��֧��û��������ͣ���ʵ����;��Ϊ���ñ�����ͨ���á�����ʹ��Macro��ά����
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class TypeConverterAttribute : Attribute
    {
        public static readonly TypeConverterAttribute Default = new TypeConverterAttribute();

        string typeName;

        public TypeConverterAttribute()
        {
            this.typeName = string.Empty;
        }

        public TypeConverterAttribute(string typeName)
        {
            typeName.ToUpper(CultureInfo.InvariantCulture);
            this.typeName = typeName;
        }

        public TypeConverterAttribute(Type type)
        {
            this.typeName = type.AssemblyQualifiedName;
        }

        public override bool Equals(object obj)
        {
            TypeConverterAttribute attribute = obj as TypeConverterAttribute;
            return ((attribute != null) && (attribute.ConverterTypeName == this.typeName));
        }

        public override int GetHashCode()
        {
            return this.typeName.GetHashCode();
        }

        public string ConverterTypeName
        {
            get { return typeName; }
        }
    }
    
    /// <summary>
    /// .net CF��֧��û��������ͣ���ʵ����;��Ϊ���ñ�����ͨ���á�����ʹ��Macro��ά����
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BrowsableAttribute : Attribute
    {
        public static readonly BrowsableAttribute Default = Yes;
        public static readonly BrowsableAttribute No = new BrowsableAttribute(false);
        public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

        bool browsable = true;

        public BrowsableAttribute(bool browsable)
        {
            this.browsable = browsable;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            BrowsableAttribute attribute = obj as BrowsableAttribute;
            return ((attribute != null) && (attribute.Browsable == this.browsable));
        }

        public override int GetHashCode()
        {
            return this.browsable.GetHashCode();
        }

        public bool Browsable
        {
            get { return browsable; }
        }

    }
#endif
}