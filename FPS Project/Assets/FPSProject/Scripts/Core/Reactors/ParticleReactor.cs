using UnityEngine.AddressableAssets;
using UnityEngine;
using RolePoloGame.Core;
using System.Collections;
using RolePoloGame.Core.Extensions;
using System;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class ParticleReactor : ReactorBase
    {
        [SerializeField]
        private AssetReferenceT<GameObject> particle;

        [SerializeField]
        private bool useObjectPool = false;
        [SerializeField]
        private bool spawnWithNoParent = true;

        private ObjectPool<ParticleSystem> m_ObjectPool;
        private ObjectPool<ParticleSystem> ObjectPool => m_ObjectPool ??= new ObjectPool<ParticleSystem>(GetParticleSystem().gameObject, GetParent());

        private void OnDestroy()
        {
            if (m_ObjectPool == null) return;
            var pool = m_ObjectPool.Pool;
            while (pool.Count > 0)
            {
                var particle = pool[0];
                pool.RemoveAt(0);
                if (particle == null) continue;
                if (!particle.IsAlive())
                    GameObjectExtensions.SafeDestroyGameObject(particle);
                else
                {
                    var mainModule = particle.main;
                    mainModule.stopAction = ParticleSystemStopAction.Destroy;
                }
            }
        }

        public override bool PlayWithResult()
        {
            if (useObjectPool)
                return SpawnAtPlayParticleSystem(ObjectPool.Get());
            return SpawnAtPlayParticleSystem(Instantiate(GetParticleSystem(), GetParent()));
        }

        public override void MarkForDestruction()
        {
            base.MarkForDestruction();
        }



        private bool SpawnAtPlayParticleSystem(ParticleSystem particleSystem)
        {
            if (particleSystem == null) return false;
            particleSystem.transform.position = transform.position;
            particleSystem.Play();
            if (useObjectPool)
                StartCoroutine(DisableWhenFinished(particleSystem));
            return true;
        }

        private Transform GetParent() => spawnWithNoParent ? null : transform;

        private IEnumerator DisableWhenFinished(ParticleSystem particleSystem)
        {
            if (m_MarkedForDestruction)
            {
                var mainModule = particleSystem.main;
                mainModule.stopAction = ParticleSystemStopAction.Destroy;
            }
            if (particleSystem == null) yield break;
            while (particleSystem.IsAlive())
            {
                yield return new WaitForEndOfFrame();
            }
            if (particleSystem == null) yield break;
            if (ObjectPool == null) yield break;
            ObjectPool.Release(particleSystem);

            if (m_MarkedForDestruction)
                GameObjectExtensions.SafeDestroyGameObject(particleSystem);
        }

        private ParticleSystem GetParticleSystem()
        {
            if (particle == null)
                throw new NullReferenceException($"{name}'s {nameof(ParticleReactor).Humanize()} contains no prefab!");

            var go = Addressables.LoadAssetAsync<GameObject>(particle).WaitForCompletion();
            if (go.TryGetComponent(out ParticleSystem system))
                return system;
            throw new MissingComponentException($"{name}'s {nameof(ParticleReactor).Humanize()} contains a prefab that holds no {nameof(ParticleSystem)}");
        }
    }
}