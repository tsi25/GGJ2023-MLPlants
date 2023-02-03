using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace GGJRuntime
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private SoundSource sourcePrefab = null;
        [SerializeField, Tooltip("Mixer group for all audio.")]
        private AudioMixerGroup mainMixerGroup = null;
        [SerializeField, Tooltip("Mixer group for Music audio.")]
        private AudioMixerGroup musicMixerGroup = null;
        [SerializeField, Tooltip("Mixer group for SFx audio.")]
        private AudioMixerGroup sfxMixerGroup = null;
        [SerializeField]
        private SoundCollection[] sounds = new SoundCollection[0];

        private List<SoundSource> sources = new List<SoundSource>();

        private static SoundManager instance = null;

        public static SoundManager Instance
        {
            get
            {
                if(instance == null) instance = Client.Get<SoundManager>();

                return instance;
            }
        }


        public static void Play(SoundId id, Transform parent=null)
        {
            Instance.Play_Internal(id, parent);
        }


        public static void Stop(SoundId id)
        {
            Instance.Stop_Internal(id);
        }


        public static void StopAll(SoundId id)
        {
            Instance.StopAll_Internal(id);
        }


        private void Play_Internal(SoundId id, Transform parent=null)
        {
            Sound sound = FindSound(id);

            if(sound == null)
            {
                Debug.LogWarning($"No sound with id [{id}] found!");
                return;
            }

            if(sound.limit > 0 && GetCount(id) >= sound.limit) return;

            //TODO: Spawn sound...
            SoundSource source = Instantiate(sourcePrefab, parent ?? transform);

            source.OnStop += OnSourceStopped;

            sources.Add(source);

            source.Play(sound, GetMixerGroup(sound.type));
        }


        private void Stop_Internal(SoundId id)
        {
            SoundSource source = FindSource(id);

            if(source == null) return;

            source.Stop();
        }


        private void StopAll_Internal(SoundId id)
        {
            for(int i = sources.Count - 1; i >= 0; i--)
            {
                if(sources[i].CurrentSound == id) sources[i].Stop();
            }
        }


        private int GetCount(SoundId id)
        {
            int count = 0;

            sources.ForEach(s => { if(s.CurrentSound == id) count++; });

            return count;
        }


        private AudioMixerGroup GetMixerGroup(SoundType type)
        {
            switch(type)
            {
                case SoundType.Music: return musicMixerGroup;
                case SoundType.SFX: return sfxMixerGroup;
            }

            return mainMixerGroup;
        }


        private Sound FindSound(SoundId id)
        {
            Sound sound = null;

            for(int i=0; i < sounds.Length; i++)
            {
                if(sounds[i].TryFind(id, out sound))
                {
                    return sound;
                }
            }

            return sound;
        }


        private SoundSource FindSource(SoundId id)
        {
            SoundSource source = sources.Find(s => s.CurrentSound == id);

            return source;
        }


        private void OnSourceStopped(SoundSource source)
        {
            source.OnStop -= OnSourceStopped;

            sources.Remove(source);
            Destroy(source.gameObject);
        }
    }
}