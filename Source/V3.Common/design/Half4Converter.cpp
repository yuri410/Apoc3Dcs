#include "stdafx.h"

#include <d3dx9.h>

#include "../InternalHelpers.h"
#include "../math/Half.h"
#include "../math/Half4.h"

#include "Half4Converter.h"
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
	Half4Converter::Half4Converter()
	{
		Type^ type = Half4::typeid;
		array<PropertyDescriptor^>^ propArray =
		{
			gcnew FieldPropertyDescriptor(type->GetField("X")),
			gcnew FieldPropertyDescriptor(type->GetField("Y")),
			gcnew FieldPropertyDescriptor(type->GetField("Z")),
			gcnew FieldPropertyDescriptor(type->GetField("W")),
		};

		m_Properties = gcnew PropertyDescriptorCollection(propArray);
	}

	bool Half4Converter::CanConvertTo(ITypeDescriptorContext^ context, Type^ destinationType)
	{
		if( destinationType == String::typeid || destinationType == InstanceDescriptor::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertTo(context, destinationType);
	}

	bool Half4Converter::CanConvertFrom(ITypeDescriptorContext^ context, Type^ sourceType)
	{
		if( sourceType == String::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertFrom(context, sourceType);
	}

	Object^ Half4Converter::ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, Type^ destinationType)
	{
		if( destinationType == nullptr )
			throw gcnew ArgumentNullException( "destinationType" );

		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		Half4^ half = dynamic_cast<Half4^>( value );

		if( destinationType == String::typeid && half != nullptr )
		{
			String^ separator = culture->TextInfo->ListSeparator + " ";
			TypeConverter^ converter = TypeDescriptor::GetConverter(Half::typeid);
			array<String^>^ stringArray = gcnew array<String^>( 4 );

			stringArray[0] = converter->ConvertToString( context, culture, half->X );
			stringArray[1] = converter->ConvertToString( context, culture, half->Y );
			stringArray[2] = converter->ConvertToString( context, culture, half->Z );
			stringArray[3] = converter->ConvertToString( context, culture, half->W );

			return String::Join( separator, stringArray );
		}
		else if( destinationType == InstanceDescriptor::typeid && half != nullptr )
		{
			ConstructorInfo^ info = (Half4::typeid)->GetConstructor( gcnew array<Type^> { Half::typeid, Half::typeid, Half::typeid, Half::typeid } );
			if( info != nullptr )
				return gcnew InstanceDescriptor( info, gcnew array<Object^> { half->X, half->Y, half->Z, half->W } );
		}

		return ExpandableObjectConverter::ConvertTo(context, culture, value, destinationType);
	}

	Object^ Half4Converter::ConvertFrom(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value)
	{
		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		String^ string = dynamic_cast<String^>( value );

		if( string != nullptr )
		{
			string = string->Trim();
			TypeConverter^ converter = TypeDescriptor::GetConverter(Half::typeid);
			array<String^>^ stringArray = string->Split( culture->TextInfo->ListSeparator[0] );

			if( stringArray->Length != 4 )
				throw gcnew ArgumentException("Invalid half format.");

			Half X = safe_cast<Half>( converter->ConvertFromString( context, culture, stringArray[0] ) );
			Half Y = safe_cast<Half>( converter->ConvertFromString( context, culture, stringArray[1] ) );
			Half Z = safe_cast<Half>( converter->ConvertFromString( context, culture, stringArray[2] ) );
			Half W = safe_cast<Half>( converter->ConvertFromString( context, culture, stringArray[3] ) );

			return gcnew Half4(X, Y, Z, W);
		}

		return ExpandableObjectConverter::ConvertFrom(context, culture, value);
	}

	bool Half4Converter::GetCreateInstanceSupported(ITypeDescriptorContext^ context)
	{
		DX_UNREFERENCED_PARAMETER(context);

		return true;
	}

	Object^ Half4Converter::CreateInstance(ITypeDescriptorContext^ context, IDictionary^ propertyValues)
	{
		DX_UNREFERENCED_PARAMETER(context);

		if( propertyValues == nullptr )
			throw gcnew ArgumentNullException( "propertyValues" );

		return gcnew Half4( safe_cast<Half>( propertyValues["X"] ), safe_cast<Half>( propertyValues["Y"] ),
			safe_cast<Half>( propertyValues["Z"] ), safe_cast<Half>( propertyValues["W"] ) );
	}

	bool Half4Converter::GetPropertiesSupported(ITypeDescriptorContext^)
	{
		return true;
	}

	PropertyDescriptorCollection^ Half4Converter::GetProperties(ITypeDescriptorContext^, Object^, array<Attribute^>^)
	{
		return m_Properties;
	}
}
}