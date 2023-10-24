using RolePoloGame.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.FPSProject.Scripts.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private int m_MaxAudioSources = 10;
        private Dictionary<AssetReferenceT<AudioClip>, AudioClip> audioClips;
        private List<AudioSource> audioSources = new();
        private AudioSource AudioSource => GetFreeAudioSource();
        public void PlayCachedAt(AssetReferenceT<AudioClip> clipReference, Vector3 worldPosition)
        {
            TryCacheClip(clipReference);
            PlayAt(audioClips[clipReference], worldPosition);
        }
        public void PlayCached(AssetReferenceT<AudioClip> clipReference)
        {
            TryCacheClip(clipReference);
            Play(audioClips[clipReference]);
        }

        public void PlayAt(AudioClip clip, Vector3 worldPosition) => AudioSource.PlayClipAtPoint(clip, worldPosition);
        public void Play(AudioClip clip)
        {
            if (AudioSource == null)
                StartCoroutine(PlayAfter(clip, 0.2f));
            else
                AudioSource.PlayOneShot(clip);
        }

        private IEnumerator PlayAfter(AudioClip clip, float v)
        {
            yield return new WaitForSeconds(v);
            Play(clip);
        }

        private void TryCacheClip(AssetReferenceT<AudioClip> clipReference)
        {
            audioClips ??= new();
            if (!audioClips.ContainsKey(clipReference))
            {
                var clip = Addressables.LoadAssetAsync<AudioClip>(clipReference).WaitForCompletion();
                audioClips.Add(clipReference, clip);
            }
        }

        private AudioSource GetFreeAudioSource()
        {
            audioSources ??= new();
            foreach (var source in audioSources)
            {
                if (source.isPlaying) continue;
                return source;
            }
            if (audioSources.Count >= m_MaxAudioSources) return null;
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
            return audioSource;
        }

    }
}