using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    [RequireComponent(typeof(Toggle))]
    public class PlaySoundOnToggle : RectMonoBehaviour
    {
        [SerializeField]
        private Toggle toggle = null;
        [SerializeField]
        private SoundId onSound = SoundId.None;
        [SerializeField]
        private SoundId offSound = SoundId.None;

        private void OnToggle(bool value)
        {
            if(value && onSound != SoundId.None)
            {
                SoundManager.Play(onSound);
            }
            else if(!value && offSound != SoundId.None)
            {
                SoundManager.Play(offSound);
            }
        }


        private void OnValidate()
        {
            if(toggle == null) toggle = GetComponent<Toggle>();
        }


        private void Start()
        {
            toggle.onValueChanged.AddListener(OnToggle);
        }
    }
}