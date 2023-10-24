using System;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public abstract class ReactorBase : MonoBehaviour
    {
        public void Play() => _ = PlayWithResult();
        public abstract bool PlayWithResult();
        public EReactorMask Mask;
        protected bool m_MarkedForDestruction = false;
        [Serializable]
        [Flags]
        public enum EReactorMask
        {
            Spawn = 1,
            Hit = 2,
            Interact = 4,
            Destroy = 8
        }

        public bool HasFlag(EReactorMask checkflag)
        {
            return (Mask & checkflag) == checkflag;
        }

        public virtual void MarkForDestruction() => m_MarkedForDestruction = true;
    }
}