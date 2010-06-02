﻿/*
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
using System.Diagnostics;
using System.Text;

namespace Apoc3D.Graphics
{
    public class FpsCounter
    {
        long begin, end;
        float fps = -1;

        long stat;
        float multiplier = 1;

        int statMax = 100;

        Stopwatch sw;

        public FpsCounter()
        {
            sw = Stopwatch.StartNew();
            SetBegin();
        }

        public int StepFrame
        {
            get { return statMax; }
            set { statMax = value; }
        }
        public float FPS
        {
            get { return fps; }
        }
        public float Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        public void Update()
        {
            stat++;

            if (stat >= statMax)
            {
                end = sw.ElapsedMilliseconds;

                fps = 1000.0f * ((float)stat / (float)(end - begin));
                stat = 0;
                SetBegin();
            }
        }
        void SetBegin()
        {
            begin = sw.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            return "FPS= " + System.Math.Round(fps * multiplier, 2).ToString();
        }
    }
}
