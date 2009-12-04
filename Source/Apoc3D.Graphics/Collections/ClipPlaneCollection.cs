using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;

namespace Apoc3D.Graphics.Collections
{
    public class ClipPlaneCollection : CollectionBase<ClipPlane>
    {
        public ClipPlaneCollection(ClipPlane[] planes)
            : base(planes.Length)
        {
            Add(planes);
        }

        public void EnableAll()
        {
            for (int i = 0; i < Count; i++) 
            {
                this[i].Enabled = true;
            }
        }
        public void DisableAll() 
        {
            for (int i = 0; i < Count; i++) 
            {
                this[i].Enabled = false;
            }
        }

    }
}