using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class VisualEffectReactor : ReactorBase
    {
        [SerializeField]
        private AssetReferenceT<GameObject> effect;

        public override bool PlayWithResult()
        {
            if (effect == null)
            {
                Debug.Log("No effect");
                return false;
            }
            GameObject go = Instantiate(GetEffect(), null);
            if (!go.TryGetComponent(out VisualEffect ve))
            {
                Debug.Log("No ve");
                return false;
            }

            ve.transform.position = transform.position;
            var destroyer = go.AddComponent<VisualEffectDestroyer>();
            ve.Play();
            destroyer.Set(ve);
            return true;
        }

        private GameObject GetEffect() => Addressables.LoadAssetAsync<GameObject>(effect).WaitForCompletion();
    }
}