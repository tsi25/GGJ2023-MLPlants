using UnityEngine;
using DG.Tweening;

namespace GGJRuntime
{
    public class TweenAnchoredPosition : RectTransformTweenComponent
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.zero;

        public override void StartTween(bool reset = false)
        {
            base.StartTween(reset);

            rectTransform.anchoredPosition = start;
            Tween tween = rectTransform.DOAnchorPos(end, duration);

            InitializeTween(tween);
        }
    }
}