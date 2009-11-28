﻿using System;
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

    public class Vector4Converter : MathTypeConverter
    {
#if !XBOX

        // Methods
        public Vector4Converter()
        {
            Type type = typeof(Vector4);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("X")), new FieldPropertyDescriptor(type.GetField("Y")), new FieldPropertyDescriptor(type.GetField("Z")), new FieldPropertyDescriptor(type.GetField("W")) }).Sort(new string[] { "X", "Y", "Z", "W" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float[] numArray = MathTypeConverter.ConvertToValues<float>(context, culture, value, 4, "X, Y, Z, W");
            if (numArray != null)
            {
                return new Vector4(numArray[0], numArray[1], numArray[2], numArray[3]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is Vector4))
            {
                Vector4 vector2 = (Vector4)value;
                return MathTypeConverter.ConvertFromValues<float>(context, culture, new float[] { vector2.X, vector2.Y, vector2.Z, vector2.W });
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Vector4))
            {
                Vector4 vector = (Vector4)value;
                ConstructorInfo constructor = typeof(Vector4).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { vector.X, vector.Y, vector.Z, vector.W });
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
            return new Vector4((float)propertyValues["X"], (float)propertyValues["Y"], (float)propertyValues["Z"], (float)propertyValues["W"]);
        }
#endif
    }
}
