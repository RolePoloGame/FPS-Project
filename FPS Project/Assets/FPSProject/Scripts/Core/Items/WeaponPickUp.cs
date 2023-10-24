using Assets.FPSProject.Scripts.Core.Items.Weapons;
using Assets.FPSProject.Scripts.Core.PlayerController;
using RolePoloGame.Core.Extensions;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Items
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField]
        private Weapon weapon;
        [SerializeField]
        private int startAmmo = 100;

        private bool m_IsEnabled = true;
        private bool m_MarkForDestrucion = false;

        public void Update()
        {
            if (!m_MarkForDestrucion) return;
            GameObjectExtensions.SafeDestroyGameObject(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEnabled) return;
            if (!other.TryGetComponent(out PlayerInventoryController controller))
            {
                controller = other.GetComponentInChildren<PlayerInventoryController>();
                if (controller == null)
                {
                    controller = other.GetComponentInParent<PlayerInventoryController>();
                    if (controller == null)
                        return;
                }
            }
            m_IsEnabled = false;
            controller.AddWeapon(weapon, startAmmo);
            m_MarkForDestrucion = true;
        }
    }
}