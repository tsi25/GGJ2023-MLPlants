using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJRuntime
{
    public class GameHUDView : GameView
    {
        public Action OnGameStarted = delegate { };

        [SerializeField]
        private Draggable[] Draggables = new Draggable[0];
        [SerializeField]
        private Droppable[] Droppables = new Droppable[0];

        [SerializeField]
        private Button _startButton = null;
        [SerializeField]
        private TextMeshProUGUI _buttonPrompt = null;

        [SerializeField]
        private float _delay = 1f;

        [Header("Prompts")]
        [SerializeField]
        private string _defaultPrompt = "Click here to start!";

        [SerializeField]
        private string[] _winPrompts = new string[0];
        [SerializeField]
        private string[] _losePrompts = new string[0];

        public override void Open()
        {
            StartCoroutine(DelayAction(base.Open));
            DisplayDefaultPrompt();
        }

        public void DisplayDefaultPrompt()
        {
            _buttonPrompt.text = _defaultPrompt;
            _startButton.interactable = true;
        }

        public void DisplayWinPrompt()
        {
            _buttonPrompt.text = _winPrompts[UnityEngine.Random.Range(0, _winPrompts.Length)];
            _startButton.interactable = true;
        }

        public void DisplayLosePrompt()
        {
            _buttonPrompt.text = _winPrompts[UnityEngine.Random.Range(0, _winPrompts.Length)];
            _startButton.interactable = true;
        }


        private IEnumerator DelayAction(Action callback)
        {
            yield return new WaitForSeconds(_delay);
            callback.Invoke();
        }

        private void OnStartButtonClicked()
        {
            _startButton.interactable = false;
            _buttonPrompt.text = "";
            OnGameStarted?.Invoke();
        }

        protected override void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }
}
