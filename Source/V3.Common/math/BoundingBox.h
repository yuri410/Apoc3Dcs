
#pragma once

#include "../design/BoundingBoxConverter.h"

#include "Enums.h"
#include "Vector3.h"

using System::Runtime::InteropServices::OutAttribute;

namespace V3
{
	value class BoundingSphere;
	value class Plane;
	value class Ray;

	ref class DataStream;

	/// <summary>
	/// An axis aligned bounding box, specified by minimum and maximum vectors.
	/// </summary>
	/// <unmanaged>None</unmanaged>
	[System::Serializable]
	[System::Runtime::InteropServices::StructLayout( System::Runtime::InteropServices::LayoutKind::Sequential )]
	[System::ComponentModel::TypeConverter( V3::Design::BoundingBoxConverter::typeid )]
	public value class BoundingBox : System::IEquatable<BoundingBox>
	{
	public:
		/// <summary>
		/// The highest corner of the box.
		/// </summary>
		Vector3 Maximum;

		/// <summary>
		/// The lowest corner of the box.
		/// </summary>
		Vector3 Minimum;

		/// <summary>
		/// Initializes a new instance of the <see cref="BoundingBox"/> structure.
		/// </summary>
		/// <param name="minimum">The lowest corner of the box.</param>
		/// <param name="maximum">The highest corner of the box.</param>
		BoundingBox( Vector3 minimum, Vector3 maximum );

		/// <summary>
		/// Retrieves the eight corners of the bounding box.
		/// </summary>
		/// <returns>An array of points representing the eight corners of the bounding box.</returns>
		array<Vector3>^ GetCorners();

		/// <summary>
		/// Determines whether the box contains the specified box.
		/// </summary>
		/// <param name="box1">The first box that will be checked for containment.</param>
		/// <param name="box2">The second box that will be checked for containment.</param>
		/// <returns>A member of the <see cref="ContainmentType"/> enumeration indicating whether the two objects intersect, are contained, or don't meet at all.</returns>
		static ContainmentType Contains( BoundingBox box1, BoundingBox box2 );

		/// <summary>
		/// Determines whether the box contains the specified sphere.
		/// </summary>
		/// <param name="box">The box that will be checked for containment.</param>
		/// <param name="sphere">The sphere that will be checked for containment.</param>
		/// <returns>A member of the <see cref="ContainmentType"/> enumeration indicating whether the two objects intersect, are contained, or don't meet at all.</returns>
		static ContainmentType Contains( BoundingBox box, BoundingSphere sphere );

		/// <summary>
		/// Determines whether the box contains the specified point.
		/// </summary>
		/// <param name="box">The box that will be checked for containment.</param>
		/// <param name="vector">The point that will be checked for containment.</param>
		/// <returns>A member of the <see cref="ContainmentType"/> enumeration indicating whether the two objects intersect, are contained, or don't meet at all.</returns>
		static ContainmentType Contains( BoundingBox box, Vector3 vector );

		/// <summary>
		/// Constructs a <see cref="BoundingBox"/> that fully contains the given points.
		/// </summary>
		/// <param name="points">The points that will be contained by the box.</param>
		/// <returns>The newly constructed bounding box.</returns>
		static BoundingBox FromPoints( array<Vector3>^ points );

		/// <summary>
		/// Constructs a <see cref="BoundingBox"/> from a given sphere.
		/// </summary>
		/// <param name="sphere">The sphere that will designate the extents of the box.</param>
		/// <returns>The newly constructed bounding box.</returns>
		static BoundingBox FromSphere( BoundingSphere sphere );

		/// <summary>
		/// Constructs a <see cref="BoundingBox"/> that is the as large as the total combined area of the two specified boxes.
		/// </summary>
		/// <param name="box1">The first box to merge.</param>
		/// <param name="box2">The second box to merge.</param>
		/// <returns>The newly constructed bounding box.</returns>
		static BoundingBox Merge( BoundingBox box1, BoundingBox box2 );

		/// <summary>
		/// Determines whether a box intersects the specified object.
		/// </summary>
		/// <param name="box1">The first box which will be tested for intersection.</param>
		/// <param name="box2">The second box that will be tested for intersection.</param>
		/// <returns><c>true</c> if the two objects are intersecting; otherwise, <c>false</c>.</returns>
		static bool Intersects( BoundingBox box1, BoundingBox box2 );

		/// <summary>
		/// Determines whether a box intersects the specified object.
		/// </summary>
		/// <param name="box">The box which will be tested for intersection.</param>
		/// <param name="sphere">The sphere that will be tested for intersection.</param>
		/// <returns><c>true</c> if the two objects are intersecting; otherwise, <c>false</c>.</returns>
		static bool Intersects( BoundingBox box, BoundingSphere sphere );

		/// <summary>
		/// Determines whether a box intersects the specified object.
		/// </summary>
		/// <param name="box">The box which will be tested for intersection.</param>
		/// <param name="ray">The ray that will be tested for intersection.</param>
		/// <param name="distance">When the method completes, contains the distance from the ray's origin in which the intersection with the box occured.</param>
		/// <returns><c>true</c> if the two objects are intersecting; otherwise, <c>false</c>.</returns>
		static bool Intersects( BoundingBox box, Ray ray, [Out] float% distance );

		/// <summary>
		/// Finds the intersection between a plane and a box.
		/// </summary>
		/// <param name="box">The box to check for intersection.</param>
		/// <param name="plane">The source plane.</param>
		/// <returns>A value from the <see cref="PlaneIntersectionType"/> enumeration describing the result of the intersection test.</returns>
		static PlaneIntersectionType Intersects( BoundingBox box, Plane plane );

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator == ( BoundingBox left, BoundingBox right );

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		static bool operator != ( BoundingBox left, BoundingBox right );

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
		virtual bool Equals( BoundingBox other );

		/// <summary>
		/// Determines whether the specified object instances are considered equal. 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
		/// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
		static bool Equals( BoundingBox% value1, BoundingBox% value2 );
	};
}