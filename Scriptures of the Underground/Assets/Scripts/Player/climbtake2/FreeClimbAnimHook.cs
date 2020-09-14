﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    //c = freeclimb script , w_rh w_lh = right/left hand weight ,w_ lf,w_rf = left/right foot weight
    //h = helper
    public class FreeClimbAnimHook : MonoBehaviour
    {
        Animator anim;
        IKSnapshot ikBase;
        IKSnapshot current = new IKSnapshot();
        IKSnapshot next = new IKSnapshot();

        public float w_rh;
        public float w_lh;
        public float w_lf;
        public float w_rf;

        Vector3 rh, lh, rf, lf;
        Transform h;

        public void Init(Freeclimb c,Transform helper)
        {
            anim = c.anim;
            ikBase = c.baseIKsnapshot;
            h = helper;
        }

        public void CreatePosition(Vector3 origin)
        {
            IKSnapshot ik = CreateSnapshot(origin);
            CopySnapshot(ref current, ik);

            UpdateIKPosition(AvatarIKGoal.LeftFoot, current.lf);
            UpdateIKPosition(AvatarIKGoal.RightFoot, current.rf);
            UpdateIKPosition(AvatarIKGoal.LeftHand, current.lh);
            UpdateIKPosition(AvatarIKGoal.RightHand, current.rh);

            UpdateIKWeight(AvatarIKGoal.LeftFoot, 1);
            UpdateIKWeight(AvatarIKGoal.RightFoot, 1);
            UpdateIKWeight(AvatarIKGoal.LeftHand, 1);
            UpdateIKWeight(AvatarIKGoal.RightHand, 1);
        }

        public IKSnapshot CreateSnapshot(Vector3 o)
        {
            IKSnapshot r = new IKSnapshot();
            Vector3 _lh = LocalToWorld(ikBase.lh);
            r.lh = GetPosActual(_lh);

            Vector3 _rh = LocalToWorld(ikBase.rh);
            r.rh = GetPosActual(_rh);
            

            Vector3 _lf = LocalToWorld(ikBase.lf);
            r.lf = GetPosActual(_lf);
            
            Vector3 _rf = LocalToWorld(ikBase.rf);
            r.rf = GetPosActual(_rf);
            return r;
        }

        public float wallOffset = 0;

        Vector3 GetPosActual(Vector3 o)
        {
            Vector3 r = o;
            Vector3 origin = o;
            Vector3 dir = h.forward;
            origin += -(dir * 0.2f);
            RaycastHit hit;
            if(Physics.Raycast(origin,dir,out hit, 1.5f))
            {
                //offset to wall
                Vector3 _r = hit.point + (hit.normal * wallOffset);
                r = _r;
            }

            return r;
        }

        //p= targetpos

        Vector3 LocalToWorld(Vector3 p)
        {
            Vector3 r = h.position;
            r += h.right * p.x;
            r += h.forward * p.z;
            r += h.up * p.y;
            return r;
        }

        public void CopySnapshot(ref IKSnapshot to, IKSnapshot from)
        {
            to.rh = from.rh;
            to.lh = from.lh;
            to.lf = from.lf;
            to.rf = from.rf;
        }

        public void UpdateIKPosition(AvatarIKGoal goal,Vector3 pos)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    lf = pos;
                    break;
                case AvatarIKGoal.RightFoot:
                    rf = pos;
                    break;
                case AvatarIKGoal.LeftHand:
                    lh = pos;
                    break;
                case AvatarIKGoal.RightHand:
                    rh = pos;
                    break;
            }
        }

        //w = weight
        public void UpdateIKWeight(AvatarIKGoal goal, float w)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    w_lf = w;
                    break;
                case AvatarIKGoal.RightFoot:
                    w_rf = w;
                    break;
                case AvatarIKGoal.LeftHand:
                    w_lh = w;
                    break;
                case AvatarIKGoal.RightHand:
                    w_rh = w;
                    break;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            SetIKPos(AvatarIKGoal.LeftHand, lh, w_lh);
            SetIKPos(AvatarIKGoal.RightHand, rh, w_rh);
            SetIKPos(AvatarIKGoal.LeftFoot, lf, w_lf);
            SetIKPos(AvatarIKGoal.RightFoot, rf, w_rf);
        }

        void SetIKPos(AvatarIKGoal goal, Vector3 tp, float w)
        {
            anim.SetIKPositionWeight(goal, w);
            anim.SetIKPosition(goal, tp);
        }
    }
}

