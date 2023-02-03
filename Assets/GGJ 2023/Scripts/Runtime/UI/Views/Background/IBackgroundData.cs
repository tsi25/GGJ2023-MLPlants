using UnityEngine;

namespace GGJRuntime
{
    public interface IBackgroundData
    {
        /// <summary>
        /// Sprite shown in the background.
        /// </summary>
        Sprite BackgroundSprite { get; }
        /// <summary>
        /// Color tint for the background.
        /// </summary>
        Color Tint { get; }
    }
}