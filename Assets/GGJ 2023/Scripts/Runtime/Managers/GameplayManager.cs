using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField]
        private TileAdjacencyAgent _mlAgent = null;

        private void OnGameStarted()
        {
            _mlAgent.StartGrowing();
        }

        private void Start()
        {
            UIManager.GetView<TitleView>(GameViewId.Title, true).Open();
            UIManager.GetView<GameHUDView>(GameViewId.Game, true).OnGameStarted += OnGameStarted;
        }
    }
}
