using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    [RequireComponent(typeof(Button))]
    public class PlaySoundOnClick : RectMonoBehaviour
    {
        [SerializeField]
        private Button button = null;
        [SerializeField]
        private SoundId sound = SoundId.None;

        private void OnButtonClicked()
        {
            if(sound == SoundId.None) return;

            SoundManager.Play(sound);
        }


        private void OnValidate()
        {
            if(button == null) button = GetComponent<Button>();
        }


        private void Start()
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }
}