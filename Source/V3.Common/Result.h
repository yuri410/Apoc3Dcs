
#pragma once

namespace V3
{
	public ref class ResultInfo
	{
	public:
		System::String^ Name;
		System::String^ Description;
		System::Collections::SortedList^ Data;
	};

	/// <summary>
	/// Represents the result of a method or operation.
	/// </summary>
	/// <unmanaged href="ms679692">HRESULT</unmanaged>
	public value class Result : System::IEquatable<Result>
	{
		int m_Code;
		ResultInfo^ m_Info;
	
		[System::ThreadStatic]
		static Result m_Last;
	
		[System::ThreadStatic]
		static int m_LastCode;

		generic< typename T >
		static void Throw( Object^ dataKey, Object^ dataValue );

	internal:
		Result( int hr );
		
		[System::Diagnostics::Conditional( "DEBUG" )]
		static void BreakIfDebugging();

		generic< typename T >
		static Result Fail( int hr, Object^ dataKey, Object^ dataValue );

		generic< typename T >
		static Result Record( int hr, Object^ dataKey, Object^ dataValue );

		generic< typename T >
		static Result Record( int hr, bool failed, Object^ dataKey, Object^ dataValue );

	public:
		/// <summary>
		/// Gets the actual HRESULT result code.
		/// </summary>
		property int Code
		{
			int get();
		};
		
		/// <summary>
		/// Gets the name of the result.
		/// </summary>
		property System::String^ Name
		{
			System::String^ get();
		};

		/// <summary>
		/// Gets the friendly description of the result.
		/// </summary>
		property System::String^ Description
		{
			System::String^ get();
		};
		
		property System::Collections::SortedList^ Data
		{
			System::Collections::SortedList^ get();
			void set( System::Collections::SortedList^ value );
		}

		/// <summary>
		/// Gets a value indicating whether or not the result represents a successful operation.
		/// </summary>
		property bool IsSuccess
		{
			bool get();
		};
		
		/// <summary>
		/// Gets a value indicating whether or not the result represents a failed operation.
		/// </summary>
		property bool IsFailure
		{
			bool get();
		};
	
		/// <summary>
		/// Gets the last recorded result of a method or operation.
		/// </summary>
		static property Result Last
		{
			Result get();
		};
		
		/// <summary>
		/// Tests for equality between two results.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator == ( Result left, Result right );

		/// <summary>
		/// Tests for inequality between two results.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator != ( Result left, Result right );

		/// <summary>
		/// Converts the value of the result to its equivalent string representation.
		/// </summary>
		/// <returns>The string representation of the value of this instance.</returns>
		virtual System::String^ ToString() override;

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		virtual int GetHashCode() override;

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns><c>true</c> if <paramref name="obj"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
		virtual bool Equals( System::Object^ obj ) override;

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified result.
		/// </summary>
		/// <param name="other">A <see cref="Result"/> to compare with this instance.</param>
		/// <returns><c>true</c> if <paramref name="other"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
		virtual bool Equals( Result other );

		/// <summary>
		/// Returns a value indicating whether the two results are equivalent.
		/// </summary>
		/// <param name="value1">The first value to compare.</param>
		/// <param name="value2">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="value1"/> has the same value as <paramref name="value2"/>; otherwise, <c>false</c>.</returns>
		static bool Equals( Result% value1, Result% value2 );
	};
}