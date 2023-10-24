using UnityEngine;
using RolePoloGame.Core.Extensions;

namespace RolePoloGame.Core
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        #region Properties & Fields

        protected Logger Debug => m_Debug ??= new Logger(gameObject);
        private Logger m_Debug;
        private static T _Instance;
        public static T Instance => _Instance;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (_Instance != null)
            {
                Debug.LogSoftError($"Singleton Instance of '{typeof(Singleton<T>)}' already exists");
                GameObjectExtensions.SafeDestroy(gameObject);
                return;
            }

            _Instance = this as T;
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void OnDestroy()
        {
            if (_Instance != this) return;
            _Instance = null;
            OnObjectDestroy();
        }

        #endregion

        #region Protected & Private Methods

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnObjectDestroy()
        {
        }

        #endregion
    }
}