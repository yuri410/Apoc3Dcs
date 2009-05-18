#include "stdafx.h"

#include "../InternalHelpers.h"
#include "../math/Rational.h"

#include "RationalConverter.h"
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
	RationalConverter::RationalConverter()
	{
		Type^ type = Rational::typeid;
		array<PropertyDescriptor^>^ propArray =
		{
			gcnew FieldPropertyDescriptor(type->GetField("Numerator")),
			gcnew FieldPropertyDescriptor(type->GetField("Denominator")),
		};

		m_Properties = gcnew PropertyDescriptorCollection(propArray);
	}

	bool RationalConverter::CanConvertTo(ITypeDescriptorContext^ context, Type^ destinationType)
	{
		if( destinationType == String::typeid || destinationType == InstanceDescriptor::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertTo(context, destinationType);
	}

	bool RationalConverter::CanConvertFrom(ITypeDescriptorContext^ context, Type^ sourceType)
	{
		if( sourceType == String::typeid )
			return true;
		else
			return ExpandableObjectConverter::CanConvertFrom(context, sourceType);
	}

	Object^ RationalConverter::ConvertTo(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value, Type^ destinationType)
	{
		if( destinationType == nullptr )
			throw gcnew ArgumentNullException( "destinationType" );

		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		Rational^ rational = dynamic_cast<Rational^>( value );

		if( destinationType == String::typeid && rational != nullptr )
		{
			String^ separator = culture->TextInfo->ListSeparator + " ";
			TypeConverter^ converter = TypeDescriptor::GetConverter(int::typeid);
			array<String^>^ stringArray = gcnew array<String^>( 2 );

			stringArray[0] = converter->ConvertToString( context, culture, rational->Numerator );
			stringArray[1] = converter->ConvertToString( context, culture, rational->Denominator );

			return String::Join( separator, stringArray );
		}
		else if( destinationType == InstanceDescriptor::typeid && rational != nullptr )
		{
			ConstructorInfo^ info = (Rational::typeid)->GetConstructor( gcnew array<Type^> { int::typeid, int::typeid } );
			if( info != nullptr )
				return gcnew InstanceDescriptor( info, gcnew array<Object^> { rational->Numerator, rational->Denominator } );
		}

		return ExpandableObjectConverter::ConvertTo(context, culture, value, destinationType);
	}

	Object^ RationalConverter::ConvertFrom(ITypeDescriptorContext^ context, CultureInfo^ culture, Object^ value)
	{
		if( culture == nullptr )
			culture = CultureInfo::CurrentCulture;

		String^ string = dynamic_cast<String^>( value );

		if( string != nullptr )
		{
			string = string->Trim();
			TypeConverter^ converter = TypeDescriptor::GetConverter(int::typeid);
			array<String^>^ stringArray = string->Split( culture->TextInfo->ListSeparator[0] );

			if( stringArray->Length != 2 )
				throw gcnew ArgumentException("Invalid rational format.");

			int Numerator = safe_cast<int>( converter->ConvertFromString( context, culture, stringArray[0] ) );
			int Denominator = safe_cast<int>( converter->ConvertFromString( context, culture, stringArray[1] ) );

			return gcnew Rational(Numerator, Denominator);
		}

		return ExpandableObjectConverter::ConvertFrom(context, culture, value);
	}

	bool RationalConverter::GetCreateInstanceSupported(ITypeDescriptorContext^ context)
	{
		DX_UNREFERENCED_PARAMETER(context);

		return true;
	}

	Object^ RationalConverter::CreateInstance(ITypeDescriptorContext^ context, IDictionary^ propertyValues)
	{
		DX_UNREFERENCED_PARAMETER(context);

		if( propertyValues == nullptr )
			throw gcnew ArgumentNullException( "propertyValues" );

		return gcnew Rational( safe_cast<int>( propertyValues["Numerator"] ), safe_cast<int>( propertyValues["Denominator"] ) );
	}

	bool RationalConverter::GetPropertiesSupported(ITypeDescriptorContext^)
	{
		return true;
	}

	PropertyDescriptorCollection^ RationalConverter::GetProperties(ITypeDescriptorContext^, Object^, array<Attribute^>^)
	{
		return m_Properties;
	}
}
}