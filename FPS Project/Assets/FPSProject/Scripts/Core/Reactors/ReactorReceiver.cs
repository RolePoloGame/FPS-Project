using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class ReactorReceiver : MonoBehaviour
    {
        [SerializeField]
        private List<SignalReactor> directSignal = new();
        [SerializeField]
        private UnityEvent ActivationUnityEvent;
        public event Action OnActivation;

        private bool m_IsActived = false;

        private void Start()
        {
            EnableSignals();
        }
        private void OnDestroy()
        {
            DisableSignals();
        }

        private void EnableSignals()
        {
            foreach (var signal in directSignal)
            {
                if (signal == null) continue;
                signal.OnSignalActivated += UpdateSignals;
            }
        }

        private void DisableSignals()
        {
            foreach (var signal in directSignal)
            {
                if (signal == null) continue;
                signal.OnSignalActivated -= UpdateSignals;
            }
        }

        private void UpdateSignals()
        {
            if (m_IsActived) return;
            foreach (var signal in directSignal)
            {
                if (signal == null) continue;
                if (!signal.IsActivated)
                {
                    return;
                }
            }
            m_IsActived = true;
            ActivationUnityEvent?.Invoke();
            OnActivation?.Invoke();
        }
    }
}