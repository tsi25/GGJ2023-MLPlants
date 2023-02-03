using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName = nameof(SoilTileData), menuName ="GGJ/Data/"+nameof(SoilTileData))]
    [System.Serializable]
    public class SoilTileData : ScriptableObject
    {
        /// <summary>
        /// Tile associated with its correlated features
        /// </summary>
        [field: SerializeField, Tooltip("Tile associated with its correlated features")]
        public TileBase Tile { get; private set; } = null;
        /// <summary>
        /// Each feature associated with a tile
        /// </summary>
        [field: SerializeField, Tooltip("Each feature associated with a tile")]
        public BaseSoilFeature[] Features { get; private set; } = null;
    }
}
