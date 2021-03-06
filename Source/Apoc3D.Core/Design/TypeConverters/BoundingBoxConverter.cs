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

namespace Apoc3D.Design
{
#if !XBOX
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using Apoc3D.MathLib;
#endif

    public class BoundingBoxConverter : MathTypeConverter
    {
#if !XBOX

        // Methods
        public BoundingBoxConverter()
        {
            Type type = typeof(BoundingBox);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("Min")), new FieldPropertyDescriptor(type.GetField("Max")) }).Sort(new string[] { "Min", "Max" });
            base.supportStringConvert = false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is BoundingBox))
            {
                BoundingBox box = (BoundingBox)value;
                ConstructorInfo constructor = typeof(BoundingBox).GetConstructor(new Type[] { typeof(Vector3), typeof(Vector3) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { box.Minimum, box.Maximum });
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
            return new BoundingBox((Vector3)propertyValues["Min"], (Vector3)propertyValues["Max"]);
        }
#endif
    }

}
