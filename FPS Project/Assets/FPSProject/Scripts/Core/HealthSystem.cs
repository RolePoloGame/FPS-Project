using System;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core
{
    [Serializable]
    public class HealthSystem
    {
        [SerializeField]
        private float m_CurrentHealth;
        [SerializeField, Header("Range")]
        private float m_MaxHealth = 100;
        [SerializeField]
        private float m_MinHealth = 0;

        public bool IsDead => m_CurrentHealth <= m_MinHealth;

        public void AddHealth(float value)
        {
            m_CurrentHealth += value;
            HandleRange();
        }

        public void RemoveHealth(float value)
        {
            m_CurrentHealth -= value;
            HandleRange();
        }

        private void HandleRange()
        {
            if (m_CurrentHealth > m_MaxHealth)
                m_CurrentHealth = m_MaxHealth;
            if (m_CurrentHealth < m_MinHealth)
                m_CurrentHealth = m_MinHealth;
        }
    }
}