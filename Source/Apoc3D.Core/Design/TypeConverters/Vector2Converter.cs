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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Apoc3D.MathLib;

namespace Apoc3D.Design
{
#if !XBOX
    using System.ComponentModel.Design.Serialization;
#endif

    public class Vector2Converter : MathTypeConverter
    {
#if !XBOX

        // Methods
        public Vector2Converter()
        {
            Type type = typeof(Vector2);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("X")), new FieldPropertyDescriptor(type.GetField("Y")) }).Sort(new string[] { "X", "Y" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float[] numArray = MathTypeConverter.ConvertToValues<float>(context, culture, value, 2, "X, Y");
            if (numArray != null)
            {
                return new Vector2(numArray[0], numArray[1]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Vector2))
            {
                Vector2 vector2 = (Vector2)value;
                return MathTypeConverter.ConvertFromValues<float>(context, culture, new float[] { vector2.X, vector2.Y });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Vector2))
            {
                Vector2 vector = (Vector2)value;
                ConstructorInfo constructor = typeof(Vector2).GetConstructor(new Type[] { typeof(float), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { vector.X, vector.Y });
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
            return new Vector2((float)propertyValues["X"], (float)propertyValues["Y"]);
        }
   
#endif
    }
}
