using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.FPSProject.Scripts.Core.Reactors
{
    public class AudioReactor : ReactorBase
    {
        [SerializeField]
        private AssetReferenceT<AudioClip> audioClip;
        [SerializeField]
        private bool allowAudioManagerCache = true;
        [SerializeField]
        private bool playAtLocation = true;
        public override bool PlayWithResult()
        {
            if (allowAudioManagerCache)
                return PlayAndCache();
            return PlayNoCache();
        }

        private bool PlayNoCache()
        {
            if (AudioManager.Instance == null) return false;
            var clip = Addressables.LoadAssetAsync<AudioClip>(audioClip).WaitForCompletion();
            if (playAtLocation)
                AudioManager.Instance.PlayAt(clip, transform.position);
            else
                AudioManager.Instance.Play(clip);
            return true;
        }

        private bool PlayAndCache()
        {
            if (AudioManager.Instance == null) return false;
            if (playAtLocation)
                AudioManager.Instance.PlayCachedAt(audioClip, transform.position);
            else
                AudioManager.Instance.PlayCached(audioClip);
            return true;
        }
    }
}