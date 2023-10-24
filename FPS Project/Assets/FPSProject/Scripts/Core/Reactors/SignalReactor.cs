using System;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class SignalReactor : ReactorBase
    {
        public event Action OnSignalActivated;
        public bool IsActivated => m_IsActivated;
        private bool m_IsActivated = false;
        public override bool PlayWithResult()
        {
            m_IsActivated = true;
            OnSignalActivated?.Invoke();
            return true;
        }
    }
}