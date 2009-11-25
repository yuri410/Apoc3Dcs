﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using VirtualBicycle.MathLib;

namespace VirtualBicycle.Design
{
#if !XBOX
    using System.ComponentModel.Design.Serialization;
#endif

    public class QuaternionConverter : MathTypeConverter
    {
#if !XBOX
        // Methods
        public QuaternionConverter()
        {
            Type type = typeof(Quaternion);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("X")), new FieldPropertyDescriptor(type.GetField("Y")), new FieldPropertyDescriptor(type.GetField("Z")), new FieldPropertyDescriptor(type.GetField("W")) }).Sort(new string[] { "X", "Y", "Z", "W" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float[] numArray = MathTypeConverter.ConvertToValues<float>(context, culture, value, 4, "X, Y, Z, W");
            if (numArray != null)
            {
                return new Quaternion(numArray[0], numArray[1], numArray[2], numArray[3]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Quaternion))
            {
                Quaternion quaternion2 = (Quaternion)value;
                return MathTypeConverter.ConvertFromValues<float>(context, culture, new float[] { quaternion2.X, quaternion2.Y, quaternion2.Z, quaternion2.W });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Quaternion))
            {
                Quaternion quaternion = (Quaternion)value;
                ConstructorInfo constructor = typeof(Quaternion).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { quaternion.X, quaternion.Y, quaternion.Z, quaternion.W });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        
#endif
    }
}
