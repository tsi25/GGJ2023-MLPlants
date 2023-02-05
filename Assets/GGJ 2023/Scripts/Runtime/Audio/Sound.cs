using UnityEngine;

namespace GGJRuntime
{
    [System.Serializable]
    public class Sound
    {
        [Tooltip("Unique ID for the sound.")]
        public SoundId id = SoundId.None;
        public SoundType type = SoundType.SFX;
        [Tooltip("AudioClip associated with the sound.")]
        public AudioClip clip = null;
        [Range(0f, 1f), Tooltip("Spatial blending for the sound. 0.0 is 2D 1.0 is 3D.")]
        public float spatialBlend = 1f;
        [Range(0f, 1f), Tooltip("Volume for the sound.")]
        public float volume = 1f;
        [Tooltip("If true, the sound will loop.")]
        public bool loop = false;
        [Tooltip("Max number of this sound that can be played at once.")]
        public int limit = 0;
    }
}