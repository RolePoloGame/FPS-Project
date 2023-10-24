using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.FPSProject.Scripts.Core.Items.Weapons
{
    [CreateAssetMenu(menuName = "FPS/Item/Weapon/Weapon")]
    public class Weapon : Item
    {
        [Header("Weapon")]
        public AssetReference Visual;
        public WeaponType Type;
        public ShootType RangedType;
        public float Distance;
        public float Damage;
        public LayerMask hitLayerMask;
        public bool HoldAttack = true;
        public float ExplosionForce = 0.0f;
        public float ExplosionRadius = 0.0f;
        [Header("Ranged")]
        public float ShootDelay = 0;
        public bool HasAmmo = false;
        public int ClipSize = 0;
        public int AmmoPerShot = 1;
        public int Penetration = 1;
        public float Spread = 1.0f;
        public float ReloadTime = 1.0f;
        public bool StopsAtGround = true;

        public WeaponController GetVisual(Transform root)
        {
            if (Visual == null) return null;

            var loadedModel = Addressables.LoadAssetAsync<GameObject>(Visual).WaitForCompletion();
            var go = Instantiate(loadedModel);
            go.transform.SetParent(root);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            if (!go.TryGetComponent(out WeaponController controller))
            {
                Debug.LogError($"{nameof(WeaponController)} is missing on {Visual}");
                return null;
            }
            return controller;
        }

        public float GetDamage() => Damage;

        public enum WeaponType
        {
            Ranged
        }

        public enum ShootType
        {
            Raycast
        }
    }
}