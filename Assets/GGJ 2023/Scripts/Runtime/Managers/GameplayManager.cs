using System;
using System.Linq;
using UnityEngine;

namespace GGJRuntime
{
    public class GameplayManager : MonoBehaviour
    {
        public static Action OnGameWin = delegate { };
        public static Action OnGameLose = delegate { };

        [SerializeField]
        private TileAdjacencyAgent _mlAgent = null;
        [SerializeField]
        private TilemapManager _map = null;

        [SerializeField]
        private BaseSoilFeature[] _winConditions = new BaseSoilFeature[0];
        [SerializeField]
        private BaseSoilFeature[] _loseConditions = new BaseSoilFeature[0];

        private bool _isPlaying = false;

        private void AssertGameOver(SoilTileData data)
        {
            if (!_isPlaying) return;

            //agent has gone off the map
            if(data == null)
            {
                EndGame(win: false);
                return;
            }

            foreach(BaseSoilFeature feature in data.Features)
            {
                if(_winConditions.Contains(feature))
                {
                    //agent has found a winning tile
                    EndGame(win: true);
                    return;
                }

                if (_loseConditions.Contains(feature))
                {
                    //agent has found a losing tile
                    EndGame(win: false);
                    return;
                }
            }
        }

        private void EndGame(bool win)
        {
            _isPlaying = false;
            _mlAgent.EndEpisode();

            if (win)
            {
                OnGameWin?.Invoke();
            }
            else
            {
                OnGameLose?.Invoke();
            }
        }

        private void OnGameStarted()
        {
            _mlAgent.StartGrowing();
            _isPlaying = true;
        }

        private void OnTileHit(Vector3Int coordinate)
        {
            SoilTileData data = _map.GetDataByTileCoordinate(coordinate);
            AssertGameOver(data);
        }

        private void Start()
        {
            _mlAgent.TileHitEvent += OnTileHit;
            UIManager.GetView<TitleView>(GameViewId.Title, true).Open();
            UIManager.GetView<GameHUDView>(GameViewId.Game, true).OnGameStarted += OnGameStarted;
        }
    }
}
