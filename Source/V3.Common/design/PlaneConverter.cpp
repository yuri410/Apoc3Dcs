#include "stdafx.h"

#include "../InternalHelpers.h"
#include "../math/Plane.h"

#include "PlaneConverter.h"
#include "FieldPropertyDescriptor.h"

using namespace System;
using namespace System::Collections;
using namespace System::Drawing;
using namespace System::ComponentModel;
using namespace System::ComponentModel::Design::Serialization;
using namespace System::Globalization;
using namespace System::Reflection;

namespace V3
{
namespace Design
{
	PlaneConverter::PlaneConverter()
	{
		Type^ type = Plane::typeid;
		array<PropertyDescriptor^>^ propArray =
		{
			gcnew FieldPropertyDescriptor(type->GetField("Normal")),
			gcnew FieldPropertyDescriptor(type->GetField("D")),
		};

		m_Properties = gcnew PropertyDescriptorCollection(propArray);
	}

	bool PlaneConverter::CanConvertTo(ITypeDescriptorContext^ context, Type^ destinationType)
	{
		if( destinationType == String::typeid || destinationType == InstanceDescriptor::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertTo(context, destinationType);
	}

	bool PlaneConverter::CanConvertFrom(ITypeDescriptorContext^ context, Type^ sourceType)
	{
		if( sourceType == String::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertFrom(context, sourceType);
	}

	Object^ PlaneConverter::ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, Type^ destinationType)
	{
		if( destinationType == nullptr )
			throw gcnew ArgumentNullException( "destinationType" );

		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		Plane^ plane = dynamic_cast<Plane^>( value );

		if( destinationType == String::typeid && plane != nullptr )
		{
			String^ separator = culture->TextInfo->ListSeparator + " ";
			TypeConverter^ vector3Converter = TypeDescriptor::GetConverter(Vector3::typeid);
			TypeConverter^ floatConverter = TypeDescriptor::GetConverter(float::typeid);
			array<String^>^ stringArray = gcnew array<String^>( 2 );

			stringArray[0] = vector3Converter->ConvertToString( context, culture, plane->Normal );
			stringArray[1] = floatConverter->ConvertToString( context, culture, plane->D );

			return String::Join( separator, stringArray );
		}
		else if( destinationType == InstanceDescriptor::typeid && plane != nullptr )
		{
			ConstructorInfo^ info = (Plane::typeid)->GetConstructor( gcnew array<Type^> { Vector3::typeid, float::typeid } );
			if( info != nullptr )
				return gcnew InstanceDescriptor( info, gcnew array<Object^> { plane->Normal, plane->D } );
		}

		return ExpandableObjectConverter::ConvertTo(context, culture, value, destinationType);
	}

	Object^ PlaneConverter::ConvertFrom(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value)
	{
		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		String^ string = dynamic_cast<String^>( value );

		if( string != nullptr )
		{
			string = string->Trim();
			TypeConverter^ vector3Converter = TypeDescriptor::GetConverter(Vector3::typeid);
			TypeConverter^ floatConverter = TypeDescriptor::GetConverter(float::typeid);
			array<String^>^ stringArray = string->Split( culture->TextInfo->ListSeparator[0] );

			if( stringArray->Length != 2 )
				throw gcnew ArgumentException("Invalid plane format.");

			Vector3 Normal = safe_cast<Vector3>( vector3Converter->ConvertFromString( context, culture, stringArray[0] ) );
			float D = safe_cast<float>( floatConverter->ConvertFromString( context, culture, stringArray[1] ) );

			return gcnew Plane(Normal, D);
		}

		return ExpandableObjectConverter::ConvertFrom(context, culture, value);
	}

	bool PlaneConverter::GetCreateInstanceSupported(ITypeDescriptorContext^ context)
	{
		DX_UNREFERENCED_PARAMETER(context);

		return true;
	}

	Object^ PlaneConverter::CreateInstance(ITypeDescriptorContext^ context, IDictionary^ propertyValues)
	{
		DX_UNREFERENCED_PARAMETER(context);

		if( propertyValues == nullptr )
			throw gcnew ArgumentNullException( "propertyValues" );

		return gcnew Plane( safe_cast<Vector3>( propertyValues["Normal"] ), safe_cast<float>( propertyValues["D"] ) );
	}

	bool PlaneConverter::GetPropertiesSupported(ITypeDescriptorContext^)
	{
		return true;
	}

	PropertyDescriptorCollection^ PlaneConverter::GetProperties(ITypeDescriptorContext^, Object^, array<Attribute^>^)
	{
		return m_Properties;
	}
}
}