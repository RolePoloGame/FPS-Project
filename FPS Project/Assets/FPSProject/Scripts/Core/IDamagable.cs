using Assets.FPSProject.Scripts.Core.Items.Weapons;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core
{
    public interface IDamagable
    {
        public void Damage(Weapon weapon, Vector3 position);
        public void Damage(Weapon weapon);
        public bool CanBeDamaged(Weapon weapon);
        public bool CanBeDamaged();
    }
}