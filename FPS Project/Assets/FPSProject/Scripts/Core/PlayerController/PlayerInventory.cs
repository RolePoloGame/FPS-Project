using Assets.FPSProject.Scripts.Core.Items;
using Assets.FPSProject.Scripts.Core.Items.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.PlayerController
{
    [Serializable]
    public class PlayerInventory
    {
        public List<Weapon> Weapons => m_Weapons;
        public List<Item> Items => m_Items;
        public int WeaponCount => Weapons.Count;

        [SerializeField] private List<Weapon> m_Weapons;
        [SerializeField] private List<Item> m_Items;

        private Dictionary<Weapon, int> m_CurrentClips = new();
        private Dictionary<Weapon, int> m_CurrentAmmo = new();
        public void AddWeapon(Weapon weapon)
        {
            AddWeapon(weapon, 400);
        }
        public void AddWeapon(Weapon weapon, int ammo)
        {
            if (m_Weapons.Contains(weapon)) return;
            m_Weapons.Add(weapon);
            m_CurrentAmmo.Add(weapon, ammo);
        }
        public void AddItem(Item item)
        {
            if (m_Items.Contains(item)) return;
            m_Items.Add(item);
        }

        public Weapon GetWeapon(int current) => Weapons[current];
        public void ChangeCurrentClip(Weapon weapon, int value)
        {
            if (!m_CurrentClips.ContainsKey(weapon))
            {
                m_CurrentClips.Add(weapon, value);
                ChangeCurrentAmmo(weapon, 0);
                return;
            }
            m_CurrentClips[weapon] += value;
            if (m_CurrentClips[weapon] < 0)
                m_CurrentClips[weapon] = 0;
            if (m_CurrentClips[weapon] > weapon.ClipSize)
                m_CurrentClips[weapon] = weapon.ClipSize;
        }

        public void ChangeCurrentAmmo(Weapon weapon, int value)
        {
            if (!m_CurrentClips.ContainsKey(weapon))
            {
                m_CurrentClips.Add(weapon, value);
                return;
            }
            m_CurrentClips[weapon] += value;
        }
        public int GetCurrentClip(Weapon weapon)
        {
            if (!m_CurrentClips.ContainsKey(weapon))
                m_CurrentClips.Add(weapon, weapon.ClipSize);
            return m_CurrentClips[weapon];
        }

        public int GetCurrentAmmo(Weapon weapon)
        {
            if (!m_CurrentAmmo.ContainsKey(weapon))
                m_CurrentAmmo.Add(weapon, 400);
            return m_CurrentAmmo[weapon];
        }

        public bool ReloadWeapon(Weapon weapon)
        {
            if (!m_CurrentClips.ContainsKey(weapon))
                m_CurrentClips.Add(weapon, 0);

            var currentAmmo = m_CurrentClips[weapon];
            var maxAmmo = m_CurrentAmmo[weapon];
            int clipSize = weapon.ClipSize;

            if (maxAmmo < clipSize - currentAmmo)
            {
                m_CurrentAmmo[weapon] -= clipSize - currentAmmo;
                m_CurrentClips[weapon] = maxAmmo - (clipSize - currentAmmo);
                return true;
            }
            m_CurrentAmmo[weapon] -= (clipSize - currentAmmo);
            m_CurrentClips[weapon] = clipSize;
            return true;
        }

        public bool CanReloadWeapon(Weapon weapon)
        {
            if (!m_CurrentAmmo.ContainsKey(weapon))
                m_CurrentAmmo.Add(weapon, 400);
            var maxAmmo = m_CurrentAmmo[weapon];
            if (maxAmmo == 0)
                return false;
            return true;
        }
    }
}