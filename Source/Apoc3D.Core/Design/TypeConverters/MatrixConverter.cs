using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Reflection;
using System.Text;
using Apoc3D.MathLib;

namespace Apoc3D.Design
{
#if !XBOX
    using System.ComponentModel.Design.Serialization;
#endif

    public class MatrixConverter : MathTypeConverter
    {
#if !XBOX

        // Methods
        public MatrixConverter()
        {
            Type componentType = typeof(Matrix);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(componentType);
            PropertyDescriptorCollection descriptors = new PropertyDescriptorCollection(new PropertyDescriptor[] { 
            properties.Find("Translation", true), new FieldPropertyDescriptor(componentType.GetField("M11")), new FieldPropertyDescriptor(componentType.GetField("M12")), new FieldPropertyDescriptor(componentType.GetField("M13")), new FieldPropertyDescriptor(componentType.GetField("M14")), new FieldPropertyDescriptor(componentType.GetField("M21")), new FieldPropertyDescriptor(componentType.GetField("M22")), new FieldPropertyDescriptor(componentType.GetField("M23")), new FieldPropertyDescriptor(componentType.GetField("M24")), new FieldPropertyDescriptor(componentType.GetField("M31")), new FieldPropertyDescriptor(componentType.GetField("M32")), new FieldPropertyDescriptor(componentType.GetField("M33")), new FieldPropertyDescriptor(componentType.GetField("M34")), new FieldPropertyDescriptor(componentType.GetField("M41")), new FieldPropertyDescriptor(componentType.GetField("M42")), new FieldPropertyDescriptor(componentType.GetField("M43")), 
            new FieldPropertyDescriptor(componentType.GetField("M44"))
         });
            base.propertyDescriptions = descriptors;
            base.supportStringConvert = false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Matrix))
            {
                Matrix matrix = (Matrix)value;
                ConstructorInfo constructor = typeof(Matrix).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44 });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues", "BaseTexts.NullNotAllowed");
            }
            return new Matrix((float)propertyValues["M11"], (float)propertyValues["M12"], (float)propertyValues["M13"], (float)propertyValues["M14"], (float)propertyValues["M21"], (float)propertyValues["M22"], (float)propertyValues["M23"], (float)propertyValues["M24"], (float)propertyValues["M31"], (float)propertyValues["M32"], (float)propertyValues["M33"], (float)propertyValues["M34"], (float)propertyValues["M41"], (float)propertyValues["M42"], (float)propertyValues["M43"], (float)propertyValues["M44"]);
        }
#endif

    }
}
