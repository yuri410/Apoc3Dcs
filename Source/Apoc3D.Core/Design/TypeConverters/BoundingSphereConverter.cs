﻿using System;
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

    public class BoundingSphereConverter : MathTypeConverter
    {
#if !XBOX

        // Methods
        public BoundingSphereConverter()
        {
            Type type = typeof(BoundingSphere);
            base.propertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("Center")), new FieldPropertyDescriptor(type.GetField("Radius")) }).Sort(new string[] { "Center", "Radius" });
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
            if ((destinationType == typeof(InstanceDescriptor)) && (value is BoundingSphere))
            {
                BoundingSphere sphere = (BoundingSphere)value;
                ConstructorInfo constructor = typeof(BoundingSphere).GetConstructor(new Type[] { typeof(Vector3), typeof(float) });
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { sphere.Center, sphere.Radius });
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
            return new BoundingSphere((Vector3)propertyValues["Center"], (float)propertyValues["Radius"]);
        }
        
#endif
    }

}
