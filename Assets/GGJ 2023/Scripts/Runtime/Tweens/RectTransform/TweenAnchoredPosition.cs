using UnityEngine;
using DG.Tweening;

namespace GGJRuntime
{
    public class TweenAnchoredPosition : RectTransformTweenComponent
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.zero;


        protected override Tween CreateTween()
        {
            rectTransform.anchoredPosition = start;
            return rectTransform.DOAnchorPos(end, duration);
        }
    }
}