#include "stdafx.h"

#include "../InternalHelpers.h"
#include "../math/Quaternion.h"

#include "QuaternionConverter.h"
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
	QuaternionConverter::QuaternionConverter()
	{
		Type^ type = Quaternion::typeid;
		array<PropertyDescriptor^>^ propArray =
		{
			gcnew FieldPropertyDescriptor(type->GetField("X")),
			gcnew FieldPropertyDescriptor(type->GetField("Y")),
			gcnew FieldPropertyDescriptor(type->GetField("Z")),
			gcnew FieldPropertyDescriptor(type->GetField("W")),
		};

		m_Properties = gcnew PropertyDescriptorCollection(propArray);
	}

	bool QuaternionConverter::CanConvertTo(ITypeDescriptorContext^ context, Type^ destinationType)
	{
		if( destinationType == String::typeid || destinationType == InstanceDescriptor::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertTo(context, destinationType);
	}

	bool QuaternionConverter::CanConvertFrom(ITypeDescriptorContext^ context, Type^ sourceType)
	{
		if( sourceType == String::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertFrom(context, sourceType);
	}

	Object^ QuaternionConverter::ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, Type^ destinationType)
	{
		if( destinationType == nullptr )
			throw gcnew ArgumentNullException( "destinationType" );

		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		Quaternion^ quaternion = dynamic_cast<Quaternion^>( value );

		if( destinationType == String::typeid && quaternion != nullptr )
		{
			String^ separator = culture->TextInfo->ListSeparator + " ";
			TypeConverter^ converter = TypeDescriptor::GetConverter(float::typeid);
			array<String^>^ stringArray = gcnew array<String^>( 4 );

			stringArray[0] = converter->ConvertToString( context, culture, quaternion->X );
			stringArray[1] = converter->ConvertToString( context, culture, quaternion->Y );
			stringArray[2] = converter->ConvertToString( context, culture, quaternion->Z );
			stringArray[3] = converter->ConvertToString( context, culture, quaternion->W );

			return String::Join( separator, stringArray );
		}
		else if( destinationType == InstanceDescriptor::typeid && quaternion != nullptr )
		{
			ConstructorInfo^ info = (Quaternion::typeid)->GetConstructor( gcnew array<Type^> { float::typeid, float::typeid, float::typeid, float::typeid } );
			if( info != nullptr )
				return gcnew InstanceDescriptor( info, gcnew array<Object^> { quaternion->X, quaternion->Y, quaternion->Z, quaternion->W } );
		}

		return ExpandableObjectConverter::ConvertTo(context, culture, value, destinationType);
	}

	Object^ QuaternionConverter::ConvertFrom(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value)
	{
		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		String^ string = dynamic_cast<String^>( value );

		if( string != nullptr )
		{
			string = string->Trim();
			TypeConverter^ converter = TypeDescriptor::GetConverter(float::typeid);
			array<String^>^ stringArray = string->Split( culture->TextInfo->ListSeparator[0] );

			if( stringArray->Length != 4 )
				throw gcnew ArgumentException("Invalid quaternion format.");

			float X = safe_cast<float>( converter->ConvertFromString( context, culture, stringArray[0] ) );
			float Y = safe_cast<float>( converter->ConvertFromString( context, culture, stringArray[1] ) );
			float Z = safe_cast<float>( converter->ConvertFromString( context, culture, stringArray[2] ) );
			float W = safe_cast<float>( converter->ConvertFromString( context, culture, stringArray[3] ) );

			return gcnew Quaternion(X, Y, Z, W);
		}

		return ExpandableObjectConverter::ConvertFrom(context, culture, value);
	}

	bool QuaternionConverter::GetCreateInstanceSupported(ITypeDescriptorContext^ context)
	{
		DX_UNREFERENCED_PARAMETER(context);

		return true;
	}

	Object^ QuaternionConverter::CreateInstance(ITypeDescriptorContext^ context, IDictionary^ propertyValues)
	{
		DX_UNREFERENCED_PARAMETER(context);

		if( propertyValues == nullptr )
			throw gcnew ArgumentNullException( "propertyValues" );

		return gcnew Quaternion( safe_cast<float>( propertyValues["X"] ), safe_cast<float>( propertyValues["Y"] ),
			safe_cast<float>( propertyValues["Z"] ), safe_cast<float>( propertyValues["W"] ) );
	}

	bool QuaternionConverter::GetPropertiesSupported(ITypeDescriptorContext^)
	{
		return true;
	}

	PropertyDescriptorCollection^ QuaternionConverter::GetProperties(ITypeDescriptorContext^, Object^, array<Attribute^>^)
	{
		return m_Properties;
	}
}
}