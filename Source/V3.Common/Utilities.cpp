#include "stdafx.h"


#include "Utilities.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Globalization;

namespace V3
{
	Utilities::Utilities()
	{
	}

	generic<typename T> where T : value class
	array<T>^ Utilities::ReadRange( ID3DXBuffer *buffer, int count )
	{
		if( count < 0 )
			throw gcnew ArgumentOutOfRangeException( "count" );
			
		size_t elementSize = sizeof(T);
		array<T>^ result = gcnew array<T>( count );

		pin_ptr<T> pinnedBuffer = &result[0];
		memcpy( pinnedBuffer, buffer->GetBufferPointer(), count * elementSize );

		return result;
	}
	

	Guid Utilities::ConvertNativeGuid( const GUID &guid )
	{
		if( guid == GUID_NULL )
			return Guid::Empty;

		Guid result(
			guid.Data1,
			guid.Data2,
			guid.Data3,
			guid.Data4[0],
			guid.Data4[1],
			guid.Data4[2], 
			guid.Data4[3],
			guid.Data4[4],
			guid.Data4[5],
			guid.Data4[6],
			guid.Data4[7]
		);

		return result;
	}

	GUID Utilities::ConvertManagedGuid( Guid guid )
	{
		if( guid == Guid::Empty )
			return GUID_NULL;

		GUID result;
		array<Byte>^ bytes = guid.ToByteArray();
		pin_ptr<unsigned char> pinned_bytes = &bytes[0];
		memcpy( &result, pinned_bytes, sizeof(GUID) );

		return result;
	}
	
	int Utilities::SizeOfFormatElement( DXGI_FORMAT format )
	{
		switch( format )
		{
			case DXGI_FORMAT_R32G32B32A32_TYPELESS:
			case DXGI_FORMAT_R32G32B32A32_FLOAT:
			case DXGI_FORMAT_R32G32B32A32_UINT:
			case DXGI_FORMAT_R32G32B32A32_SINT:
				return 128;
				
			case DXGI_FORMAT_R32G32B32_TYPELESS:
			case DXGI_FORMAT_R32G32B32_FLOAT:
			case DXGI_FORMAT_R32G32B32_UINT:
			case DXGI_FORMAT_R32G32B32_SINT:
				return 96;
				
			case DXGI_FORMAT_R16G16B16A16_TYPELESS:
			case DXGI_FORMAT_R16G16B16A16_FLOAT:
			case DXGI_FORMAT_R16G16B16A16_UNORM:
			case DXGI_FORMAT_R16G16B16A16_UINT:
			case DXGI_FORMAT_R16G16B16A16_SNORM:
			case DXGI_FORMAT_R16G16B16A16_SINT:
			case DXGI_FORMAT_R32G32_TYPELESS:
			case DXGI_FORMAT_R32G32_FLOAT:
			case DXGI_FORMAT_R32G32_UINT:
			case DXGI_FORMAT_R32G32_SINT:
			case DXGI_FORMAT_R32G8X24_TYPELESS:
			case DXGI_FORMAT_D32_FLOAT_S8X24_UINT:
			case DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS:
			case DXGI_FORMAT_X32_TYPELESS_G8X24_UINT:
				return 64;
			
			case DXGI_FORMAT_R10G10B10A2_TYPELESS:
			case DXGI_FORMAT_R10G10B10A2_UNORM:
			case DXGI_FORMAT_R10G10B10A2_UINT:
			case DXGI_FORMAT_R11G11B10_FLOAT:
			case DXGI_FORMAT_R8G8B8A8_TYPELESS:
			case DXGI_FORMAT_R8G8B8A8_UNORM:
			case DXGI_FORMAT_R8G8B8A8_UNORM_SRGB:
			case DXGI_FORMAT_R8G8B8A8_UINT:
			case DXGI_FORMAT_R8G8B8A8_SNORM:
			case DXGI_FORMAT_R8G8B8A8_SINT:
			case DXGI_FORMAT_R16G16_TYPELESS:
			case DXGI_FORMAT_R16G16_FLOAT:
			case DXGI_FORMAT_R16G16_UNORM:
			case DXGI_FORMAT_R16G16_UINT:
			case DXGI_FORMAT_R16G16_SNORM:
			case DXGI_FORMAT_R16G16_SINT:
			case DXGI_FORMAT_R32_TYPELESS:
			case DXGI_FORMAT_D32_FLOAT:
			case DXGI_FORMAT_R32_FLOAT:
			case DXGI_FORMAT_R32_UINT:
			case DXGI_FORMAT_R32_SINT:
			case DXGI_FORMAT_R24G8_TYPELESS:
			case DXGI_FORMAT_D24_UNORM_S8_UINT:
			case DXGI_FORMAT_R24_UNORM_X8_TYPELESS:
			case DXGI_FORMAT_X24_TYPELESS_G8_UINT:
			case DXGI_FORMAT_B8G8R8A8_UNORM:
			case DXGI_FORMAT_B8G8R8X8_UNORM:
				return 32;
				
			case DXGI_FORMAT_R8G8_TYPELESS:
			case DXGI_FORMAT_R8G8_UNORM:
			case DXGI_FORMAT_R8G8_UINT:
			case DXGI_FORMAT_R8G8_SNORM:
			case DXGI_FORMAT_R8G8_SINT:
			case DXGI_FORMAT_R16_TYPELESS:
			case DXGI_FORMAT_R16_FLOAT:
			case DXGI_FORMAT_D16_UNORM:
			case DXGI_FORMAT_R16_UNORM:
			case DXGI_FORMAT_R16_UINT:
			case DXGI_FORMAT_R16_SNORM:
			case DXGI_FORMAT_R16_SINT:
			case DXGI_FORMAT_B5G6R5_UNORM:
			case DXGI_FORMAT_B5G5R5A1_UNORM:
				return 16;
				
			case DXGI_FORMAT_R8_TYPELESS:
			case DXGI_FORMAT_R8_UNORM:
			case DXGI_FORMAT_R8_UINT:
			case DXGI_FORMAT_R8_SNORM:
			case DXGI_FORMAT_R8_SINT:
			case DXGI_FORMAT_A8_UNORM:
				return 8;
			
			// Compressed format; http://msdn2.microsoft.com/en-us/library/bb694531(VS.85).aspx
			case DXGI_FORMAT_BC2_TYPELESS:
			case DXGI_FORMAT_BC2_UNORM:
			case DXGI_FORMAT_BC2_UNORM_SRGB:
			case DXGI_FORMAT_BC3_TYPELESS:
			case DXGI_FORMAT_BC3_UNORM:
			case DXGI_FORMAT_BC3_UNORM_SRGB:
			case DXGI_FORMAT_BC5_TYPELESS:
			case DXGI_FORMAT_BC5_UNORM:
			case DXGI_FORMAT_BC5_SNORM:
				return 128;
				
			// Compressed format; http://msdn2.microsoft.com/en-us/library/bb694531(VS.85).aspx
			case DXGI_FORMAT_R1_UNORM:
			case DXGI_FORMAT_BC1_TYPELESS:
			case DXGI_FORMAT_BC1_UNORM:
			case DXGI_FORMAT_BC1_UNORM_SRGB:
			case DXGI_FORMAT_BC4_TYPELESS:
			case DXGI_FORMAT_BC4_UNORM:
			case DXGI_FORMAT_BC4_SNORM:
				return 64;
			
			// Compressed format; http://msdn2.microsoft.com/en-us/library/bb694531(VS.85).aspx
			case DXGI_FORMAT_R9G9B9E5_SHAREDEXP:
				return 32;
			
			// These are compressed, but bit-size information is unclear.
			case DXGI_FORMAT_R8G8_B8G8_UNORM:
			case DXGI_FORMAT_G8R8_G8B8_UNORM:
				return 32;

			case DXGI_FORMAT_UNKNOWN:
			default:
				throw gcnew InvalidOperationException( "Cannot determine format element size; invalid format specified." );
		}
	}
	
	Drawing::Rectangle Utilities::ConvertRect(RECT rect)
	{
		return Drawing::Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
	}

	void Utilities::CheckArrayBounds( Array^ data, int offset, int% count )
	{
		if( data == nullptr )
			throw gcnew ArgumentNullException( "data" );

		CheckBounds( 0, data->Length, offset, count );
	}
	
	void Utilities::CheckBounds( int lowerBound, int size, int offset, int% count )
	{
		if( offset < lowerBound )
			throw gcnew ArgumentOutOfRangeException( "offset" );
		if( count < 0 )
			throw gcnew ArgumentOutOfRangeException( "count" );
		if( offset + count > size )
			throw gcnew ArgumentException( "The sum of offset and count is greater than the buffer length." );
			
		if( count == 0 )
			count = size - offset;
	}
	
	generic<typename T>
	bool Utilities::CheckElementEquality( array<T>^ left, array<T>^ right )
	{
		if( left->Length != right->Length )
			return false;
		
		for( int index = 0; index < left->Length; ++index )
		{
			if( !left[index]->Equals( right[index] ) ) 
			{
				return false;
			}
		}
		
		return true;
	}
	
	generic<typename T>
	bool Utilities::CheckElementEquality( IList<T>^ left, IList<T>^ right )
	{
		if( left->Count != right->Count )
			return false;
		
		for( int index = 0; index < left->Count; ++index )
		{
			if( !left[index]->Equals( right[index] ) ) 
			{
				return false;
			}
		}
		
		return true;
	}
	
	String^ Utilities::BufferToString( ID3DXBuffer *buffer )
	{
		if( buffer != NULL )
		{
			String^ string = gcnew String( reinterpret_cast<const char*>( buffer->GetBufferPointer() ) );
			buffer->Release();
			return string;
		}
		else
		{
			return String::Empty;
		}
	}

	void Utilities::FreeNativeString( LPCSTR string )
	{
		if( string == NULL )
			return;

		System::Runtime::InteropServices::Marshal::FreeHGlobal( IntPtr( const_cast<void*>( reinterpret_cast<const void*>( string ) ) ) );
	}

	void Utilities::FreeNativeString( LPSTR string )
	{
		if( string == NULL )
			return;

		System::Runtime::InteropServices::Marshal::FreeHGlobal( IntPtr( reinterpret_cast<void*>( string ) ) );
	}

	LPSTR Utilities::AllocateNativeString( String^ value )
	{
		if( value == nullptr || String::IsNullOrEmpty( value ) )
			return NULL;
		else
			return reinterpret_cast<LPSTR>( System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi( value ).ToPointer() );
	}

	generic<typename T>
	T Utilities::FromIntToT( int value )
	{
		if( T::typeid->IsEnum )
			return safe_cast<T>( static_cast<int>( value ) );
		else if( T::typeid == float::typeid )
			return safe_cast<T>( *reinterpret_cast<float*>( &value ) );
		else
			return safe_cast<T>( Convert::ChangeType( static_cast<int>( value ), T::typeid, CultureInfo::InvariantCulture ) );
	}
}