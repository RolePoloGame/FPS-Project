using Assets.FPSProject.Scripts.Core.Items.Weapons;
using Assets.FPSProject.Scripts.Core.Reactors;
using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Items
{
    [SelectionBase]
    public class DestructibleObject : MonoBehaviour, IDamagable
    {
        [SerializeField]
        private ItemMaterial material;
        [SerializeField]
        private bool indestructible;
        [SerializeField]
        private HealthSystem healthSystem;

        private Rigidbody m_Rigidbody;
        private ReactorManager m_ReactorManager;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_ReactorManager = GetComponent<ReactorManager>();
            m_ReactorManager.PlayOnSpawn();
        }

        public bool CanBeDamaged(Weapon weapon)
        {
            if (indestructible) return false;
            if (material == null) return false;
            return (material.CanBeDamaged(weapon));
        }

        public bool CanBeDamaged()
        {
            return !indestructible;
        }

        public void Damage(Weapon weapon, Vector3 position)
        {
            Damage(weapon);
            if (weapon.ExplosionForce == 0.0f) return;
            TryApplyForce(position, weapon);
        }

        private void TryApplyForce(Vector3 position, Weapon weapon)
        {
            if (m_Rigidbody == null) return;
            m_Rigidbody.AddExplosionForce(weapon.ExplosionForce, position, weapon.ExplosionRadius);
        }

        public void Damage(Weapon weapon)
        {
            healthSystem.RemoveHealth(weapon.GetDamage());
            m_ReactorManager.PlayOnHit();
            if (!healthSystem.IsDead) return;
            HandleDeath();
        }

        private void HandleDeath()
        {
            if (m_ReactorManager == null) return;
            m_ReactorManager.PlayOnDestroy();
            GameObjectExtensions.SafeDestroyGameObject(this);
        }
    }
}