
#pragma once

#include "../design/HalfConverter.h"
#include <d3dx9.h>

namespace V3
{
	/// <summary>
	/// A half precision (16 bit) floating point value.
	/// </summary>
	/// <unmanaged>D3DXFLOAT16</unmanaged>
	[System::Serializable]
	[System::Runtime::InteropServices::StructLayout(System::Runtime::InteropServices::LayoutKind::Sequential)]
	[System::ComponentModel::TypeConverter( V3::Design::HalfConverter::typeid )]
	public value class Half
	{
	private:
		System::UInt16 m_Value;

		static Half()
		{
			Half::Epsilon = D3DX_16F_EPSILON;
			Half::MaxValue = D3DX_16F_MAX;
			Half::MinValue = D3DX_16F_MIN;
		}

	public:
		/// <summary>
		/// Number of decimal digits of precision.
		/// </summary>
		literal int PrecisionDigits = D3DX_16F_DIG;

		/// <summary>
		/// Number of bits in the mantissa.
		/// </summary>
		literal int MantissaBits = D3DX_16F_MANT_DIG;

		/// <summary>
		/// Maximum decimal exponent.
		/// </summary>
		literal int MaximumDecimalExponent = D3DX_16F_MAX_10_EXP;

		/// <summary>
		/// Maximum binary exponent.
		/// </summary>
		literal int MaximumBinaryExponent = D3DX_16F_MAX_EXP;

		/// <summary>
		/// Minimum decimal exponent.
		/// </summary>
		literal int MinimumDecimalExponent = D3DX_16F_MIN_10_EXP;

		/// <summary>
		/// Minimum binary exponent.
		/// </summary>
		literal int MinimumBinaryExponent = D3DX_16F_MIN_EXP;

		/// <summary>
		/// Exponent radix.
		/// </summary>
		literal int ExponentRadix = D3DX_16F_RADIX;

		/// <summary>
		/// Additional rounding.
		/// </summary>
		literal int AdditionRounding = D3DX_16F_ROUNDS;

		/// <summary>
		/// Smallest such that 1.0 + epsilon != 1.0
		/// </summary>
		static initonly float Epsilon;

		/// <summary>
		/// Maximum value of the number.
		/// </summary>
		static initonly float MaxValue;

		/// <summary>
		/// Minimum value of the number.
		/// </summary>
		static initonly float MinValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="Half"/> structure.
		/// </summary>
		/// <param name="value">The floating point value that should be stored in 16 bit format.</param>
		Half( float value );

		/// <summary>
		/// Converts an array of half precision values into full precision values.
		/// </summary>
		/// <param name="values">The values to be converted.</param>
		/// <returns>An array of converted values.</returns>
		static array<float>^ ConvertToFloat( array<Half>^ values );

		/// <summary>
		/// Converts an array of full precision values into half precision values.
		/// </summary>
		/// <param name="values">The values to be converted.</param>
		/// <returns>An array of converted values.</returns>
		static array<Half>^ ConvertToHalf( array<float>^ values );

		/// <summary>
		/// Performs an explicit conversion from <see cref="System::Single"/> to <see cref="Half"/>.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		/// <returns>The converted value.</returns>
		static explicit operator Half( float value );

		/// <summary>
		/// Performs an implicit conversion from <see cref="Half"/> to <see cref="System::Single"/>.
		/// </summary>
		/// <param name="value">The value to be converted.</param>
		/// <returns>The converted value.</returns>
		static operator float( Half value );

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator == ( Half left, Half right );

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator != ( Half left, Half right );

		/// <summary>
		/// Converts the value of the object to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		virtual System::String^ ToString() override;

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		virtual int GetHashCode() override;

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to a specified object. 
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		/// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
		virtual bool Equals( System::Object^ obj ) override;

		/// <summary>
		/// Returns a value that indicates whether the current instance is equal to the specified object. 
		/// </summary>
		/// <param name="other">Object to make the comparison with.</param>
		/// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
		virtual bool Equals( Half other );

		/// <summary>
		/// Determines whether the specified object instances are considered equal. 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
		/// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
		static bool Equals( Half% value1, Half% value2 );
	};
}