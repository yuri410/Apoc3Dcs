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
using System.ComponentModel;
using System.IO;
using System.Text;
using Apoc3D.Core;
using Apoc3D.Design;
using Apoc3D.Graphics.Animation;
using Apoc3D.Vfs;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics
{
    public class AnimationCompletedEventArgs : EventArgs 
    {
        public enum AnimationType { Root, Rigid, Skinned };

        public AnimationType Type { get; private set; }

        public AnimationCompletedEventArgs(AnimationType type) 
        {
            Type = type;
        }
    }
    public delegate void AnimationCompeletedEventHandler(object sender, AnimationCompletedEventArgs e);

    /// <summary>
    ///  定义3D模型提供基础结构
    /// </summary>
    /// <typeparam name="MeshType"></typeparam>
    public abstract class ModelBase<MeshType> : Resource
        where MeshType : class
    {
        public const int MdlId = 0;

        protected static readonly string EntityCountTag = "EntityCount";
        protected static readonly string EntityPrefix = "Ent";

        static readonly string AnimationDataTag = "AnimationData";
        static readonly string RootBoneTag = "RootBone";

        static readonly string BonesTag = "Bones";
        static readonly string BoneCountTag = "BoneCount";

        static readonly string BindPoseTag = "BindPose";
        static readonly string BindPoseCountTag = "BindPoseCount";

        static readonly string InvBindPoseTag = "InvBindPose";
        static readonly string InvBindPoseCountTag = "InvBindPoseCount";

        static readonly string ModelAnimationClipTag = "ModelAnimationClip";
        static readonly string ModelAnimationClipCountTag = "ModelAnimationClipCount";

        static readonly string RootAnimationClipTag = "RootAnimationClip";
        static readonly string RootAnimationClipCountTag = "RootAnimationClipCount";

        static readonly string BoneHierarchyTag = "BoneHierarchy";
        static readonly string BoneHierarchyCountTag = "BoneHierarchyCount";

        protected static readonly string MaterialAnimationTag2 = "MaterialAnimation2.0";
        protected static readonly string MaterialAnimationDurationTag = "Duration";
        protected static readonly string MaterialAnimationKeyframesTag = "Keyframes";
        protected static readonly string MaterialAnimationKeyframeCountTag = "KeyframeCount";

        protected MeshType[] entities;
        protected Bone[] bones;
        protected int rootBone;

        protected AnimationData animData;
        protected MaterialAnimationClip materialAnimation;

        //public Dictionary<string, TapeHelper> TapeHelpers
        //{
        //    get;
        //    set;
        //}
        public Bone[] Bones 
        {
            get { return bones; }
        }
        public AnimationData Animation 
        {
            get { return animData; }
        }
        public MaterialAnimationClip MaterialAnimationClip 
        {
            get { return materialAnimation; }
        }
        public void SetMaterialAnimationClip(MaterialAnimationClip clip)
        {
            materialAnimation = clip;
        }
        public ResourceLocation DataSource
        {
            get;
            protected set;
        }

        protected ModelBase(ResourceLocation rl)
            : base(ModelManager.Instance, rl.Name)
        {
            DataSource = rl;
        }

        protected ModelBase()
        {
        }

        protected ModelBase(string name)
            : base(ModelManager.Instance, name)
        {
        }

        public MeshType[] Entities
        {
            get
            {
                Use();
                return entities;
            }
            set { entities = value; }
        }

        protected abstract MeshType LoadMesh(BinaryDataReader data);
        protected abstract BinaryDataWriter SaveMesh(MeshType mesh);

        BinaryDataWriter SaveAnimation()
        {
            BinaryDataWriter data = new BinaryDataWriter();

            #region Bones
            if (Bones != null)
            {
                data.AddEntry(BoneCountTag, Bones.Length);

                ContentBinaryWriter bw = data.AddEntry(BonesTag);
                for (int i = 0; i < Bones.Length; i++)
                {
                    bw.Write(Bones[i].Index);
                    bw.WriteStringUnicode(Bones[i].Name);
                    bw.Write(Bones[i].Transforms);

                    bw.Write(Bones[i].Parent);

                    int cldCount = Bones[i].Children.Length;
                    bw.Write(cldCount);

                    for (int j = 0; j < cldCount; j++)
                    {
                        bw.Write(Bones[i].Children[j]);
                    }

                }
                bw.Close();

                data.AddEntry(RootBoneTag, rootBone);
            }
            #endregion


            #region BindPoseTag
            List<Matrix> bindPose = animData.BindPose;

            if (bindPose != null)
            {
                data.AddEntry(BindPoseCountTag, bindPose.Count);

                ContentBinaryWriter bw = data.AddEntry(BindPoseTag);
                for (int i = 0; i < bindPose.Count; i++)
                {
                    bw.Write(bindPose[i]);
                }
                bw.Close();
            }
            #endregion

            #region InvBindPoseTag
            List<Matrix> invBindPose = animData.InverseBindPose;
            if (invBindPose != null)
            {
                data.AddEntry(InvBindPoseCountTag, invBindPose.Count);

                ContentBinaryWriter bw = data.AddEntry(InvBindPoseTag);
                for (int i = 0; i < invBindPose.Count; i++)
                {
                    bw.Write(invBindPose[i]);
                }
                bw.Close();
            }

            #endregion

            #region AnimationClipTag

            var aclip = animData.ModelAnimationClips;

            if (aclip != null)
            {
                data.AddEntry(ModelAnimationClipCountTag, aclip.Count);

                ContentBinaryWriter bw = data.AddEntry(ModelAnimationClipTag);
                foreach (var e in aclip)
                {
                    bw.WriteStringUnicode(e.Key);

                    ModelAnimationClip clip = e.Value;
                    bw.Write(clip.Duration.TotalSeconds);

                    bw.Write(clip.Keyframes.Count);

                    for (int i = 0; i < clip.Keyframes.Count; i++)
                    {
                        bw.Write(clip.Keyframes[i].Bone);
                        bw.Write(clip.Keyframes[i].Time.TotalSeconds);
                        bw.Write(clip.Keyframes[i].Transform);
                    }
                }
                bw.Close();
            }


            #endregion

            #region RootAnimationClipTag
            aclip = animData.RootAnimationClips;

            if (aclip != null)
            {
                data.AddEntry(RootAnimationClipCountTag, aclip.Count);

                ContentBinaryWriter bw = data.AddEntry(RootAnimationClipTag);
                foreach (var e in aclip)
                {
                    bw.WriteStringUnicode(e.Key);

                    ModelAnimationClip clip = e.Value;
                    bw.Write(clip.Duration.TotalSeconds);

                    bw.Write(clip.Keyframes.Count);

                    for (int i = 0; i < clip.Keyframes.Count; i++)
                    {
                        bw.Write(clip.Keyframes[i].Bone);
                        bw.Write(clip.Keyframes[i].Time.TotalSeconds);
                        bw.Write(clip.Keyframes[i].Transform);
                    }
                }
                bw.Close();
            }

            #endregion

            #region BoneHierarchyTag

            List<int> bh = animData.SkeletonHierarchy;
            if (bh != null)
            {
                data.AddEntry(BoneHierarchyCountTag, bh.Count);

                ContentBinaryWriter bw = data.AddEntry(BoneHierarchyTag);
                for (int i = 0; i < bh.Count; i++)
                {
                    bw.Write(bh[i]);
                }
                bw.Close();
            }

            #endregion

            return data;
        }
        BinaryDataWriter SaveMaterialAnimation()
        {
            BinaryDataWriter data = new BinaryDataWriter();

            data.AddEntry(MaterialAnimationDurationTag, materialAnimation.Duration);
            data.AddEntry(MaterialAnimationKeyframeCountTag, materialAnimation.Keyframes.Count);


            List<MaterialAnimationKeyFrame> keyframes = materialAnimation.Keyframes;

            ContentBinaryWriter bw = data.AddEntry(MaterialAnimationKeyframesTag);

            for (int i = 0; i < keyframes.Count; i++)
            {
                bw.Write(keyframes[i].Time);
                bw.Write(keyframes[i].MaterialIndex);                
            }
            bw.Close();

            return data;
        }
        void LoadAnimation(BinaryDataReader ad) 
        {

            #region Bones
            if (ad.Contains(BoneCountTag))
            {
                int boenCount = ad.GetDataInt32(BoneCountTag);

                ContentBinaryReader br2 = ad.GetData(BonesTag);
                List<Bone> bones = new List<Bone>(boenCount);

                for (int i = 0; i < boenCount; i++)
                {

                    int bidx = br2.ReadInt32();
                    string name = br2.ReadStringUnicode();
                    Matrix transform = br2.ReadMatrix();

                    int parentId = br2.ReadInt32();

                    int cldCount = br2.ReadInt32();
                    List<int> children = new List<int>(cldCount);
                    for (int j = 0; j < cldCount; j++)
                    {
                        children.Add(br2.ReadInt32());
                    }

                    bones.Add(new Bone(bidx, transform, children.ToArray(), parentId, name));
                }
                br2.Close();

                rootBone = ad.GetDataInt32(RootBoneTag);
            }



            #endregion

            List<Matrix> bindPose = null;
            List<Matrix> invBindPose = null;
            Dictionary<string, ModelAnimationClip> modelAnim = null;
            Dictionary<string, ModelAnimationClip> rootAnim = null;
            List<int> skeleHierarchy = null;

            #region BindPoseTag
            if (ad.Contains(BindPoseCountTag))
            {
                int count = ad.GetDataInt32(BindPoseCountTag);
                bindPose = new List<Matrix>(count);

                ContentBinaryReader br2 = ad.GetData(BindPoseTag);
                for (int i = 0; i < count; i++)
                {
                    bindPose.Add(br2.ReadMatrix());
                }
                br2.Close();
            }


            #endregion

            #region InvBindPoseTag


            if (ad.Contains(InvBindPoseCountTag))
            {
                int count = ad.GetDataInt32(InvBindPoseCountTag);
                invBindPose = new List<Matrix>(count);

                ContentBinaryReader br2 = ad.GetData(InvBindPoseTag);
                for (int i = 0; i < count; i++)
                {
                    invBindPose.Add(br2.ReadMatrix());
                }
                br2.Close();
            }

            #endregion

            #region AnimationClipTag

            if (ad.Contains(ModelAnimationClipCountTag))
            {
                int count = ad.GetDataInt32(ModelAnimationClipCountTag);

                modelAnim = new Dictionary<string, ModelAnimationClip>(count);

                ContentBinaryReader br2 = ad.GetData(ModelAnimationClipTag);

                for (int i = 0; i < count; i++)
                {
                    string key = br2.ReadStringUnicode();

                    TimeSpan duration = TimeSpan.FromSeconds(br2.ReadDouble());

                    int frameCount = br2.ReadInt32();
                    List<ModelKeyframe> frames = new List<ModelKeyframe>(frameCount);
                    for (int j = 0; j < frameCount; j++)
                    {
                        int bone = br2.ReadInt32();
                        TimeSpan totalSec = TimeSpan.FromSeconds(br2.ReadDouble());
                        Matrix transform = br2.ReadMatrix();

                        ModelKeyframe frame = new ModelKeyframe(bone, totalSec, transform);
                        frames.Add(frame);
                    }

                    ModelAnimationClip clip = new ModelAnimationClip(duration, frames);

                    modelAnim.Add(key, clip);
                }
                br2.Close();
            }


            #endregion

            #region RootAnimationClipTag

            if (ad.Contains(RootAnimationClipCountTag))
            {
                int count = ad.GetDataInt32(RootAnimationClipCountTag);

                rootAnim = new Dictionary<string, ModelAnimationClip>(count);

                ContentBinaryReader br2 = ad.GetData(RootAnimationClipTag);

                for (int i = 0; i < count; i++)
                {
                    string key = br2.ReadStringUnicode();

                    TimeSpan duration = TimeSpan.FromSeconds(br2.ReadDouble());

                    int frameCount = br2.ReadInt32();
                    List<ModelKeyframe> frames = new List<ModelKeyframe>(frameCount);
                    for (int j = 0; j < frameCount; j++)
                    {
                        int bone = br2.ReadInt32();
                        TimeSpan totalSec = TimeSpan.FromSeconds(br2.ReadDouble());
                        Matrix transform = br2.ReadMatrix();

                        ModelKeyframe frame = new ModelKeyframe(bone, totalSec, transform);
                        frames.Add(frame);
                    }

                    ModelAnimationClip clip = new ModelAnimationClip(duration, frames);
                    rootAnim.Add(key, clip);
                }
                br2.Close();
            }

            #endregion

            #region BoneHierarchyTag

            if (ad.Contains(BoneHierarchyCountTag))
            {
                int count = ad.GetDataInt32(BoneHierarchyCountTag);
                skeleHierarchy = new List<int>(count);


                ContentBinaryReader br2 = ad.GetData(BoneHierarchyTag);
                for (int i = 0; i < count; i++)
                {
                    skeleHierarchy.Add(br2.ReadInt32());
                }

                br2.Close();
            }


            #endregion

            this.animData = new AnimationData(modelAnim, rootAnim, bindPose, invBindPose, skeleHierarchy);
        }
        void LoadMaterialAnimation(BinaryDataReader md)
        {
            float duration = md.GetDataSingle(MaterialAnimationDurationTag);
            int count = md.GetDataInt32(MaterialAnimationKeyframeCountTag);

            List<MaterialAnimationKeyFrame> keyframes = new List<MaterialAnimationKeyFrame>(count);

            ContentBinaryReader br = md.GetData(MaterialAnimationKeyframesTag);
            for (int i = 0; i < count; i++)
            {
                float time = br.ReadSingle();
                int mid = br.ReadInt32();

                keyframes.Add(new MaterialAnimationKeyFrame(time, mid));
            }
            br.Close();

            materialAnimation = new MaterialAnimationClip(duration, keyframes);
        }
        protected void ReadData(BinaryDataReader data)
        {
            int entCount = data.GetDataInt32(EntityCountTag);
            entities = new MeshType[entCount];

            ContentBinaryReader br;
            for (int i = 0; i < entCount; i++)
            {
                br = data.GetData(EntityPrefix + i.ToString());
                BinaryDataReader meshData = br.ReadBinaryData();
                entities[i] = LoadMesh(meshData);
                meshData.Close();
                br.Close();
            }


            if (data.Contains(AnimationDataTag)) 
            {
                br = data.GetData(AnimationDataTag);

                BinaryDataReader ad = br.ReadBinaryData();

                LoadAnimation(ad);

                ad.Close();
                br.Close();
            }

            if (data.Contains(MaterialAnimationTag2)) 
            {
                br = data.GetData(MaterialAnimationTag2);

                BinaryDataReader md = br.ReadBinaryData();

                LoadMaterialAnimation(md);

                md.Close();

                br.Close();
            }

        }
        protected void WriteData(BinaryDataWriter data)
        {
            UseSync();
            data.AddEntry(EntityCountTag, entities.Length);

            ContentBinaryWriter bw;
            for (int i = 0; i < entities.Length; i++)
            {
                bw = data.AddEntry(EntityPrefix + i.ToString());

                BinaryDataWriter meshData = SaveMesh(entities[i]);
                bw.Write(meshData);
                meshData.Dispose();
                bw.Close();
            }

           
            if (animData != null)
            {
                bw = data.AddEntry(AnimationDataTag);
                BinaryDataWriter ad = SaveAnimation();
                bw.Write(ad);
                ad.Dispose();
                bw.Close();
            }

            if (materialAnimation != null) 
            {
                bw = data.AddEntry(MaterialAnimationTag2);
                BinaryDataWriter md = SaveMaterialAnimation();
                bw.Write(md);
                md.Dispose();
                bw.Close();                
            }

            //ModelAnimationFlags flags = ModelAnimationFlags.EntityTransform;

            //if (skinAnim != null)
            //{
            //    flags |= ModelAnimationFlags.Skin;
            //}


            //data.AddEntry(AnimationFlagTag, (int)flags);

            //BinaryDataWriter animData;

            //if ((flags & ModelAnimationFlags.EntityTransform) == ModelAnimationFlags.EntityTransform)
            //{
            //    bw = data.AddEntry(AnimationTag + ModelAnimationFlags.EntityTransform.ToString());
            //    animData = transAnim.Data.Save();
            //    bw.Write(animData);
            //    animData.Dispose();
            //}

            //if ((flags & ModelAnimationFlags.Skin) == ModelAnimationFlags.Skin)
            //{
            //    bw = data.AddEntry(AnimationTag + ModelAnimationFlags.Skin.ToString());
            //    animData = skinAnim.Data.Save();
            //    bw.Write(animData);
            //    animData.Dispose();
            //}
        }


        public override int GetSize()
        {
            if (DataSource != null)
            {
                return DataSource.Size;
            }
            return 0;
        }
        protected override void load()
        {
            if (DataSource != null)
            {
                ContentBinaryReader br = new ContentBinaryReader(DataSource);
                if (br.ReadInt32() == MdlId)
                {
                    BinaryDataReader data = br.ReadBinaryData();

                    ReadData(data);

                    data.Close();
                }

                br.Close();
            }
        }


        public static BinaryDataWriter ToBinary(ModelBase<MeshType> mdl)
        {
            BinaryDataWriter data = new BinaryDataWriter();
            mdl.WriteData(data);
            return data;
        }

        public static void ToFile(ModelBase<MeshType> mdl, string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            ContentBinaryWriter bw = new ContentBinaryWriter(fs);

            bw.Write(ModelData.MdlId);
            BinaryDataWriter mdlData = ToBinary(mdl);
            bw.Write(mdlData);
            mdlData.Dispose();

            bw.Close();
        }

        public static void ToStream(ModelBase<MeshType> mdl, Stream stm)
        {
            ContentBinaryWriter bw = new ContentBinaryWriter(stm, Encoding.Default);

            bw.Write(MdlId);

            BinaryDataWriter mdlData = ToBinary(mdl);
            bw.Write(mdlData);
            mdlData.Dispose();

            bw.Close();
        }
    }

    public class Model : IRenderable, IUpdatable
    {
        enum AnimationControl
        {
            Play,
            Stop,
            Resume,
            Pause
        }
        /// <summary>
        ///  已缓存的RenderOperation
        /// </summary>
        RenderOperation[] opBuffer;

        /// <summary>
        ///  renderOpEntId[i] 表示索引为i的renderOperation的Entity索引
        /// </summary>
        int[] renderOpEntId;
        int[] renderOpEntPartId;

        //bool animationInitialized;
        bool rigidAnimCompleted;
        bool rootAnimCompleted;
        bool skinAnimCompleted;
        bool materialAnimCompleted;

        protected ResourceHandle<ModelData> data;

        SkinnedAnimationPlayer skinPlayer;
        RootAnimationPlayer rootPlayer;
        RigidAnimationPlayer rigidPlayer;

        MaterialAnimationPlayer mtrlPlayer;

        protected List<ModelAnimationPlayerBase> animInstance = new List<ModelAnimationPlayerBase>();

        public List<ModelAnimationPlayerBase> CurrentAnimation
        {
            get { return animInstance; }
        }

        public event AnimationCompeletedEventHandler AnimationCompeleted;

        public bool AutoLoop
        {
            get;
            set;
        }

        public float SkinAnimDuration 
        {
            get
            {
                ModelData mdlData = data.GetWeakResource();
                AnimationData animData = mdlData.Animation;

                if (animData.ModelAnimationClips != null)
                {
                    if (animData.ModelAnimationClips.ContainsKey("Take 001"))
                    {
                        ModelAnimationClip clip = animData.ModelAnimationClips["Take 001"];
                        return (float)clip.Duration.TotalSeconds;
                    }
                }
                return 0;
            }
        }

        public Model(ResourceHandle<ModelData> data)
        {
            //CurrentAnimation = new NoAnimation(); 
            this.data = data;

            //if (!animationInitialized)
            //{
            InitializeAnimation();
            //animationInitialized = true;
            //}
        }
        protected Model()
        {
            //CurrentAnimation = new NoAnimation();
        }

        private void ControlRootAnimation(AnimationControl ctrl)
        {
            if (rootPlayer == null)
                return;
            
            ModelData mdlData = data.GetWeakResource();
            AnimationData animData = mdlData.Animation;

            if (animData == null)
                return;

            if (animData.RootAnimationClips != null)
            {
                if (animData.RootAnimationClips.ContainsKey("Take 001"))
                {
                    ModelAnimationClip clip = animData.RootAnimationClips["Take 001"];

                    switch (ctrl)
                    {
                        case AnimationControl.Play:
                            rootPlayer.StartClip(clip, 1, TimeSpan.Zero);
                            break;
                        case AnimationControl.Pause:
                            rootPlayer.PauseClip();
                            break;
                        case AnimationControl.Stop:
                            rootPlayer.PauseClip();
                            rootPlayer.CurrentKeyFrame = clip.Keyframes.Count > 10 ? 10 : 0;
                            break;
                        case AnimationControl.Resume:
                            rootPlayer.ResumeClip();
                            break;
                    }

                }
            }
        }
        private void ControlSkinnedAnimation(AnimationControl ctrl)
        {
            if (skinPlayer == null)
                return;

            ModelData mdlData = data.GetWeakResource();
            AnimationData animData = mdlData.Animation;

            if (animData == null)
                return;


            if (animData.ModelAnimationClips != null)
            {
                if (animData.ModelAnimationClips.ContainsKey("Take 001"))
                {
                    ModelAnimationClip clip = animData.ModelAnimationClips["Take 001"];

                    switch (ctrl)
                    {
                        case AnimationControl.Play:
                            skinPlayer.StartClip(clip, 1, TimeSpan.Zero);
                            break;
                        case AnimationControl.Stop:
                            skinPlayer.PauseClip();
                            skinPlayer.CurrentKeyFrame = clip.Keyframes.Count > 10 ? 10 : 0;
                            break;
                        case AnimationControl.Pause:
                            skinPlayer.PauseClip();
                            break;
                        case AnimationControl.Resume:
                            skinPlayer.ResumeClip();
                            break;
                    }
                }
            }
        }
        private void ControlRigidAnimation(AnimationControl ctrl)
        {
            if (rigidPlayer == null)
                return;
            ModelData mdlData = data.GetWeakResource();
            AnimationData animData = mdlData.Animation;

            if (animData == null)
                return;

            if (animData.ModelAnimationClips != null)
            {
                if (animData.ModelAnimationClips.ContainsKey("Take 001"))
                {
                    ModelAnimationClip clip = animData.ModelAnimationClips["Take 001"];

                    switch (ctrl)
                    {
                        case AnimationControl.Play:
                            rigidPlayer.StartClip(clip, 1, TimeSpan.Zero);
                            break;
                        case AnimationControl.Pause:
                            rigidPlayer.PauseClip();
                            break;
                        case AnimationControl.Stop:
                            rigidPlayer.PauseClip();
                            rigidPlayer.CurrentKeyFrame = clip.Keyframes.Count > 10 ? 10 : 0;
                            break;
                        case AnimationControl.Resume:
                            rigidPlayer.ResumeClip();
                            break;
                    }
                }
            }
        }
        private void ControlMaterialAnimation(AnimationControl ctrl)
        {
            if (mtrlPlayer == null)
                return;
         
            ModelData mdlData = data.GetWeakResource();
            MaterialAnimationClip animData = mdlData.MaterialAnimationClip;

            if (animData != null)
            {
                switch (ctrl)
                {
                    case AnimationControl.Play:
                        mtrlPlayer.StartClip(animData);
                        break;
                    case AnimationControl.Pause:
                        mtrlPlayer.PauseClip();
                        break;
                    case AnimationControl.Stop:
                        mtrlPlayer.PauseClip();
                        mtrlPlayer.CurrentKeyFrame = 0;
                        break;
                    case AnimationControl.Resume:
                        mtrlPlayer.ResumeClip();
                        break;
                }
            }

        }

        public void PlayAnimation()
        {
            ControlRootAnimation(AnimationControl.Play);
            ControlSkinnedAnimation(AnimationControl.Play);
            ControlRigidAnimation(AnimationControl.Play);
            ControlMaterialAnimation(AnimationControl.Play);
        }
        public void PauseAnimation()
        {
            ControlRootAnimation(AnimationControl.Pause);
            ControlSkinnedAnimation(AnimationControl.Pause);
            ControlRigidAnimation(AnimationControl.Pause);
            ControlMaterialAnimation(AnimationControl.Pause);
        }
        public void ResumeAnimation()
        {
            ControlRootAnimation(AnimationControl.Resume);
            ControlSkinnedAnimation(AnimationControl.Resume);
            ControlRigidAnimation(AnimationControl.Resume);
            ControlMaterialAnimation(AnimationControl.Resume);
        }
        public void StopAnimation()
        {
            ControlRootAnimation(AnimationControl.Stop);
            ControlSkinnedAnimation(AnimationControl.Stop);
            ControlRigidAnimation(AnimationControl.Stop);
            ControlMaterialAnimation(AnimationControl.Stop);
        }

        public float GetAnimationDuration()
        {
            if (skinPlayer != null)
            {
                return (float)skinPlayer.CurrentClip.Duration.TotalSeconds;
            }
            else if (rigidPlayer != null)
            {
                return (float)rigidPlayer.CurrentClip.Duration.TotalSeconds;
            }
            else if (rootPlayer != null)
            {
                return (float)rootPlayer.CurrentClip.Duration.TotalSeconds;
            }
            return 0;
        }

        public ModelData GetData()
        {
            data.TouchSync();
            return data.Resource;
        }

        void RootAnim_Completed(object sender, EventArgs e)
        {
            rootAnimCompleted = true;
            if (AnimationCompeleted != null)
                AnimationCompeleted(sender, new AnimationCompletedEventArgs(AnimationCompletedEventArgs.AnimationType.Root));
        }
        void RigidAnim_Competed(object sender, EventArgs e)
        {
            rigidAnimCompleted = true;
            if (AnimationCompeleted != null)
                AnimationCompeleted(sender, new AnimationCompletedEventArgs(AnimationCompletedEventArgs.AnimationType.Rigid));
        }
        void SkinAnim_Completed(object sender, EventArgs e)
        {
            skinAnimCompleted = true;
            if (AnimationCompeleted != null)
                AnimationCompeleted(sender, new AnimationCompletedEventArgs(AnimationCompletedEventArgs.AnimationType.Skinned));
        }
        void MtrlAnim_Completed(object sender, EventArgs e) 
        {
            materialAnimCompleted = true;
        }
        #region IRenderable 成员

        void InitializeAnimation()
        {
            data.TouchSync();

            ModelData mdlData = data.GetWeakResource();
            AnimationData animData = mdlData.Animation;

            if (animData == null)
                return;

            if (animData.RootAnimationClips != null)
            {
                if (animData.RootAnimationClips.ContainsKey("Take 001"))
                {
                    //ModelAnimationClip clip = animData.RootAnimationClips["Take 001"];

                    rootPlayer = new RootAnimationPlayer();
                    CurrentAnimation.Add(rootPlayer);

                    rootPlayer.Completed += RootAnim_Completed;
                    //rootPlayer.StartClip(clip, 1, TimeSpan.Zero);

                }
            }
            if (animData.ModelAnimationClips != null)
            {
                if (animData.BindPose != null && animData.InverseBindPose != null)
                {
                    if (animData.ModelAnimationClips.ContainsKey("Take 001"))
                    {
                        //ModelAnimationClip clip = animData.ModelAnimationClips["Take 001"];

                        skinPlayer = new SkinnedAnimationPlayer(animData.BindPose, animData.InverseBindPose, animData.SkeletonHierarchy);
                        CurrentAnimation.Add(skinPlayer);

                        skinPlayer.Completed += SkinAnim_Completed;
                        //skinPlayer.StartClip(clip, 1, TimeSpan.Zero);
                    }

                }
                else
                {
                    if (animData.ModelAnimationClips.ContainsKey("Take 001"))
                    {
                        //ModelAnimationClip clip = animData.ModelAnimationClips["Take 001"];

                        rigidPlayer = new RigidAnimationPlayer(mdlData.Bones.Length);
                        CurrentAnimation.Add(rigidPlayer);

                        rigidPlayer.Completed += RigidAnim_Competed;
                        //rigidPlayer.StartClip(clip, 1, TimeSpan.Zero);
                    }
                }
            }

            MaterialAnimationClip mtrlClip = mdlData.MaterialAnimationClip;
            if (mtrlClip != null) 
            {
                mtrlPlayer = new MaterialAnimationPlayer();
                mtrlPlayer.Completed += MtrlAnim_Completed;
            }
        }
        public void ReloadMaterialAnimation()
        {
            if (mtrlPlayer != null)
            {
                mtrlPlayer.Completed -= MtrlAnim_Completed;
            }

            ModelData mdlData = data.GetWeakResource();
            MaterialAnimationClip mtrlClip = mdlData.MaterialAnimationClip;
            if (mtrlClip != null)
            {
                mtrlPlayer = new MaterialAnimationPlayer();
                mtrlPlayer.Completed += MtrlAnim_Completed;
            }
        }
        void UpdateAnimtaion()
        {
            if (rootAnimCompleted)
            {
                if (AutoLoop)
                    ControlRootAnimation(AnimationControl.Play);
                rootAnimCompleted = false;
            }
            if (rigidAnimCompleted)
            {
                if (AutoLoop)
                    ControlRigidAnimation(AnimationControl.Play);
                rigidAnimCompleted = false;
            }
            if (skinAnimCompleted)
            {
                if (AutoLoop)
                    ControlSkinnedAnimation(AnimationControl.Play);
                skinAnimCompleted = false;
            }
            if (materialAnimCompleted) 
            {
                if (AutoLoop)
                    ControlMaterialAnimation(AnimationControl.Play);
                materialAnimCompleted = false;
            }
        }
        public RenderOperation[] GetRenderOperation()
        {
            if (data.State != ResourceState.Loaded && data.GetWeakResource().IsManaged)
            {
                data.Touch();
                return null;
            }

            UpdateAnimtaion();

            Mesh[] entities = data.Resource.Entities;

            // 未创建缓存
            if (opBuffer == null)
            {
                RenderOperation[][] entOps = new RenderOperation[entities.Length][];

                int opCount = 0;
                for (int i = 0; i < entities.Length; i++)
                {
                    entOps[i] = entities[i].GetRenderOperation();

                    opCount += entOps[i].Length;
                }

                int dstIdx = 0;
                //gmBuffer = new GeomentryData[opCount];
                opBuffer = new RenderOperation[opCount];
                renderOpEntId = new int[opCount];
                renderOpEntPartId = new int[opCount];

                for (int i = 0; i < entities.Length; i++)
                {
                    Array.Copy(entOps[i], 0, opBuffer, dstIdx, entOps[i].Length);

                    for (int j = 0; j < entOps[i].Length; j++)
                    {
                        int opid = dstIdx + j;
                        renderOpEntId[opid] = i;
                        renderOpEntPartId[opid] = j;

                        opBuffer[opid].BoneTransforms = skinPlayer == null ? null : skinPlayer.GetSkinTransforms();
                        
                        if (animInstance.Count > 0)
                        {
                            opBuffer[opid].Transformation = Matrix.Identity;

                            for (int k = 0; k < animInstance.Count; k++)
                            {
                                int entId = i;

                                int boneId = entities[entId].ParentBoneID;

                                opBuffer[opid].Transformation *= animInstance[k].GetTransform(boneId);

                            }
                        }
                        else
                        {
                            opBuffer[opid].Transformation = Matrix.Identity;
                        }
                    }

                    dstIdx += entOps[i].Length;
                }
            }
            else
            {
                for (int i = 0; i < opBuffer.Length; i++)
                {
                    if (mtrlPlayer != null) 
                    {
                        int partId = renderOpEntPartId[i];
                        int entId = renderOpEntId[i];
                        int frame = mtrlPlayer.CurrentFrame;
                        if (frame >= entities[entId].Materials[partId].Length) 
                        {
                            frame = entities[entId].Materials[partId].Length - 1;
                        }
                        opBuffer[i].Material = entities[entId].Materials[partId][frame];
                    }

                    if (animInstance.Count > 0)
                    {
                        opBuffer[i].Transformation = Matrix.Identity;

                        for (int k = 0; k < animInstance.Count; k++)
                        {
                            int entId = renderOpEntId[i];

                            int boneId = entities[entId].ParentBoneID;

                            opBuffer[i].Transformation *= animInstance[k].GetTransform(boneId);

                        }
                    }
                    else
                    {
                        opBuffer[i].Transformation = Matrix.Identity;
                    }

                    opBuffer[i].BoneTransforms = skinPlayer == null ? null : skinPlayer.GetSkinTransforms();
                    
                }

            }
            return opBuffer;
        }
        public RenderOperation[] GetRenderOperation(int level)
        {
            return GetRenderOperation();
        }

        #endregion

        #region IUpdatable 成员

        public void Update(GameTime dt)
        {
            for (int i = 0; i < animInstance.Count; i++)
            {
                animInstance[i].Update(dt);
            }
            if (mtrlPlayer != null)
            {
                mtrlPlayer.Update(dt);
            }

            //GameMesh[] entities = data.Resource.Entities;
            //if (entities != null)
            //{
            //    for (int i = 0; i < entities.Length; i++)
            //    {
            //        entities[i].Update(dt);
            //    }
            //}
        }

        #endregion
    }

    public class ModelMemoryData : ModelBase<MeshData>
    {
        protected RenderSystem renderSystem;
        [Browsable(false)]
        public RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }
        public ModelMemoryData(RenderSystem renderSystem, ResourceLocation rl)
        {
            this.renderSystem = renderSystem;
            DataSource = rl;
            load();
        }

        public ModelMemoryData(RenderSystem renderSystem, MeshData[] entities)
        {
            this.renderSystem = renderSystem;

            this.entities = entities;
        }
        public ModelMemoryData(RenderSystem renderSystem, int entityCount)
        {
            this.renderSystem = renderSystem;

            this.entities = new MeshData[entityCount];
        }

        protected override MeshData LoadMesh(BinaryDataReader data)
        {
            MeshData md = new MeshData(renderSystem);
            md.Load(data);
            return md;
        }

        protected override BinaryDataWriter SaveMesh(MeshData mesh)
        {
            return mesh.Save();
        }

        protected override void unload()
        {
            
        }
    }

    /// <summary>
    ///  表示3D模型的数据
    /// </summary>
    public class ModelData : ModelBase<Mesh>, IDisposable
    {
        protected RenderSystem renderSystem;
        [Browsable(false)]
        public RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }

        public ModelData(RenderSystem renderSystem, ResourceLocation rl)
            : base(rl)
        {
            this.renderSystem = renderSystem;
        }

        public ModelData(RenderSystem renderSystem, Mesh[] entities)
        {
            this.renderSystem = renderSystem;

            this.entities = entities;
        }
        public ModelData(RenderSystem renderSystem, int entityCount)
        {
            this.renderSystem = renderSystem;

            this.entities = new Mesh[entityCount];
        }


        protected override void unload()
        {
            if (entities != null)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    if (!entities[i].Disposed)
                    {
                        entities[i].Dispose();
                    }
                    entities[i] = null;
                }
            }
        }

        private ModelData(RenderSystem dev)
        {
            this.renderSystem = dev;
        }

        protected override Mesh LoadMesh(BinaryDataReader data)
        {
            MeshData md = new MeshData(renderSystem);
            md.Load(data);
            return new Mesh(renderSystem, md);
        }
        protected override BinaryDataWriter SaveMesh(Mesh mesh)
        {
            MeshData md = new MeshData(mesh);
            return md.Save();
        }


        #region IDisposable 成员

        protected override void dispose(bool disposing)
        {
            base.dispose(disposing);

            if (disposing)
            {
                if (entities != null)
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        if (!entities[i].Disposed)
                        {
                            entities[i].Dispose();
                        }
                        entities[i] = null;
                    }
                }
            }
            entities = null;
        }

        #endregion

    }
}
