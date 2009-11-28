//Originally by Jon Watte.
//Released into the JigLibX project under the JigLibX license.
//Separately released into the public domain by the author.

#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JigLibX.Geometry;
using Apoc3D.MathLib;
#endregion

namespace JigLibX.Collision
{

    /// <summary>
    /// Implementing a collision system (broad-phase test) based on the sweep-and-prune 
    /// algorithm
    /// </summary>
    public class CollisionSystemSAP : CollisionSystem, IComparer<CollisionSkin>
    {
        List<CollisionSkin> skins_ = new List<CollisionSkin>();
        bool dirty_;
        float largestX_;
        List<CollisionSkin> active_ = new List<CollisionSkin>();
        List<Primitive> testing_ = new List<Primitive>();
        List<Primitive> second_ = new List<Primitive>();

        public float LargestX { get { return largestX_; } }

        public CollisionSystemSAP()
        {
        }

        public override void AddCollisionSkin(CollisionSkin collisionSkin)
        {
            collisionSkin.CollisionSystem = this;
            skins_.Add(collisionSkin);
            dirty_ = true;
            float dx = collisionSkin.WorldBoundingBox.Maximum.X - collisionSkin.WorldBoundingBox.Minimum.X;
            if (dx > largestX_)
                largestX_ = dx;
        }

        public override bool RemoveCollisionSkin(CollisionSkin collisionSkin)
        {
            int ix = skins_.IndexOf(collisionSkin);
            if (ix >= skins_.Count || ix < 0)
                return false;
            skins_.RemoveAt(ix);
            return true;
        }

        public override ReadOnlyCollection<CollisionSkin> CollisionSkins
        {
            get { return skins_.AsReadOnly(); }
        }

        public override void CollisionSkinMoved(CollisionSkin skin)
        {
            dirty_ = true;
        }

        void Extract(Vector3 min, Vector3 max, List<CollisionSkin> skins)
        {
            if (skins_.Count == 0)
                return;
            MaybeSort();
            int i = bsearch(min.X - largestX_);
            float xMax = max.X;
            while (i < skins_.Count && skins_[i].WorldBoundingBox.Minimum.X < xMax)
            {
                if (skins_[i].WorldBoundingBox.Maximum.X > min.X)
                    skins.Add(skins_[i]);
                ++i;
            }
        }

        int bsearch(float x)
        {
            //  It is up to the caller to make sure this isn't called on an empty collection.
            int top = skins_.Count;
            int bot = 0;
            while (top > bot)
            {
                int mid = (top + bot) >> 1;
                if (skins_[mid].WorldBoundingBox.Minimum.X >= x)
                {
#if DEBUG
                    System.Diagnostics.Debug.Assert(top > mid);
#endif
                    top = mid;
                }
                else
                {
#if DEBUG
                    System.Diagnostics.Debug.Assert(bot <= mid);
#endif
                    bot = mid + 1;
                }
            }

#if DEBUG
            System.Diagnostics.Debug.Assert(top >= 0 && top <= skins_.Count);
            System.Diagnostics.Debug.Assert(top == 0 || skins_[top - 1].WorldBoundingBox.Minimum.X < x);
            System.Diagnostics.Debug.Assert(top == skins_.Count || skins_[top].WorldBoundingBox.Minimum.X >= x);
#endif

            return top;
        }

        public override void DetectCollisions(JigLibX.Physics.Body body, CollisionFunctor collisionFunctor, CollisionSkinPredicate2 collisionPredicate, float collTolerance)
        {
            if (!body.IsActive)
                return;

            CollDetectInfo info = new CollDetectInfo();
            info.Skin0 = body.CollisionSkin;
            if (info.Skin0 == null)
                return;

            active_.Clear();
            testing_.Clear();
            Extract(info.Skin0.WorldBoundingBox.Minimum, info.Skin0.WorldBoundingBox.Maximum, active_);

            for (int j = 0, m = info.Skin0.NumPrimitives; j != m; ++j)
                testing_.Add(info.Skin0.GetPrimitiveNewWorld(j));

            int nBodyPrims = testing_.Count;

            for (int i = 0, n = active_.Count; i != n; ++i)
            {
                info.Skin1 = active_[i];
                if (info.Skin0 != info.Skin1 && (collisionPredicate == null ||
                    collisionPredicate.ConsiderSkinPair(info.Skin0, info.Skin1)))
                {
                    int nPrim1 = info.Skin1.NumPrimitives;
                    second_.Clear();
                    for (int k = 0; k != nPrim1; ++k)
                        second_.Add(info.Skin1.GetPrimitiveNewWorld(k));
                    for (info.IndexPrim0 = 0; info.IndexPrim0 != nBodyPrims; ++info.IndexPrim0)
                    {
                        for (info.IndexPrim1 = 0; info.IndexPrim1 != nPrim1; ++info.IndexPrim1)
                        {
                            DetectFunctor f =
                              GetCollDetectFunctor(info.Skin0.GetPrimitiveNewWorld(info.IndexPrim0).Type,
                                info.Skin1.GetPrimitiveNewWorld(info.IndexPrim1).Type);
                            if (f != null)
                                f.CollDetect(info, collTolerance, collisionFunctor);
                        }
                    }
                }
            }
        }

        SkinTester skinTester_ = new SkinTester();

        public override void DetectAllCollisions(List<JigLibX.Physics.Body> bodies, CollisionFunctor collisionFunctor, CollisionSkinPredicate2 collisionPredicate, float collTolerance)
        {
            skinTester_.Set(this, collisionFunctor, collisionPredicate, collTolerance);

            MaybeSort();
            //  I know that each skin for the bodies is already in my list of skins.
            //  Thus, I can do collision between all skins, culling out non-active bodies.
            int nSkins = skins_.Count;
            active_.Clear();

            // BEN-OPTIMISATION: unsafe, remove array boundary checks.
            unsafe
            {
                for (int i = 0; i != nSkins; ++i)
                    AddToActive(skins_[i], skinTester_);
            }
        }

        class SkinTester : CollisionSkinPredicate2
        {
            CollisionFunctor collisionFunctor_;
            CollisionSkinPredicate2 collisionPredicate_;
            float collTolerance_;
            CollDetectInfo info_;
            CollisionSystem sys_;

            internal SkinTester()
            {
            }

            internal void Set(CollisionSystem sys, CollisionFunctor collisionFunctor, CollisionSkinPredicate2 collisionPredicate, float collTolerance)
            {
                sys_ = sys;
                collisionFunctor_ = collisionFunctor;
                collisionPredicate_ = collisionPredicate;
                if (collisionPredicate_ == null)
                    collisionPredicate_ = this;
                collTolerance_ = collTolerance;
            }

            private static bool CheckCollidables(CollisionSkin skin0,CollisionSkin skin1)
            {
                List<CollisionSkin> nonColl0 = skin0.NonCollidables;
                List<CollisionSkin> nonColl1 = skin1.NonCollidables;

                // Most common case
                if (nonColl0.Count == 0 && nonColl1.Count == 0)
                    return true;

                for (int i0 = nonColl0.Count; i0-- != 0; )
                {
                    if (nonColl0[i0] == skin1)
                        return false;
                }

                for (int i1 = nonColl1.Count; i1-- != 0; )
                {
                    if (nonColl1[i1] == skin0)
                        return false;
                }

                return true;
            }

            // BEN-OPTIMISATION: unsafe i.e. Remove array boundary checks.
            internal unsafe void TestSkin(CollisionSkin b, CollisionSkin s)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(b.Owner != null);
                System.Diagnostics.Debug.Assert(b.Owner.IsActive);
#endif
                if (!collisionPredicate_.ConsiderSkinPair(b, s))
                    return;

                info_.Skin0 = b;
                info_.Skin1 = s;
                int nSkin0 = info_.Skin0.NumPrimitives;
                int nSkin1 = info_.Skin1.NumPrimitives;

                // BEN-OPTIMISATION: Reuse detectFunctor.
                DetectFunctor detectFunctor;
                for (info_.IndexPrim0 = 0; info_.IndexPrim0 != nSkin0; ++info_.IndexPrim0)
                {
                    for (info_.IndexPrim1 = 0; info_.IndexPrim1 != nSkin1; ++info_.IndexPrim1)
                    {
                        if (CheckCollidables(info_.Skin0, info_.Skin1))
                        {
                            detectFunctor = sys_.GetCollDetectFunctor(
                                info_.Skin0.GetPrimitiveNewWorld(info_.IndexPrim0).Type,
                                info_.Skin1.GetPrimitiveNewWorld(info_.IndexPrim1).Type);

                            if (detectFunctor != null)
                                detectFunctor.CollDetect(info_, collTolerance_, collisionFunctor_);
                        }
                    }
                }
            }

            public override bool ConsiderSkinPair(CollisionSkin skin0, CollisionSkin skin1)
            {
                return true;
            }
        }

        void AddToActive(CollisionSkin cs, SkinTester st)
        {
            int n = active_.Count;
            float xMin = cs.WorldBoundingBox.Minimum.X;
            bool active = (cs.Owner != null) && cs.Owner.IsActive;
            // BEN-OPTIMISATION: unsafe i.e. Remove array boundary checks.
            unsafe
            {
                CollisionSkin asi;
                for (int i = 0; i != n; )
                {
                    asi = active_[i];
                    if (asi.WorldBoundingBox.Maximum.X < xMin)
                    {
                        //  prune no longer interesting boxes from potential overlaps
                        --n;
                        active_.RemoveAt(i);
                    }
                    else
                    {
                        // BEN-OPTIMISATION: Inlined BoundingBoxHelper.OverlapTest() and removed two redundant
                        //                   comparisons the X comparison and the extra "if (active)" which can
                        //                   be removed by rearranging.
                        if (active)
                        {
                            if (!((cs.WorldBoundingBox.Minimum.Z >= asi.WorldBoundingBox.Maximum.Z) ||
                                    (cs.WorldBoundingBox.Maximum.Z <= asi.WorldBoundingBox.Minimum.Z) ||
                                    (cs.WorldBoundingBox.Minimum.Y >= asi.WorldBoundingBox.Maximum.Y) ||
                                    (cs.WorldBoundingBox.Maximum.Y <= asi.WorldBoundingBox.Minimum.Y) ||
                                    (cs.WorldBoundingBox.Maximum.X <= asi.WorldBoundingBox.Minimum.X)))
                            {
                                st.TestSkin(cs, asi);
                            }
                        }
                        else if (active_[i].Owner != null && asi.Owner.IsActive
                                && !((cs.WorldBoundingBox.Minimum.Z >= asi.WorldBoundingBox.Maximum.Z) ||
                                    (cs.WorldBoundingBox.Maximum.Z <= asi.WorldBoundingBox.Minimum.Z) ||
                                    (cs.WorldBoundingBox.Minimum.Y >= asi.WorldBoundingBox.Maximum.Y) ||
                                    (cs.WorldBoundingBox.Maximum.Y <= asi.WorldBoundingBox.Minimum.Y) ||
                                    (cs.WorldBoundingBox.Maximum.X <= asi.WorldBoundingBox.Minimum.X)))
                        {
                            st.TestSkin(asi, cs);
                        }
                        ++i;
                    }
                }
            }
            active_.Add(cs);
        }

        public override bool SegmentIntersect(out float fracOut, out CollisionSkin skinOut, out Vector3 posOut, out Vector3 normalOut, JigLibX.Geometry.Segment seg, CollisionSkinPredicate1 collisionPredicate)
        {
            fracOut = float.MaxValue;
            skinOut = null;
            posOut = normalOut = Vector3.Zero;

            float frac;
            Vector3 pos;
            Vector3 normal;

            Vector3 segmentBeginning = seg.Origin;
            Vector3 segmentEnd = seg.Origin + seg.Delta;

            Vector3 min = Vector3.Minimize(segmentBeginning, segmentEnd);
            Vector3 max = Vector3.Maximize(segmentBeginning, segmentEnd);

            active_.Clear();

            BoundingBox box = new BoundingBox(min, max);
            Extract(min, max, active_);

            float distanceSquared = float.MaxValue;
            int nActive = active_.Count;
            for (int i = 0; i != nActive; ++i)
            {
                CollisionSkin skin = active_[i];
                if (collisionPredicate == null || collisionPredicate.ConsiderSkin(skin))
                {
                    if (BoundingBoxHelper.OverlapTest(ref box, ref skin.WorldBoundingBox))
                    {
                        if (skin.SegmentIntersect(out frac, out pos, out normal, seg))
                        {
                            if (frac >= 0)
                            {
                                float newDistanceSquared = Vector3.DistanceSquared(segmentBeginning, pos);
                                if (newDistanceSquared < distanceSquared)
                                {
                                    distanceSquared = newDistanceSquared;

                                    fracOut = frac;
                                    skinOut = skin;
                                    posOut = pos;
                                    normalOut = normal;
                                }
                            }
                        }
                    }
                }
            }
            
            return (fracOut <= 1);
        }

        void MaybeSort()
        {
            if (dirty_)
            {
                skins_.Sort(this);
                dirty_ = false;
            }
        }

        public int Compare(CollisionSkin x, CollisionSkin y)
        {
            float f = x.WorldBoundingBox.Minimum.X - y.WorldBoundingBox.Minimum.X;
            return (f < 0) ? -1 : (f > 0) ? 1 : 0;
        }
    }
}