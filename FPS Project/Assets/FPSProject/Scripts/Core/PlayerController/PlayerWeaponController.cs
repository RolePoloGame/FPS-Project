using Assets.FPSProject.Scripts.Core.Items.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.PlayerController
{
    [RequireComponent(typeof(PlayerInventoryController))]
    public class PlayerWeaponController : MonoBehaviour
    {
        private PlayerInventoryController Inventory => m_Inventory = m_Inventory != null ? m_Inventory : GetComponent<PlayerInventoryController>();
        private PlayerInventoryController m_Inventory;

        private Weapon m_Weapon;
        private WeaponController m_WeaponController;

        [SerializeField] private GameObject VisualsRoot;
        Dictionary<Weapon, WeaponController> m_WeaponControllers;

        private int m_CurrentWeaponIndex = -1;
        private bool m_EnableShooting = false;

        private float m_BetweenShootTimer = 0.0f;

        public bool HasAmmo => m_HasAmmo;
        public int CurrentAmmo => m_CurrentAmmo;
        public int ClipSize => m_ClipSize;
        public int MaxAmmo => m_MaxAmmo;
        public bool HasWeapon => m_Weapon != null;

        public Weapon Weapon => m_Weapon;
        public bool IsReloading => m_IsReloading;

        public event Action OnWeaponUpdate;

        private bool m_HasAmmo = false;
        private int m_ClipSize = 0;
        private int m_CurrentAmmo = 0;
        private int m_MaxAmmo = 0;
        private float m_ReloadDelay = 0.0f;
        private bool m_IsReloading;

        private void Start()
        {
            SubscribeToInput();
            m_WeaponControllers = new();
        }
        private void OnDestroy()
        {
            UnsubscribeFromInput();
        }

        private void Update()
        {
            if (IsReloading)
            {
                if (m_ReloadDelay > 0.0f)
                {
                    m_ReloadDelay -= Time.deltaTime;
                    return;
                }
                m_IsReloading = false;
                if (Inventory.ReloadWeapon(m_Weapon))
                    m_WeaponController.ReloadAction();
                HandleWeaponUpdate();
            }
            

            if (!m_EnableShooting) return;
            if (m_WeaponController == null) return;
            if (m_Weapon.ShootDelay > m_BetweenShootTimer)
            {
                m_BetweenShootTimer += Time.deltaTime;
                return;
            }

            Shoot();
            m_BetweenShootTimer = 0.0f;
        }

        private void UpdateVisuals()
        {
            foreach (var controller in m_WeaponControllers.Values)
                controller.Hide();

            if (m_Weapon == null)
            {
                return;
            }

            if (!m_WeaponControllers.ContainsKey(m_Weapon))
            {
                m_WeaponController = m_Weapon.GetVisual(VisualsRoot.transform);
                m_WeaponController.Set(m_Weapon);
                m_WeaponControllers.Add(m_Weapon, m_WeaponController);
            }
            else
            {
                m_WeaponController = m_WeaponControllers[m_Weapon];
                m_WeaponController.Set(m_Weapon);
            }

            if (m_WeaponController != null)
                m_WeaponController.Show();
        }

        private void UnsubscribeFromInput()
        {
            if (InputManager.Instance == null) return;
            InputManager.Instance.OnFireStarted -= HandleFireStartEvent;
            InputManager.Instance.OnReload -= HandleReload;
            InputManager.Instance.OnWeaponSwap -= HandleWeaponSwap;
            InputManager.Instance.OnWeaponWheel -= HandleWeaponWheel;
        }

        private void SubscribeToInput()
        {
            if (InputManager.Instance == null) return;
            InputManager.Instance.OnFireStarted += HandleFireStartEvent;
            InputManager.Instance.OnFireCanceled += HandleFireCanceledEvent;
            InputManager.Instance.OnReload += HandleReload;
            InputManager.Instance.OnWeaponSwap += HandleWeaponSwap;
            InputManager.Instance.OnWeaponWheel += HandleWeaponWheel;
        }

        private void HandleWeaponWheel()
        {

            //HandleWeaponUpdate();
        }

        private void HandleReload()
        {
            if (m_WeaponController == null)
            {
                return;
            }
            if (!Inventory.CanReloadWeapon(m_Weapon))
                return;

            m_IsReloading = true;
            m_ReloadDelay = m_Weapon.ReloadTime;
        }

        private void HandleWeaponSwap(bool isNext)
        {
            m_CurrentWeaponIndex += isNext ? 1 : -1;
            Weapon newWeapon = HandleGetWeaponByIndex();
            if (newWeapon == null) Debug.Log("No weapon!");
            m_Weapon = newWeapon;
            m_IsReloading = false;
            m_EnableShooting = false;
            HandleWeaponUpdate();
        }

        private void HandleWeaponUpdate()
        {
            UpdateWeapon();
            UpdateVisuals();
            OnWeaponUpdate?.Invoke();
        }

        private void UpdateWeapon()
        {
            m_HasAmmo = m_Weapon.HasAmmo;
            if (!m_HasAmmo) return;
            m_ClipSize = m_Weapon.ClipSize;
            m_CurrentAmmo = Inventory.GetCurrentClip(m_Weapon);
            m_MaxAmmo = Inventory.GetCurrentAmmo(m_Weapon);
        }
        public float GetReloadActionRatio()
        {
            if (m_Weapon == null) return 0.0f;
            return 1.0f - (m_ReloadDelay / m_Weapon.ReloadTime);
        }
        private Weapon HandleGetWeaponByIndex()
        {
            if (m_CurrentWeaponIndex < 0)
            {
                if (Inventory.WeaponCount == 0)
                {
                    return null;
                }
                m_CurrentWeaponIndex = Inventory.WeaponCount - 1;
            }
            if (m_CurrentWeaponIndex >= Inventory.WeaponCount)
            {
                if (Inventory.WeaponCount == 0)
                {
                    return null;
                }
                m_CurrentWeaponIndex = 0;
            }
            return Inventory.GetWeapon(m_CurrentWeaponIndex);
        }

        private void Shoot()
        {
            if (!m_Weapon.HasAmmo)
            {
                m_WeaponController.PrimaryAction();
            }
            else
            {
                if (Inventory.GetCurrentClip(m_Weapon) >= m_Weapon.AmmoPerShot)
                {

                    Inventory.ChangeCurrentClip(m_Weapon, -m_Weapon.AmmoPerShot);
                    m_WeaponController.PrimaryAction();
                }
            }
            HandleWeaponUpdate();
        }

        private void HandleFireStartEvent()
        {
            if (m_WeaponController == null) return;
            if (IsReloading) return;
            Shoot();
            if (!m_Weapon.HoldAttack)
            {
                return;
            }
            m_BetweenShootTimer = 0.0f;
            m_EnableShooting = true;
        }
        private void HandleFireCanceledEvent()
        {
            if (m_WeaponController == null) return;
            m_EnableShooting = false;
        }
    }
}