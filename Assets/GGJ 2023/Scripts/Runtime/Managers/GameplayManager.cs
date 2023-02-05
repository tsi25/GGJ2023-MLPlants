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
        private LightningAgent[] _mlAgents = new LightningAgent[0];
        [SerializeField]
        private TilemapManager _map = null;

        [SerializeField]
        private BaseSoilFeature[] _winConditions = new BaseSoilFeature[0];
        [SerializeField]
        private BaseSoilFeature[] _loseConditions = new BaseSoilFeature[0];

        private bool _isPlaying = false;

        private void AssertGameOver(SoilTileData data, LightningAgent agent)
        {
            if (!_isPlaying) return;

            //agent has gone off the map
            if(data == null)
            {
                agent.IsGrowing = false;
                return;
            }

            foreach(BaseSoilFeature feature in data.Features)
            {
                if(_winConditions.Contains(feature))
                {
                    //agent has found a winning tile
                    EndGame(win: true);
                    StopAgents();
                    return;
                }

                if (_loseConditions.Contains(feature))
                {
                    //agent has found a losing tile
                    agent.IsGrowing = false;
                    return;
                }
            }
        }

        private void AssertGameLost()
        {
            if (!_isPlaying) return;

            bool stillAlive = false;
            foreach(LightningAgent agent in _mlAgents)
            {
                if(agent.IsGrowing)
                {
                    stillAlive = true;
                    break;
                }
            }

            if(!stillAlive)
            {
                EndGame(win: false);
            }
        }

        private void StopAgents()
        {
            foreach (LightningAgent agent in _mlAgents)
            {
                if(agent.IsGrowing) agent.IsGrowing = false;
            }
        }

        private void EndGame(bool win)
        {
            _isPlaying = false;

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
            foreach (LightningAgent agent in _mlAgents)
            {
                agent.EndEpisode();
                agent.StartGrowing();
            }

            _isPlaying = true;
        }

        private void OnTileHit(Vector3Int coordinate, LightningAgent agent)
        {
            SoilTileData data = _map.GetDataByTileCoordinate(coordinate);
            AssertGameOver(data, agent);
        }

        private void Start()
        {
            foreach (LightningAgent agent in _mlAgents)
            {
                agent.TileHitEvent += OnTileHit;
                agent.OnGrowthHalted += AssertGameLost;
            }

            UIManager.GetView<TitleView>(GameViewId.Title, true).Open();
            UIManager.GetView<GameHUDView>(GameViewId.Game, true).OnGameStarted += OnGameStarted;
        }
    }
}
