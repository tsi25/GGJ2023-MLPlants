using UnityEngine;

namespace GGJRuntime
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformTweenComponent : TweenComponent
    {
        [System.NonSerialized]
        private RectTransform _rectTransform = null;

        public RectTransform rectTransform
        {
            get { return _rectTransform != null ? _rectTransform : (_rectTransform = (RectTransform)transform); }
        }
    }
}