using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName="Background", menuName="GGJ/UI/Background Data")]
    public class BackgroundData : ScriptableObject, IBackgroundData
    {
        public Sprite sprite = null;
        public Color color = Color.white;

        public Sprite BackgroundSprite
        {
            get { return sprite; }
        }

        public Color Tint
        {
            get { return color; }
        }
    }
}