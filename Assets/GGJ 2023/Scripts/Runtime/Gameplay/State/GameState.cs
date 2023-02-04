using System;
using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName =nameof(GameState), menuName ="GGJ/Data/"+nameof(GameState))]
    public class GameState : ScriptableObject
    {
        public Action<StateType> OnStateChanged = delegate { };

        [SerializeField]
        private StateType _stateType = StateType.None;

        public StateType StateType 
        {
            get => _stateType;
            set
            {
                if(_stateType != value)
                {
                    _stateType = value;
                    OnStateChanged?.Invoke(_stateType);
                }
            }
        }
    }
}
