using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    public class TilemapManager : MonoBehaviour
    {
        /// <summary>
        /// Map managed by the tile manager
        /// </summary>
        [field: SerializeField, Tooltip("Map managed by the tile manager")]
        public Tilemap Map { get; private set; } = null;
        /// <summary>
        /// Feature collection correlating particular tiles with their attributes
        /// </summary>
        [field: SerializeField, Tooltip("Feature collection correlating particular tiles with their attributes")]
        public SoilFeatureCollection FeatureCollection { get; private set; } = null;

        [SerializeField]
        private Vector3Int _testCoordinate = Vector3Int.zero;

        private Dictionary<Vector3Int, SoilTileData> DataMap = new Dictionary<Vector3Int, SoilTileData>();

        public void CalculateMap()
        {
            BoundsInt mapBounds = Map.cellBounds;

            Debug.Log("Map Boudns : " + mapBounds);

            for (int x = mapBounds.xMin; x < mapBounds.xMax; x++)
            {
                for (int y = mapBounds.yMin; y < mapBounds.yMax; y++)
                {
                    Vector3Int coordinates = new Vector3Int(x, y, 0);
                    TileBase tile = Map.GetTile(coordinates);
                    SoilTileData data = FeatureCollection.GetDataByTile(tile);

                    DataMap.Add(new Vector3Int(x, y, 0), data);
                }
            }
        }

        public SoilTileData GetDataByCoordinate(Vector3Int coordinate)
        {
            if (!DataMap.ContainsKey(coordinate))
            {
                Debug.LogWarning($"No valid tile found at coordinate {coordinate}");
                return null;
            }
            return DataMap[coordinate];
        }

        private void Start()
        {
            CalculateMap();
        }

#if UNITY_EDITOR
         
        [ContextMenu("LogTileAtCoordinate")]
        public void LogTileAtCoordinate()
        {
            SoilTileData data = GetDataByCoordinate(_testCoordinate);
            Debug.Log(data.name);
            Debug.Log(data.Features.Length);
        }

#endif
    }
}
