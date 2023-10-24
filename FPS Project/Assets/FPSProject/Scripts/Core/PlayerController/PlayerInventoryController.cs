using Assets.FPSProject.Scripts.Core.Items.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.PlayerController
{
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory Inventory;

        public event Action<Weapon> OnWeaponAdded;

        public int WeaponCount => Inventory.WeaponCount;

        public Weapon GetWeapon(int index) => Inventory.GetWeapon(index);

        public List<Weapon> GetWeapons() => Inventory.Weapons;

        public int GetCurrentClip(Weapon weapon) => Inventory.GetCurrentClip(weapon);

        public int GetCurrentAmmo(Weapon weapon) => Inventory.GetCurrentAmmo(weapon);

        public void ChangeCurrentAmmo(Weapon weapon, int value) => Inventory.ChangeCurrentAmmo(weapon, value);

        public void ChangeCurrentClip(Weapon weapon, int value) => Inventory.ChangeCurrentClip(weapon, value);

        public bool ReloadWeapon(Weapon weapon) => Inventory.ReloadWeapon(weapon);

        public bool CanReloadWeapon(Weapon weapon) => Inventory.CanReloadWeapon(weapon);

        public void AddWeapon(Weapon weapon, int startAmmo)
        {
            Inventory.AddWeapon(weapon, startAmmo);
            OnWeaponAdded?.Invoke(weapon);
        }
    }
}