using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class AgentTileHitTracker : MonoBehaviour
    {
        [field: SerializeField, Header("Tracker Settings")]
        public TileAdjacencyAgent Agent { get; private set; } = null;
        [field: SerializeField]
        public TilemapManager MapManager { get; private set; } = null;

        [SerializeField, Header("Other")]
        private bool _debug = false;


        private void OnAgentEntersTile(Vector3Int tileCoord)
        {
            SoilTileData data = MapManager.GetDataByTileCoordinate(tileCoord);

            if (_debug)
            {
                Debug.Log($"{Agent.name} has just hit a {data?.Tile.name} tile!");
            }
        }

        private void Start()
        {
            Agent.TileHitEvent += OnAgentEntersTile;
        }

        private void OnDestroy()
        {
            Agent.TileHitEvent -= OnAgentEntersTile;
        }
    }
}
