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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Scene
{
    public class ClusterTableBase<CType> : IEnumerable<CType>
         where CType : Block
    {
        class Enumrator : IEnumerator<CType>
        {
            Dictionary<ClusterDescription, CType>.ValueCollection.Enumerator enu;
            Dictionary<ClusterDescription, CType> dictionary;

            public Enumrator(Dictionary<ClusterDescription, CType> dictionary)
            {
                this.dictionary = dictionary;
                this.enu = dictionary.Values.GetEnumerator();
            }

            #region IEnumerator<CType> 成员

            public CType Current
            {
                get { return enu.Current; }
            }

            #endregion

            #region IDisposable 成员

            public void Dispose()
            {
                enu.Dispose();
            }

            #endregion

            #region IEnumerator 成员

            object System.Collections.IEnumerator.Current
            {
                get { return enu.Current; }
            }

            public bool MoveNext()
            {
                return enu.MoveNext();
            }

            public void Reset()
            {
                this.enu = dictionary.Values.GetEnumerator();
            }

            #endregion
        }

        Dictionary<ClusterDescription, CType> hashTable;

        public ClusterTableBase(CType[] clusters)
        {
            hashTable = new Dictionary<ClusterDescription, CType>(clusters.Length);

            for (int i = 0; i < clusters.Length; i++)
            {
                ClusterDescription desc = clusters[i].Description;

                hashTable.Add(desc, clusters[i]);
            }
        }

        #region 方法

        /// <summary>
        ///  尝试查询一个Cluster并返回。如果没找到返回null
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public CType TryGetCluster(ClusterDescription desc)
        {
            CType res;
            if (hashTable.TryGetValue(desc, out res))
            {
                return res;
            }
            return null;
        }

        #endregion

        #region 属性

        public CType this[ClusterDescription desc]
        {
            get { return hashTable[desc]; }
        }

        public int Count
        {
            get { return hashTable.Count; }
        }

        #endregion

        #region IEnumerable<CType> 成员

        public IEnumerator<CType> GetEnumerator()
        {
            return new Enumrator(hashTable);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumrator(hashTable);
        }

        #endregion
    }
}
