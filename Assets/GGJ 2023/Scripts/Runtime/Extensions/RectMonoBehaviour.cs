using UnityEngine;

namespace GGJRuntime
{
    public class RectMonoBehaviour : MonoBehaviour
    {
        [System.NonSerialized]
        private RectTransform _rectTransform = null;

        /// <summary>
        /// Transform for the object as a <see cref="RectTransform"/>.
        /// </summary>
        public RectTransform rectTransform
        {
            get { return _rectTransform != null ? _rectTransform : (_rectTransform = (RectTransform)transform); }
        }
    }
}