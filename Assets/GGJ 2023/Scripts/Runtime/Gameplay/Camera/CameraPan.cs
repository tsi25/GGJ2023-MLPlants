using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class CameraPan : MonoBehaviour
    {
        [SerializeField]
        private Transform _menuPosition = null;
        [SerializeField]
        private Transform _gameplayPosition = null;
        [SerializeField]
        private Transform _cameraTransform = null;

        [SerializeField]
        private GameState _state = null;

        [SerializeField]
        private float _lerpDuration = 2f;
        [SerializeField]
        private AnimationCurve _lerpCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        private void PanToGameplay()
        {
            StopAllCoroutines();
            StartCoroutine(LerpCameraToTarget(_gameplayPosition));
        }

        private void PanToMenu()
        {
            StopAllCoroutines();
            StartCoroutine(LerpCameraToTarget(_menuPosition));
        }

        private IEnumerator LerpCameraToTarget(Transform target)
        {
            Vector3 startPosition = _cameraTransform.position;
            Vector3 targetPosition = target.position;

            float elapsedTime = 0f;
            while(elapsedTime < _lerpDuration)
            {
                _cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, _lerpCurve.Evaluate(elapsedTime / _lerpDuration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _cameraTransform.position = target.position;
        }

        private void OnGameStateChanged(StateType state)
        {
            switch(state)
            {
                case StateType.Gameplay:
                    PanToGameplay();
                    break;

                case StateType.Title:
                    PanToMenu();
                    break;
            }
        }

        private void Start()
        {
            _state.OnStateChanged += OnGameStateChanged;
        }
    }
}
