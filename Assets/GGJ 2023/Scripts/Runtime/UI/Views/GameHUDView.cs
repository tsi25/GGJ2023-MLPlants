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
        private SoilFeatureCollection _featureCollection = null;

        [SerializeField]
        private Button _startButton = null;
        [SerializeField]
        private TextMeshProUGUI _buttonPrompt = null;
        [SerializeField]
        private GameObject _startingExplanation = null;

        [SerializeField]
        private float _delay = 1f;

        [SerializeField]
        private ConditionalStatement[] _statements = new ConditionalStatement[0];

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
            _buttonPrompt.text = _losePrompts[UnityEngine.Random.Range(0, _losePrompts.Length)];
            _startButton.interactable = true;
        }

        private void InitializeTileRewards()
        {
            _featureCollection.ClearTileRewards();

            foreach (ConditionalStatement statement in _statements)
            {
                if (statement.SelectedSoilType != SoilType.None &&
                    statement.SelectedModifierType != ModifierType.None)
                {
                    switch (statement.SelectedModifierType)
                    {
                        case ModifierType.Bad:
                            _featureCollection.BadTileTypes.Add(statement.SelectedSoilType);
                            break;

                        case ModifierType.Good:
                            _featureCollection.GoodTileTypes.Add(statement.SelectedSoilType);
                            break;

                        case ModifierType.VeryBad:
                            _featureCollection.VeryBadTileTypes.Add(statement.SelectedSoilType);
                            break;

                        case ModifierType.VeryGood:
                            _featureCollection.VeryGoodTileTypes.Add(statement.SelectedSoilType);
                            break;
                    }
                }
            }
        }

        private IEnumerator DelayAction(Action callback)
        {
            yield return new WaitForSeconds(_delay);
            callback.Invoke();
        }

        private void OnStartButtonClicked()
        {
            InitializeTileRewards();
            _startButton.interactable = false;
            _buttonPrompt.text = "";
            _startingExplanation.SetActive(false);
            OnGameStarted?.Invoke();
        }

        protected override void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);

            GameplayManager.OnGameWin += DisplayWinPrompt;
            GameplayManager.OnGameLose += DisplayLosePrompt;
        }
    }
}
