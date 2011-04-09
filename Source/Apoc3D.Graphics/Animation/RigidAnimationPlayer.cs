

using System;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    /// <summary>
    /// This animation player knows how to play an animation on a rigid model, applying transformations
    /// to each of the objects in the model over time
    /// </summary>
    public class RigidAnimationPlayer : ModelAnimationPlayerBase
    {        
        // This is an array of the transforms to each object in the model
        Matrix[] boneTransforms;        
               
        /// <summary>
        /// Create a new rigid animation player
        /// </summary>
        /// <param name="count">Number of bones (objects) in the model</param>
        public RigidAnimationPlayer(int count)
        {
            if (count <= 0)
                throw new Exception("Bad arguments to model animation player");
            
            this.boneTransforms = new Matrix[count];
        }

        /// <summary>
        /// Initializes all the bone transforms to the identity
        /// </summary>
        protected override void InitClip()
        {
            for (int i = 0; i < this.boneTransforms.Length; i++)
                this.boneTransforms[i] = Matrix.Identity;
        }

        /// <summary>
        /// Sets the key frame for a bone to a transform
        /// </summary>
        /// <param name="keyframe">Keyframe to set</param>
        protected override void SetKeyframe(ModelKeyframe keyframe)
        {
            this.boneTransforms[keyframe.Bone] = keyframe.Transform;
        }

        /// <summary>
        /// Gets the current bone transform matrices for the animation
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }

        public override Matrix GetTransform(int boneId)
        {
            if (boneTransforms != null && boneId != -1)
                return boneTransforms[boneId];
            return Matrix.Identity;
        }
    }
}
