/*
-----------------------------------------------------------------------------
This source file is part of Apoc3D Engine

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;

namespace Apoc3D.MathLib
{
    using T = System.Single;

    public class ValueSmoother
    {
        FastQueue<T> valueQueue = new FastQueue<T>();
        int smoothSize;



        public int SmoothSize 
        {
            get { return smoothSize; }
        }

        public ValueSmoother(int smSize) 
        {
            smoothSize = smSize;
        }

        public void Add(T value) 
        {
            valueQueue.Enqueue(value);
            while (valueQueue.Count > smoothSize)
                valueQueue.Dequeue();
        }
        public void Clear ()
        {
            valueQueue.Clear();
        }
        public T Result 
        {
            get 
            {
                if (valueQueue.Count == 0)
                    return 0;

                T result = default(T);
                for (int i = 0; i < valueQueue.Count; i++)
                {
                    result += valueQueue.GetElement(i);
                }
                result /= valueQueue.Count;
                return result;
            }
        }

        public bool BoolResult 
        {
            get 
            {
                float r = Result;
                if (r > 0.5f)
                    return true;
                return false;
            }
        }
    }
}
