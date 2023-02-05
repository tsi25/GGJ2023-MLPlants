using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class AgentTileHitTracker : MonoBehaviour
    {
        [field: SerializeField, Header("Tracker Settings")]
        public LightningAgent[] Agents { get; private set; } = new LightningAgent[0];
        [field: SerializeField]
        public TilemapManager MapManager { get; private set; } = null;

        [SerializeField, Header("Other")]
        private bool _debug = false;


        private void OnAgentEntersTile(Vector3Int tileCoord, LightningAgent agent)
        {
            SoilTileData data = MapManager.GetDataByTileCoordinate(tileCoord);

            if (_debug)
            {
                Debug.Log($"{agent.name} has just hit a {data?.Tile.name} tile!");
            }
        }

        private void Start()
        {
            foreach (LightningAgent agent in Agents)
            {
                agent.TileHitEvent += OnAgentEntersTile;
            }
        }

        private void OnDestroy()
        {
            foreach(LightningAgent agent in Agents)
            {
                agent.TileHitEvent -= OnAgentEntersTile;
            }
        }
    }
}
