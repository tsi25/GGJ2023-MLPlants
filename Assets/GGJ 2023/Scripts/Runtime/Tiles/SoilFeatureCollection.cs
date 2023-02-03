using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName = nameof(SoilFeatureCollection), menuName =("GGJ/Collections/"+nameof(SoilFeatureCollection)))]
    public class SoilFeatureCollection : ScriptableObject
    {
        /// <summary>
        /// Static array of soil features used to track valid tiles
        /// </summary>
        [field: SerializeField, Tooltip("Static array of soil features used to track valid tiles")]
        public SoilTileData[] SoilFeatures { get; private set; } = new SoilTileData[0];

        public SoilTileData GetDataByTile(TileBase tile)
        {
            // TODO prolly a cool way to optimize this with a dictionary but it involves some initialization i dont want to think about atm
            return SoilFeatures.FirstOrDefault(sf => sf.Tile == tile);
        }
    }
}
