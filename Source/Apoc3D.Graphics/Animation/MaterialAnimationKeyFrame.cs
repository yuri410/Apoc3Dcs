using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics.Animation
{
    /// <summary>
    ///  表示材质动画的关键帧
    /// </summary>
    public struct MaterialAnimationKeyFrame
    {
        public float Time;
        public int MaterialIndex;

        public MaterialAnimationKeyFrame(float time, int mid)
        {
            Time = time;
            MaterialIndex = mid;
        }
    }

    public class MaterialAnimationClip 
    {
        /// <summary>
        /// Gets the total length of the model animation clip
        /// </summary>
        public float Duration { get; private set; }

        public void SetDuration(float val) { Duration = val; }

        /// <summary>
        /// Gets a combined list containing all the keyframes for all bones,
        /// sorted by time.
        /// </summary>
        public List<MaterialAnimationKeyFrame> Keyframes { get; private set; }
        
        /// <summary>
        /// Constructs a new model animation clip object.
        /// </summary>
        public MaterialAnimationClip(float duration, List<MaterialAnimationKeyFrame> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }
    }
}
