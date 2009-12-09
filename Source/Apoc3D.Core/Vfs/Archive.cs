using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Apoc3D.Vfs
{
    /// <summary>
    ///  为文件包定义抽象基类
    /// </summary>
    public abstract class Archive : FileBase
    {
        protected Archive(string file, int size, bool isinArchive)
            : base(file, size, isinArchive)
        { }


        public abstract int FileCount
        {
            get;
        }

        public abstract Stream GetEntryStream(string file);

        public abstract void Dispose(bool disposing);

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);

                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }
        
    }


    /// <summary>
    ///  为文件包定义抽象工厂
    /// </summary>
    public abstract class ArchiveFactory : IAbstractFactory<Archive, FileLocation>
    {

        #region IAbstractFactory<Archive,FileLocation> 成员

        public abstract Archive CreateInstance(string file);

        public abstract Archive CreateInstance(FileLocation fl);

        public abstract string Type
        {
            get;
        }

        #endregion
    }


    
}
