using UnityEngine;
using UnityEngine.Audio;

namespace GGJRuntime
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {
        public event System.Action<SoundSource> OnStop = delegate { };

        private AudioSource source = null;

        public AudioSource Source
        {
            get { return source != null ? source : (source = GetComponent<AudioSource>()); }
        }

        public SoundId CurrentSound { get; protected set; }

        public void Play(Sound sound, AudioMixerGroup mixerGroup)
        {
            CurrentSound = sound.id;

            Source.clip = sound.clip;
            Source.spatialBlend = sound.spatialBlend;
            Source.volume = sound.volume;
            Source.loop = sound.loop;
            Source.outputAudioMixerGroup = mixerGroup;

            Source.Play();
        }


        public void Stop()
        {
            Source.Stop();
        }


        private void Update()
        {
            if(Source.isPlaying) return;

            OnStop.Invoke(this);
        }
    }
}