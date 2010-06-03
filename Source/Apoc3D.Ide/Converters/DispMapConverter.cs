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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using Apoc3D;
using Apoc3D.Graphics;
using Apoc3D.Ide.Designers;
using Apoc3D.Vfs;
using Apoc3D.Scene;

namespace Apoc3D.Ide.Converters
{
    public class DispMapConverter : ConverterBase
    {
        public DispMapConverter()
        {
            HeightScale = 35f / 256f;
        }

        public float HeightScale
        {
            get;
            private set;
        }

        public override void ShowDialog(object sender, EventArgs e)
        {
            string[] files;
            string path;
            if (ConvDlg.Show("", GetOpenFilter(), out files, out path) == DialogResult.OK)
            {
                ProgressDlg pd = new ProgressDlg(DevStringTable.Instance["GUI:Converting"]);

                pd.MinVal = 0;
                pd.Value = 0;
                pd.MaxVal = files.Length;

                pd.Show();
                for (int i = 0; i < files.Length; i++)
                {
                    string dest = Path.Combine(path, Path.GetFileNameWithoutExtension(files[i]) + ".dmp");

                    Convert(new DevFileLocation(files[i]), new DevFileLocation(dest));
                    pd.Value = i;
                }
                pd.Close();
                pd.Dispose();
            }
        }

        public unsafe override void Convert(ResourceLocation source, ResourceLocation dest)
        {
            Stream srcStm = source.GetStream;
            Bitmap bmp = new Bitmap(srcStm);

            srcStm.Close();

            int width = bmp.Width;
            int height = bmp.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int* src = (int*)data.Scan0.ToPointer();

            Stream fs = dest.GetStream;
            fs.SetLength(0);
            GZipStream gzipStm = new GZipStream(fs, CompressionMode.Compress);
            ContentBinaryWriter bw = new ContentBinaryWriter(gzipStm);

            bw.Write(width);
            bw.Write(height);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bw.Write((float)((src[i * width + j] & 0xff) * HeightScale) / SceneData.MaxTerrainHeight);
                }
            }


            bmp.UnlockBits(data);

            bw.Close();
        }

        public override string Name
        {
            get { return DevStringTable.Instance["GUI:DispMapConverter"]; }
        }

        public override string[] SourceExt
        {
            get { return new string[] { ".bmp", ".png" }; }
        }

        public override string[] DestExt
        {
            get { return new string[] { ".dmp" }; }
        }

        public override string SourceDesc
        {
            get { return string.Empty; }
        }

        public override string DestDesc
        {
            get { return string.Empty; }
        }
    }
}
