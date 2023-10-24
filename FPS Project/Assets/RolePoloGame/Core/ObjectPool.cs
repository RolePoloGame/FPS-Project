using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RolePoloGame.Core
{
    [Serializable]
    public class ObjectPool<T> where T : Component
    {
        private readonly HashSet<T> m_Pool;
        public Transform Root => root;
        public GameObject Prefab => prefab;
        public List<T> Pool => m_Pool.ToList();

        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Transform root;

        public ObjectPool()
        {
            m_Pool = new HashSet<T>();
        }

        public ObjectPool(GameObject prefab, Transform root)
        {
            m_Pool = new HashSet<T>();
            SetPrefab(prefab);
            SetRoot(root);
        }

        private bool m_CheckedForComponenet = false;
        private bool m_HasComponent = false;

        public void SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            CheckForComponent();
        }

        public void SetRoot(Transform root)
        {
            this.root = root;
        }

        public bool CheckForComponent()
        {
            m_CheckedForComponenet = true;
            return m_HasComponent = prefab && prefab.TryGetComponent<T>(out _);
        }

        public void ResetPool()
        {
            foreach (var go in m_Pool)
            {
                if (go == null) continue;
                if (!go.gameObject.activeSelf) continue;
                Release(go);
            }
            m_Pool.Remove(null);
        }

        public GameObject GetGameObject() => GetGameObject(root);
        public T Get() => Get(root);
        private GameObject GetGameObject(Transform parent) => Get(parent).gameObject;

        public void Release(T component)
        {
            if (!m_Pool.Contains(component)) return;
            component.gameObject.SetActive(false);
        }

        private T Get(Transform parent)
        {
            T firstOf = null;
            foreach (var go in m_Pool)
            {
                if (go.gameObject.activeSelf) continue;
                firstOf = go;
                break;
            }

            firstOf ??= CreateNew();
            Transform transform;
            (transform = firstOf.transform).SetParent(parent);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            firstOf.gameObject.SetActive(true);
            return firstOf;
        }

        private T CreateNew()
        {
            if (!m_CheckedForComponenet) CheckForComponent();
            if (!prefab) throw new NullReferenceException($"Prefab cannot ever be null! (ObjectPool<{typeof(T)}>)");
            if (!m_HasComponent) throw new NullReferenceException($"{prefab.name} has no Component of type {typeof(T)}");

            var go = Object.Instantiate(prefab);
            var component = go.GetComponent<T>();
            if (!m_Pool.Contains(component))
                m_Pool.Add(component);
            return component;
        }
    }
}