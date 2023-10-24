using RolePoloGame.Core.Extensions;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Assets.FPSProject.Scripts.Core.Items.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        private const int MAX_COLLISIONS = 50;
        private Weapon weapon;

        [SerializeField] private GameObject root;
        [SerializeField] private Transform muzzle;
        [SerializeField] private AssetReferenceT<GameObject> bulletDecal;
        private GameObject m_BulletDecalInstance;
        public UnityEvent OnPrimaryAction;
        public UnityEvent OnReload;

        private static RaycastHit[] raycastHits = new RaycastHit[MAX_COLLISIONS];

        public void Set(Weapon weapon) => this.weapon = weapon;
        public void Hide() => SetRoot(false);
        public void Show() => SetRoot(true);

        public void PrimaryAction()
        {
            if (weapon == null) return;
            switch (weapon.Type)
            {
                case Weapon.WeaponType.Ranged:
                    HandleRanged();
                    break;
            }
            OnPrimaryAction?.Invoke();
        }

        public void ReloadAction()
        {
            if (weapon == null) return;
            OnReload?.Invoke();
        }

        private void HandleRanged()
        {
            switch (weapon.RangedType)
            {
                case Weapon.ShootType.Raycast:
                    HandleRaycast();
                    break;
            }
        }

        private void HandleRaycast()
        {
            var gaussianDistribusion = new MathExtensions.GaussianDistribution();
            Vector3 forwardVector = transform.forward;
            for (var i = 0; i < weapon.AmmoPerShot; i++)
            {
                var direction = forwardVector;
                var spread = gaussianDistribusion.Next(0.0f, 1.0f, -1.0f, 1.0f);
                spread *= weapon.Spread;

                direction.x += UnityEngine.Random.Range(-spread, spread);
                direction.y += UnityEngine.Random.Range(-spread, spread);
                direction.z += UnityEngine.Random.Range(-spread, spread);
                RaycastAll(direction);
            }
        }

        private void RaycastAll(Vector3 forwardVector)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, forwardVector, float.PositiveInfinity, weapon.hitLayerMask);

            Array.Sort(hits, (a, b) => (a.distance.CompareTo(b.distance)));
            var penetrations = 0;
            for (var i = 0; i < hits.Length; i++)
            {
                if (penetrations >= weapon.Penetration) break;
                var hit = hits[i];
                if (hit.transform == null) continue;
                penetrations++;

                SpawnDecal(hit.transform, hit.point, hit.normal);

                if (!hit.transform.TryGetComponent(out IDamagable damagable))
                {
                    damagable = hit.transform.GetComponentInParent<IDamagable>();
                    if (damagable == null)
                    {
                        if (weapon.StopsAtGround)
                            return;
                        continue;
                    }
                }
                if (!damagable.CanBeDamaged(weapon)) continue;
                damagable.Damage(weapon, hit.point);
            }
        }

        private void SpawnDecal(Transform parent, Vector3 point, Vector3 normal)
        {
            var decal = GetDecal();
            decal.transform.SetParent(parent);
            decal.transform.position = point;
            decal.transform.localScale = Vector3.one;
            decal.transform.forward = -normal;
        }

        private GameObject GetDecal()
        {
            if (m_BulletDecalInstance == null)
            {
                m_BulletDecalInstance = Addressables.LoadAssetAsync<GameObject>(bulletDecal).WaitForCompletion();
            }
            return Instantiate(m_BulletDecalInstance);
        }

        private void SetRoot(bool value) => root.SetActive(value);
    }
}