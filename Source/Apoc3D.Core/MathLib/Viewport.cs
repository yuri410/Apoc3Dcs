using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Apoc3D.MathLib
{
    /// <summary>
    /// Defines a rectangular region of a render-target surface onto which 
    /// a 3D rendering is projected.
    /// </summary>
    public struct Viewport
    {
        /// <summary>
        /// Gets or sets the viewport's X position.
        /// </summary>
        public int X;
        /// <summary>
        /// Gets or sets the viewport's Y position.
        /// </summary>
        public int Y;
        /// <summary>
        /// Gets or sets the viewport's width.
        /// </summary>
        public int Width;
        /// <summary>
        /// Gets or sets the viewport's height.
        /// </summary>
        public int Height;
        /// <summary>
        /// Gets or sets the viewport's minimum Z depth.
        /// </summary>
        public float MinZ;
        /// <summary>
        /// Gets or sets the viewport's maximum Z depth.
        /// </summary>
        public float MaxZ;


        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;
            return ((-float.Epsilon <= num) && (num <= float.Epsilon));
        }


        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Matrix matrix = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
            source.X = (((source.X - this.X) / ((float)this.Width)) * 2f) - 1f;
            source.Y = -((((source.Y - this.Y) / ((float)this.Height)) * 2f) - 1f);
            source.Z = (source.Z - this.MinZ) / (this.MaxZ - this.MinZ);
            Vector3 vector = Vector3.TransformSimple(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;

            if (!WithinEpsilon(a, 1f))
            {
                vector = vector / a;
            }
            return vector;
        }

        public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Matrix matrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            Vector3 vector = Vector3.TransformSimple(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector = vector / a;
            }
            vector.X = (((vector.X + 1f) * 0.5f) * this.Width) + this.X;
            vector.Y = (((-vector.Y + 1f) * 0.5f) * this.Height) + this.Y;
            vector.Z = (vector.Z * (this.MaxZ - this.MinZ)) + this.MinZ;
            return vector;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The X coordinate of the viewport.</param>
        /// <param name="y">The Y coordinate of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        public Viewport(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.MinZ = 0;
            this.MaxZ = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The X coordinate of the viewport.</param>
        /// <param name="y">The Y coordinate of the viewport.</param>
        /// <param name="width">The width of the viewport.</param>
        /// <param name="height">The height of the viewport.</param>
        /// <param name="minZ">The minimum Z distance of the viewport.</param>
        /// <param name="maxZ">The maximum Z distance of the viewport.</param>
        public Viewport(int x, int y, int width, int height, float minZ, float maxZ)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.MinZ = minZ;
            this.MaxZ = maxZ;
        }

        /// <summary>
        /// Tests for equality between two viewports.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Viewport left, Viewport right)
        {
            return Equals(ref left, ref right);
        }

        /// <summary>
        /// Tests for inequality between two viewports.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Viewport left, Viewport right)
        {
            return !Equals(ref left, ref right);
        }

        /// <summary>
        /// Converts the value of the viewport to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Width:{2} Height:{3} MinZ:{4} MaxZ:{5}",
                X, Y, Width, Height, MinZ, MaxZ);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Width.GetHashCode()
                 + Height.GetHashCode() + MinZ.GetHashCode() + MaxZ.GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Viewport)obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">A <see cref="Viewport"/> to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Viewport other)
        {
            return (X == other.X && Y == other.Y &&
                Width == other.Width && Height == other.Height &&
                MinZ == other.MinZ && MaxZ == other.MaxZ);
        }

        /// <summary>
        /// Returns a value indicating whether the two viewports are equivalent.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> has the same value as <paramref name="value2"/>; otherwise, <c>false</c>.</returns>
        public static bool Equals(ref Viewport value1, ref Viewport value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y && value1.Width == value2.Width
                 && value1.Height == value2.Height && value1.MinZ == value2.MinZ && value1.MaxZ == value2.MaxZ);
        }
    }
}
