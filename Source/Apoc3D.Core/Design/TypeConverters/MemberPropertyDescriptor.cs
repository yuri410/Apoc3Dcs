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
using System.Reflection;
using System.Text;

namespace Apoc3D.Design
{
    internal abstract class MemberPropertyDescriptor : PropertyDescriptor
    {

        // Fields
        private MemberInfo _member;

        // Methods
        public MemberPropertyDescriptor(MemberInfo member)
            : base(member.Name, (Attribute[])member.GetCustomAttributes(typeof(Attribute), true))
        {
            this._member = member;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            MemberPropertyDescriptor descriptor = obj as MemberPropertyDescriptor;
            return ((descriptor != null) && descriptor._member.Equals(this._member));
        }

        public override int GetHashCode()
        {
            return this._member.GetHashCode();
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        // Properties
        public override Type ComponentType
        {
            get
            {
                return this._member.DeclaringType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

    }
    internal class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        // Fields
        private PropertyInfo _property;

        // Methods
        public PropertyPropertyDescriptor(PropertyInfo property)
            : base(property)
        {
            this._property = property;
        }

        public override object GetValue(object component)
        {
            return this._property.GetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            this._property.SetValue(component, value, null);
            this.OnValueChanged(component, EventArgs.Empty);
        }

        // Properties
        public override Type PropertyType
        {
            get
            {
                return this._property.PropertyType;
            }
        }
    }

}
