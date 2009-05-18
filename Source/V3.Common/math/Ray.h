
#pragma once

#include "../design/RayConverter.h"

#include "Vector3.h"

using System::Runtime::InteropServices::OutAttribute;

namespace V3
{
	value class BoundingBox;
	value class BoundingSphere;
	value class Plane;

	/// <summary>
	/// Defines a ray in three dimensions, specified by a starting position and a direction.
	/// </summary>
	[System::Serializable]
	[System::Runtime::InteropServices::StructLayout( System::Runtime::InteropServices::LayoutKind::Sequential )]
	[System::ComponentModel::TypeConverter( V3::Design::RayConverter::typeid )]
	public value class Ray : System::IEquatable<Ray>
	{
	public:
		/// <summary>
		/// Specifies the location of the ray's origin.
		/// </summary>
		Vector3 Position;

		/// <summary>
		/// A unit vector specifying the direction in which the ray is pointing.
		/// </summary>
		Vector3 Direction;

		/// <summary>
		/// Initializes a new instance of the <see cref="Ray"/> structure.
		/// </summary>
		/// <param name="position">The location of the ray's origin.</param>
		/// <param name="direction">A unit vector specifying the direction in which the ray is pointing.</param>
		Ray( Vector3 position, Vector3 direction );

		/// <summary>
		/// Determines whether a ray intersects the specified object.
		/// </summary>
		/// <param name="ray">The ray which will be tested for intersection.</param>
		/// <param name="plane">A plane that will be tested for intersection.</param>
		/// <param name="distance">When the method completes, contains the distance at which the ray intersected the plane.</param>
		/// <returns><c>true</c> if the ray intersects the plane; otherwise, <c>false</c>.</returns>
		static bool Intersects( Ray ray, Plane plane, [Out] float% distance );
		
		/// <summary>
		/// Determines whether a ray intersects the specified object.
		/// </summary>
		/// <param name="ray">The ray which will be tested for intersection.</param>
		/// <param name="vertex1">The first vertex of a triangle that will be tested for intersection.</param>
		/// <param name="vertex2">The second vertex of a triangle that will be tested for intersection.</param>
		/// <param name="vertex3">The third vertex of a triangle that will be tested for intersection.</param>
		/// <param name="distance">When the method completes, contains the distance at which the ray intersected the plane.</param>
		/// <returns><c>true</c> if the ray intersects the plane; otherwise, <c>false</c>.</returns>
		static bool Intersects( Ray ray, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, [Out] float% distance );
		
		/// <summary>
		/// Determines whether a ray intersects the specified object.
		/// </summary>
		/// <param name="ray">The ray which will be tested for intersection.</param>
		/// <param name="box">A box that will be tested for intersection.</param>
		/// <param name="distance">When the method completes, contains the distance at which the ray intersected the plane.</param>
		/// <returns><c>true</c> if the ray intersects the plane; otherwise, <c>false</c>.</returns>
		static bool Intersects( Ray ray, BoundingBox box, [Out] float% distance );
		
		/// <summary>
		/// Determines whether a ray intersects the specified object.
		/// </summary>
		/// <param name="ray">The ray which will be tested for intersection.</param>
		/// <param name="sphere">A sphere that will be tested for intersection.</param>
		/// <param name="distance">When the method completes, contains the distance at which the ray intersected the plane.</param>
		/// <returns><c>true</c> if the ray intersects the plane; otherwise, <c>false</c>.</returns>
		static bool Intersects( Ray ray, BoundingSphere sphere, [Out] float% distance );

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator == ( Ray left, Ray right );

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator != ( Ray left, Ray right );

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
		virtual bool Equals( Ray other );

		/// <summary>
		/// Determines whether the specified object instances are considered equal. 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
		/// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
		static bool Equals( Ray% value1, Ray% value2 );
	};
}
