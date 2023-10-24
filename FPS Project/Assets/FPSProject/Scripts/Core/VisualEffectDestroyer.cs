using RolePoloGame.Core.Extensions;
using UnityEngine;
using UnityEngine.VFX;

namespace Assets.FPSProject.Scripts.Core
{
    public class VisualEffectDestroyer : MonoBehaviour
    {
        private VisualEffect m_VisualEffect;
        public void Set(VisualEffect effect)
        {
            m_VisualEffect = effect;
        }

        private float m_Delay = 2.0f;
        private bool m_Running = true;

        void Update()
        {
            if (!m_Running) return;
            if (m_VisualEffect == null) return;
            if (m_VisualEffect.aliveParticleCount != 0) return;
            if (m_Delay > 0)
            {
                m_Delay -= Time.deltaTime;
                return;
            }
            m_Running = false;
            /// Can be replaced with VFX Pooling Manager in the future
            GameObjectExtensions.SafeDestroyGameObject(this);
        }
    }
}