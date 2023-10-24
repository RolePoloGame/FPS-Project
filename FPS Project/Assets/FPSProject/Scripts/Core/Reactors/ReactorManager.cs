using RolePoloGame.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class ReactorManager : MonoBehaviour
    {
        [SerializeField]
        private bool preloadReactors = false;
        [SerializeField]
        private bool loadFromChildren = false;
        private HashSet<ReactorBase> OnSpawnReactors => m_OnSpawnReactors ??= new();
        private HashSet<ReactorBase> OnHitReactors => m_OnHitReactors ??= new();
        private HashSet<ReactorBase> OnInteractionReactors => m_OnInteractionReactors ??= new();
        private HashSet<ReactorBase> OnDestroyReactors => m_OnDestroyReactors ??= new();

        private HashSet<ReactorBase> m_OnSpawnReactors;
        private HashSet<ReactorBase> m_OnHitReactors;
        private HashSet<ReactorBase> m_OnInteractionReactors;
        private HashSet<ReactorBase> m_OnDestroyReactors;

        private bool m_ReactorsLoaded = false;

        void Start()
        {
            if (!preloadReactors) return;
            LoadReactors();
        }
        public void PlayOnSpawn() => PlayReactors(OnSpawnReactors);
        public void PlayOnHit() => PlayReactors(OnHitReactors);
        public void PlayOnInteraction() => PlayReactors(OnInteractionReactors);
        public void PlayOnDestroy()
        {
            PlayReactors(OnDestroyReactors);
            foreach (var reactor in OnDestroyReactors)
                reactor.MarkForDestruction();
        }

        private void PlayReactors(HashSet<ReactorBase> reactorsList)
        {
            if (!preloadReactors) LoadReactors();
            if (reactorsList == null || reactorsList.Count == 0) return;

            foreach (var reactor in reactorsList)
            {
                var played = reactor.PlayWithResult();
            }
        }

        private void LoadReactors()
        {
            if (m_ReactorsLoaded) return;

            AddReactors(GetComponents<ReactorBase>());
            if (loadFromChildren)
                AddReactors(GetComponentsInChildren<ReactorBase>());
            m_ReactorsLoaded = true;

            void AddReactors(ReactorBase[] newReactors)
            {
                if (newReactors == null || newReactors.Length == 0) return;
                foreach (ReactorBase reactor in newReactors)
                {
                    if (reactor.HasFlag(ReactorBase.EReactorMask.Spawn))
                    {
                        OnSpawnReactors.Add(reactor);
                    }
                    if (reactor.HasFlag(ReactorBase.EReactorMask.Hit))
                    {
                        OnHitReactors.Add(reactor);
                    }
                    if (reactor.HasFlag(ReactorBase.EReactorMask.Interact))
                    {
                        OnInteractionReactors.Add(reactor);
                    }
                    if (reactor.HasFlag(ReactorBase.EReactorMask.Destroy))
                    {
                        OnDestroyReactors.Add(reactor);
                    }
                }
            }
        }
    }
}