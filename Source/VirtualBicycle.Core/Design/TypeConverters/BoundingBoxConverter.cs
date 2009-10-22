﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Text;
using VirtualBicycle.MathLib;

namespace VirtualBicycle.Design
{
    public class BoundingBoxConverter : MathTypeConverter
    {
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
    }

 

}
