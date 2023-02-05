using UnityEngine;
using DG.Tweening;

namespace GGJRuntime
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TweenCanvasGroupAlpha : TweenComponent
    {
        [Range(0f, 1f)]
        public float start = 0f;
        [Range(0f, 1f)]
        public float end = 1f;

        [System.NonSerialized]
        private CanvasGroup _group = null;

        public CanvasGroup group
        {
            get { return _group != null ? _group : (_group = GetComponent<CanvasGroup>()); }
        }


        protected override Tween CreateTween()
        {
            group.alpha = start;
            return group.DOFade(end, duration);
        }
    }
}