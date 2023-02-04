using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GGJRuntime
{
    public class TitleView : GameView
    {
        private const string STATE_KEY = "state";

        private const string OPENING_KEY = "opening";
        private const string CLOSING_KEY = "closing";

        [SerializeField]
        private Animator _buttonAnimator = null;
        [SerializeField]
        private float _delay = 1f;

        [SerializeField]
        private GameState _state = null;

        public override void Open()
        {
            base.Open();

            OpenSequence();
        }

        public override void Close()
        {
            CloseSequence();
            _state.StateType = StateType.Gameplay;
            StartCoroutine(DelayAction(base.Close));
        }

        public void OpenSequence()
        {
            _buttonAnimator.SetBool(OPENING_KEY, true);
            _buttonAnimator.SetBool(CLOSING_KEY, false);
        }

        public void CloseSequence()
        {
            _buttonAnimator.SetBool(OPENING_KEY, false);
            _buttonAnimator.SetBool(CLOSING_KEY, true);
        }

        private IEnumerator DelayAction(Action callback)
        {
            yield return new WaitForSeconds(_delay);
            callback.Invoke();
        }

        protected override void Start()
        {
            base.Start();

            _state.StateType = StateType.Title;
        }
    }
}