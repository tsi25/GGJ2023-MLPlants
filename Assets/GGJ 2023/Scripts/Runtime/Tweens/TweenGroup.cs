using UnityEngine;

namespace GGJRuntime
{
    public class TweenGroup : MonoBehaviour
    {
        public TweenComponent[] tweens = new TweenComponent[0];

        private int completedTweens = 0;

        public void StartTweens(System.Action callback=null, bool reset=false)
        {
            completedTweens = 0;

            for(int i=0; i < tweens.Length; i++)
            {
                tweens[i].StartTween(() => { OnTweenComplete(callback); }, false);
            }
        }


        public void StopTweens()
        {
            for(int i = 0; i < tweens.Length; i++)
            {
                tweens[i].StopTween();
            }
        }


        private void OnTweenComplete(System.Action callback)
        {
            completedTweens++;

            if(callback == null) return;

            if(completedTweens >= tweens.Length) callback.Invoke();
        }
    }
}