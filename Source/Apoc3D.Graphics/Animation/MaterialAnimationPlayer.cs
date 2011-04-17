using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics.Animation
{
    class MaterialAnimationPlayer
    {
        // Clip currently being played
        MaterialAnimationClip currentClipValue;

        // Current timeindex and keyframe in the clip
        float currentTimeValue;
        int currentKeyframe;

        // Speed of playback
        float playbackRate = 1.0f;

        // The amount of time for which the animation will play.
        // TimeSpan.MaxValue will loop forever. TimeSpan.Zero will play once. 
        float duration = float.MaxValue;

        // Amount of time elapsed while playing
        float elapsedPlaybackTime = 0;

        // Whether or not playback is paused
        bool paused;

        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public MaterialAnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        /// <summary>
        /// Get/Set the current key frame index
        /// </summary>
        public int CurrentKeyFrame
        {
            get { return currentKeyframe; }
            set
            {
                List<MaterialAnimationKeyFrame> keyframes = currentClipValue.Keyframes;
                float time = keyframes[value].Time;
                CurrentTimeValue = time;
            }
        }

        /// <summary>
        /// Gets/set the current play position.
        /// </summary>
        public float CurrentTimeValue
        {
            get { return currentTimeValue; }
            set
            {
                float time = value;

                // If the position moved backwards, reset the keyframe index.
                if (time < currentTimeValue)
                {
                    currentKeyframe = 0;
                    InitClip();
                }

                currentTimeValue = time;

                // Read keyframe matrices.
                IList<MaterialAnimationKeyFrame> keyframes = currentClipValue.Keyframes;


                while (currentKeyframe < keyframes.Count)
                {
                    MaterialAnimationKeyFrame keyframe = keyframes[currentKeyframe];

                    // Stop when we've read up to the current time position.                    
                    if (keyframe.Time > currentTimeValue)
                    {
                        break;
                    }

                    // Use this keyframe
                    SetKeyframe(keyframe);

                    currentKeyframe++;
                }

            }
        }

        /// <summary>
        /// Invoked when playback has completed.
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>        
        public void StartClip(MaterialAnimationClip clip)
        {
            StartClip(clip, 1.0f, 0);
        }


        public int CurrentFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Starts playing a clip
        /// </summary>
        /// <param name="clip">Animation clip to play</param>
        /// <param name="playbackRate">Speed to playback</param>
        /// <param name="duration">Length of time to play (max is looping, 0 is once)</param>
        public void StartClip(MaterialAnimationClip clip, float playbackRate, float duration)
        {
            if (clip == null)
                throw new ArgumentNullException("Clip required");

            // Store the clip and reset playing data            
            currentClipValue = clip;
            currentKeyframe = 0;
            CurrentTimeValue = 0;
            elapsedPlaybackTime = 0;
            paused = false;

            // Store the data about how we want to playback
            this.playbackRate = playbackRate;
            this.duration = duration;

            // Call the virtual to allow initialization of the clip
            InitClip();
        }

        /// <summary>
        /// Will pause the playback of the current clip
        /// </summary>
        public void PauseClip()
        {
            paused = true;
        }

        /// <summary>
        /// Will resume playback of the current clip
        /// </summary>
        public void ResumeClip()
        {
            paused = false;
        }

        void InitClip()
        {
        }

        void SetKeyframe(MaterialAnimationKeyFrame keyframe)
        {
            CurrentFrame = keyframe.MaterialIndex;
        }

        void OnUpdate()
        {
        }

        /// <summary>
        /// Called during the update loop to move the animation forward
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (currentClipValue == null)
                return;

            if (paused)
                return;


            float time = gameTime.ElapsedGameTimeSeconds;

            // Adjust for the rate
            if (playbackRate != 1.0f)
                time *= playbackRate;

            elapsedPlaybackTime += time;

            // See if we should terminate
            if (elapsedPlaybackTime >= duration && duration > float.Epsilon ||
                elapsedPlaybackTime >= currentClipValue.Duration && duration < float.Epsilon)
            {
                if (Completed != null)
                    Completed(this, EventArgs.Empty);

                currentClipValue = null;

                return;
            }

            // Update the animation position.        
            time += currentTimeValue;

            // If we reached the end, loop back to the start.
            while (time >= currentClipValue.Duration)
                time -= currentClipValue.Duration;

            CurrentTimeValue = time;

            OnUpdate();
        }
    }
}
