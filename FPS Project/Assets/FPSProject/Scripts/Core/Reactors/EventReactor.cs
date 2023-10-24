using System;
using UnityEngine.Events;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class EventReactor : ReactorBase
    {
        public UnityEvent EventAction;
        public override bool PlayWithResult()
        {
            if (EventAction == null)
                return false;
            EventAction?.Invoke();
            return true;
        }
    }
}