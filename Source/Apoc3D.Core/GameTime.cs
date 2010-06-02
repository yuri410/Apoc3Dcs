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
namespace Apoc3D
{
    /// <summary>
    /// Contains the current timing state of the game.
    /// </summary>
    public class GameTime
    {
		private float m_FramesPerSecond;
        /// <summary>
        /// Gets the current frames-per-second measure.
        /// </summary>
        /// <value>The current frames-per-second measure.</value>
        public float FramesPerSecond
        {
			get { return m_FramesPerSecond; }
            protected set { m_FramesPerSecond = value; }
        }

        private TimeSpan m_ElapsedGameTime;
        /// <summary>
        /// Gets the elapsed game time, in seconds.
        /// </summary>
        /// <value>The elapsed game time.</value>
        public TimeSpan ElapsedGameTime
        {
			get { return m_ElapsedGameTime; }
            protected set
            {
                m_ElapsedGameTime = value;
                ElapsedGameTimeSeconds = (float)value.TotalSeconds;
            }
        }
        public float ElapsedGameTimeSeconds
        {
            get;
            private set;
        }
		private float m_ElapsedRealTime;
        /// <summary>
        /// Gets the elapsed real time, in seconds.
        /// </summary>
        /// <value>The elapsed real time.</value>
        public float ElapsedRealTime
        {
            get { return m_ElapsedRealTime; }
            protected set { m_ElapsedRealTime = value; }
        }
      

        private TimeSpan m_TotalGameTime;
        /// <summary>
        /// Gets the total game time, in seconds.
        /// </summary>
        /// <value>The total game time.</value>
        public TimeSpan TotalGameTime
        {
            get { return m_TotalGameTime; }
            protected set { m_TotalGameTime = value; }
        }
        

        private float m_TotalRealTime;
        /// <summary>
        /// Gets the total real time, in seconds.
        /// </summary>
        /// <value>The total real time.</value>
        public float TotalRealTime
        {
            get { return m_TotalRealTime; }
            protected set { m_TotalRealTime = value; }
        }

        private bool m_IsRunningSlowly;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is running slowly.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running slowly; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningSlowly
        {
            get { return m_IsRunningSlowly; }
            protected set { m_IsRunningSlowly = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTime"/> class.
        /// </summary>
        public GameTime()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTime"/> class.
        /// </summary>
        /// <param name="totalRealTime">The total real time.</param>
        /// <param name="elapsedRealTime">The elapsed real time.</param>
        /// <param name="totalGameTime">The total game time.</param>
        /// <param name="elapsedGameTime">The elapsed game time.</param>
        public GameTime(float totalRealTime, float elapsedRealTime, TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            TotalRealTime = totalRealTime;
            ElapsedRealTime = elapsedRealTime;
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
        }

        public void SetElapsedGameTime(TimeSpan v)
        {
            ElapsedGameTime = v;
        }
        public void SetElapsedRealTime(float v)
        {
            ElapsedRealTime = v;
        }

        public void SetFramesPerSecond(float v)
        {
            FramesPerSecond = v;
        }
        public void SetIsRunningSlowly(bool v)
        {
            IsRunningSlowly = v;
        }
        public void SetTotalGameTime(TimeSpan v)
        {
            TotalGameTime = v;
        }
        public void SetTotalRealTime(float v)
        {
            TotalRealTime = v;
        }
    }
}
