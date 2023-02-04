using System;
using System.Collections;
using UnityEngine;

namespace GGJRuntime
{
    public class GameHUDView : GameView
    {
        [SerializeField]
        private Draggable[] Draggables = new Draggable[0];
        [SerializeField]
        private Droppable[] Droppables = new Droppable[0];

        [SerializeField]
        private float _delay = 1f;

        public override void Open()
        {
            StartCoroutine(DelayAction(base.Open));
        }


        private IEnumerator DelayAction(Action callback)
        {
            yield return new WaitForSeconds(_delay);
            callback.Invoke();
        }
    }
}
