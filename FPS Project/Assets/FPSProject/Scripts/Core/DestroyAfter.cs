using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core
{
    public class DestroyAfter : MonoBehaviour
    {
        [SerializeField]
        private float m_Time = 0.0f;
        [SerializeField]
        private bool m_IsActive = false;
        public void SetTime(float time)
        {
            this.m_Time = time;
        }

        public void Activate() => m_IsActive = true;
        void Update()
        {
            if (!m_IsActive) return;
            if (m_Time > 0.0f)
            {
                m_Time -= Time.deltaTime;
                return;
            }
            GameObjectExtensions.SafeDestroyGameObject(this);
        }
    }
}