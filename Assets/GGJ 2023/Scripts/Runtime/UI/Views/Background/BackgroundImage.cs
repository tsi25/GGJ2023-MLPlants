using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    public class BackgroundImage : RectMonoBehaviour
    {
        [SerializeField]
        private Image backgroundImage = null;

        public IBackgroundData Data { get; protected set; }

        public void Initialize(IBackgroundData data)
        {
            backgroundImage.sprite = data.BackgroundSprite;
            backgroundImage.color = data.Tint;

            Data = data;
        }
    }
}