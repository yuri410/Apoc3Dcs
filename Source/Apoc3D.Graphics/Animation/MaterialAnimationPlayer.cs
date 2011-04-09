using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Apoc3D.Graphics.Animation
{
    class MaterialAnimationPlayer
    {
        private int MaxKeyframeIndex;

        //Current key frame 
        private float intervalPerFrame;

        // Current frame index
        private int currentIndex;
    
        // Speed of playback
        float playbackRate = 1.0f;

        bool isLoop;

        // Amount of time elapsed while playing
        float elapsedTime;

        // Whether or not playback is paused
        bool paused;

        int back = 1;

        public int FrameCount 
        {
            get { return MaxKeyframeIndex; }
        }
        public int CurrentFrameIndex
        {
            get { return currentIndex; }
        }

        /// <summary>
        /// Invoked when playback has completed.
        /// </summary>
        public event EventHandler Completed;

        public MaterialAnimationPlayer(float interval, int frameCount)
        {
            MaxKeyframeIndex = frameCount;
            this.intervalPerFrame = interval;
            currentIndex = 0;
            elapsedTime = 0;
            isLoop = false;
        }

        public void StartAnimation(int startIndex, float playbackRate, int playDirection)
        {
            this.back = Math.Sign(playDirection);
            this.currentIndex = startIndex;
            this.playbackRate = playbackRate;
            this.elapsedTime = 0;

            paused = false;
        }

        public void SetLoopPlayer(bool loop)
        {
            this.isLoop = loop;
        }

        public void PauseAnimation()
        {
            paused = true;
        }

        public void ResumeAnimation()
        {
            paused = false;
        }


        public void Update(GameTime gameTime)
        {
            if(paused)
                return;

            TimeSpan time = gameTime.ElapsedGameTime;

            // Adjust for the rate
            if (playbackRate != 1.0f)
                time = TimeSpan.FromMilliseconds(time.TotalMilliseconds * playbackRate);

            float dt = (float)time.TotalSeconds;

            elapsedTime += dt;

            if (elapsedTime >= intervalPerFrame)
            {
                currentIndex += 1 * (back);
                
                if( currentIndex >= MaxKeyframeIndex  || currentIndex <= 0 )
                {
                    if (isLoop)
                    {
                        back *= -1;
                    }
                    else
                    {
                        if (Completed != null)
                            Completed(this, EventArgs.Empty);
                        return;
                    }
                    
                }
                elapsedTime = 0.0f;    //重新计时
             }
             

        }

    }
}
