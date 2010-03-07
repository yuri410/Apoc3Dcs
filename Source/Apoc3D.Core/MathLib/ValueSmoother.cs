using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;

namespace Apoc3D.MathLib
{
    using T = System.Single;

    class ValueSmoother
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

        public T Result 
        {
            get 
            {
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
