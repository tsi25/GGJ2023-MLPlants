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

        private Dictionary<TileBase, SoilTileData> _dictionary = null;

        public SoilTileData GetDataByTile(TileBase tile)
        {
            if (tile == null) { Debug.LogWarning("Requested Data for null tile, returning null!"); return null; }
            if (_dictionary == null) { InitializeDictionary(); }

            if (_dictionary.ContainsKey(tile))
                return _dictionary[tile];

            Debug.LogWarning($"Dictionary on {name} could not find Tile, solving in non-performant manner!");

            return SoilFeatures.FirstOrDefault(sf => sf.Tile == tile);

        }

        public void InitializeDictionary()
        {
            if (_dictionary == null)
                _dictionary = new Dictionary<TileBase, SoilTileData>();
            else
                _dictionary.Clear();

            for (int i = 0; i < SoilFeatures.Length; i++)
            {
                _dictionary[SoilFeatures[i].Tile] = SoilFeatures[i];
            }
        }

#if UNITY_EDITOR
        [ContextMenu("IntializeDictionary")]
        public void EditorIntializeDictionary()
        {
            InitializeDictionary();
        }
#endif
    }
}
