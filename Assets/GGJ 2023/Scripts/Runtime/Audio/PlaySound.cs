using UnityEngine;

namespace GGJRuntime
{
    public class PlaySound : MonoBehaviour
    {
        [SerializeField]
        private SoundId sound = SoundId.None;

        private void OnEnable()
        {
            if(sound != SoundId.None) SoundManager.Play(sound);
        }

        private void OnDisable()
        {
            if(sound != SoundId.None) SoundManager.Stop(sound);
        }
    }
}