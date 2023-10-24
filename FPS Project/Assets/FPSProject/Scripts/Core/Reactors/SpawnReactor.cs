using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class SpawnReactor : ReactorBase
    {
        [SerializeField]
        private AssetReferenceT<GameObject> spawned;
        [SerializeField]
        private bool spawnAsChild = false;

        public GameObject GetPrefab() => Addressables.LoadAssetAsync<GameObject>(spawned).WaitForCompletion();
        public Transform GetParent() => spawnAsChild ? transform : null;

        public override bool PlayWithResult()
        {
            if (spawned == null) return false;
            var go = Instantiate(GetPrefab(), GetParent());
            go.transform.position = transform.position;
            return true;
        }
    }
}