#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Apoc3D.MathLib;


namespace Apoc3D.Graphics.Animation
{
    /// <summary>
    /// Combines all the data needed to render and animate a skinned object.
    /// This is typically stored in the Tag property of the Model being animated.
    /// </summary>
    public class AnimationData
    {
        /// <summary>
        /// Gets a collection of animation clips that operate on the root of the object.
        /// These are stored by name in a dictionary, so there could for instance be 
        /// clips for "Walk", "Run", "JumpReallyHigh", etc.
        /// </summary>
        public Dictionary<string, ModelAnimationClip> RootAnimationClips { get; private set; }

        /// <summary>
        /// Gets a collection of model animation clips. These are stored by name in a
        /// dictionary, so there could for instance be clips for "Walk", "Run",
        /// "JumpReallyHigh", etc.
        /// </summary>
        public Dictionary<string, ModelAnimationClip> ModelAnimationClips { get; private set; }

        /// <summary>
        /// Bindpose matrices for each bone in the skeleton,
        /// relative to the parent bone.
        /// </summary>
        public List<Matrix> BindPose { get; private set; }

        /// <summary>
        /// Vertex to bonespace transforms for each bone in the skeleton.
        /// </summary>
        public List<Matrix> InverseBindPose { get; private set; }

        /// <summary>
        /// For each bone in the skeleton, stores the index of the parent bone.
        /// </summary>
        public List<int> SkeletonHierarchy { get; private set; }

        /// <summary>
        /// Constructs a new skinning data object.
        /// </summary>
        public AnimationData(
            Dictionary<string, ModelAnimationClip> modelAnimationClips,
            Dictionary<string, ModelAnimationClip> rootAnimationClips,
            List<Matrix> bindPose,
            List<Matrix> inverseBindPose,
            List<int> skeletonHierarchy)
        {
            ModelAnimationClips = modelAnimationClips;
            RootAnimationClips = rootAnimationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }

    }
}
