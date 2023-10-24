using Assets.FPSProject.Scripts.Core.Items.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Items
{
    [CreateAssetMenu(menuName = "FPS/Item/Item Material")]
    public class ItemMaterial : ScriptableObject
    {
        public string Name;

        [SerializeField]
        private bool canBeDamagedByAny = true;

        [SerializeField]
        private List<Weapon> damagingWeapons = new();

        public bool CanBeDamaged(Weapon weapon)
        {
            if (canBeDamagedByAny) return true;
            if (damagingWeapons.Count == 0) return false;
            return damagingWeapons.Contains(weapon);
        }
    }
}