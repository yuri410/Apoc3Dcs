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
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Apoc3D.Graphics;

namespace Apoc3D.Design
{
#if !XBOX
    using System.ComponentModel.Design.Serialization;
#endif

    public class MeshFaceConverter : ExpandableObjectConverter
    {
#if !XBOX
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (((destinationType == typeof(string))
                || (destinationType == typeof(InstanceDescriptor)))
                || base.CanConvertTo(context, destinationType));
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string @string = null;
            string[] stringArray = null;
            TypeConverter converter = null;
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            @string = value as string;
            if (@string != null)
            {
                @string = @string.Trim();
                converter = TypeDescriptor.GetConverter(typeof(int));
                char[] separator = new char[] { culture.TextInfo.ListSeparator[0] };
                stringArray = @string.Split(separator);
                int idxA = (int)converter.ConvertFromString(context, culture, stringArray[0]);
                int idxB = (int)converter.ConvertFromString(context, culture, stringArray[1]);
                int idxC = (int)converter.ConvertFromString(context, culture, stringArray[2]);
                int matId = (int)converter.ConvertFromString(context, culture, stringArray[3]);

                //ValueType modopt(Vector2) modopt(IsBoxed) type = new Vector2();
                return new MeshFace(idxA, idxB, idxC, matId);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //ValueType modopt(Vector2) modopt(IsBoxed) vector = null;
            //Type[] $S9 = null;
            string[] stringArray = null;
            ConstructorInfo info = null;
            TypeConverter converter = null;
            string separator = null;
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            if ((destinationType == typeof(string)) && (value is MeshFace))
            {
                separator = culture.TextInfo.ListSeparator + " ";
                converter = TypeDescriptor.GetConverter(typeof(int));
                MeshFace faceData = (MeshFace)value;

                stringArray = new string[]
                {
                    converter.ConvertToString(context, culture, faceData.IndexA),
                    converter.ConvertToString(context, culture, faceData.IndexB),
                    converter.ConvertToString(context, culture, faceData.IndexC),
                    converter.ConvertToString(context, culture, faceData.MaterialIndex)
                };

                return string.Join(separator, stringArray);
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is MeshFace))
            {
                //$S9 = new Type[] { typeof(float), typeof(float) };
                MeshFace faceData = (MeshFace)value;
                info = typeof(MeshFace).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
                if (info != null)
                {
                    return new InstanceDescriptor(info, new object[] { faceData.IndexA, faceData.IndexB, faceData.IndexC, faceData.MaterialIndex });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues");
            }
            //ValueType modopt(Vector2) modopt(IsBoxed) type = new Vector2();
            return new MeshFace((int)propertyValues["IndexA"], (int)propertyValues["IndexB"], (int)propertyValues["IndexC"], (int)propertyValues["MaterialID"]);
        }
#endif
    }
}
