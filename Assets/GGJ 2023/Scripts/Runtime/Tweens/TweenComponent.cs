using UnityEngine;
using DG.Tweening;

namespace GGJRuntime
{
    public class TweenComponent : MonoBehaviour
    {
        public bool playOnEnable = false;
        public float duration = 1f;
        public float delay = 0f;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public TweenLoopType type = TweenLoopType.Default;
        public int loopThreshold = -1;

        public string InstanceGUID { get; protected set; }

        public virtual void StartTween(bool reset=false)
        {
            StopTween();
        }


        public virtual void StopTween()
        {
            if(string.IsNullOrEmpty(InstanceGUID)) InstanceGUID = System.Guid.NewGuid().ToString();

            if(DOTween.IsTweening(InstanceGUID)) DOTween.Kill(InstanceGUID);
        }


        protected virtual void InitializeTween(Tween tween)
        {
            tween.SetEase(curve).SetDelay(delay).SetId(InstanceGUID);

            switch(type)
            {
                case TweenLoopType.Loop:
                    tween.SetLoops(loopThreshold, LoopType.Restart);
                    break;
                case TweenLoopType.PingPong:
                    tween.SetLoops(loopThreshold, LoopType.Yoyo);
                    break;
            }
        }


        protected virtual void OnEnable()
        {
            if(playOnEnable) StartTween(true);
        }


        protected virtual void OnDisable()
        {
            StopTween();
        }
    }
}