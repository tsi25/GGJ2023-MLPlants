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
        [field: SerializeField, Tooltip("Map managed by the tile manager"), Header("References")]
        public Tilemap Map { get; private set; } = null;
        /// <summary>
        /// Feature collection correlating particular tiles with their attributes
        /// </summary>
        [field: SerializeField, Tooltip("Feature collection correlating particular tiles with their attributes")]
        public SoilFeatureCollection FeatureCollection { get; private set; } = null;

        /// <summary>
        /// When TRUE, Diagonals are considered neighbors
        /// </summary>
        [field: SerializeField, Tooltip("When TRUE, Diagonals are considered neighbors"), Header("Settings")]
        public bool UseDiagonals { get; private set; } = true;

        [SerializeField, Header("Testing")]
        private Vector3 _testWorldCoord = Vector3.zero;
        [SerializeField]
        private Vector3Int _testCoordinate = Vector3Int.zero;

        private Dictionary<Vector3Int, SoilTileData> DataMap = new Dictionary<Vector3Int, SoilTileData>();

        #region HELPERS
        /// <summary>
        /// Takes in word position and returns the coordinate of the tile that contains that world position. Ignores Z-position
        /// </summary>
        public Vector3Int GetTileCoordFromWorldCoord(Vector2 worldPos)
        {
            int xCoord = (int)worldPos.x - (worldPos.x < 0 ? 1 : 0);
            int yCoord = (int)worldPos.y - (worldPos.y < 0 ? 1 : 0);
            return new Vector3Int(xCoord, yCoord, 0);
        }
        public Vector3Int GetTileCoordFromWorldCoord(Vector3 worldPos)
        {
            return GetTileCoordFromWorldCoord(new Vector2(worldPos.x, worldPos.y));
        }

        /// <summary>
        /// Given in a TileCoordinate, returns the neighboring TileCoordinates and their relative directions, diagonals are optional
        /// </summary>
        public List<NeighborCoord> GetNeighboringTileCoords(Vector3Int tileCoord, bool includeDiagonals = false)
        {
            List<NeighborCoord> neighborCoords = new List<NeighborCoord>();

            // North neighbor
            NeighborCoord curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.North);
            curNeighbor.TileCoordinate.y += 1;
            neighborCoords.Add(curNeighbor);

            // NorthEast neighbor
            if (includeDiagonals)
            {
                curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.NorthEast);
                curNeighbor.TileCoordinate.y += 1;
                curNeighbor.TileCoordinate.x += 1;
                neighborCoords.Add(curNeighbor);
            }

            // East neighbor
            curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.East);
            curNeighbor.TileCoordinate.x += 1;
            neighborCoords.Add(curNeighbor);

            // SouthEast neighbor
            if (includeDiagonals)
            {
                curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.SouthEast);
                curNeighbor.TileCoordinate.y -= 1;
                curNeighbor.TileCoordinate.x += 1;
                neighborCoords.Add(curNeighbor);
            }

            // South neighbor
            curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.South);
            curNeighbor.TileCoordinate.y -= 1;
            neighborCoords.Add(curNeighbor);

            // SouthWest neighbor
            if (includeDiagonals)
            {
                curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.SouthWest);
                curNeighbor.TileCoordinate.y -= 1;
                curNeighbor.TileCoordinate.x -= 1;
                neighborCoords.Add(curNeighbor);
            }

            // West neighbor
            curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.West);
            curNeighbor.TileCoordinate.x -= 1;
            neighborCoords.Add(curNeighbor);

            // NorthWest neighbor
            if (includeDiagonals)
            {
                curNeighbor = new NeighborCoord(tileCoord, NeighborDirections.NorthWest);
                curNeighbor.TileCoordinate.y += 1;
                curNeighbor.TileCoordinate.x -= 1;
                neighborCoords.Add(curNeighbor);
            }

            return neighborCoords;
        }
        public List<NeighborCoord> GetNeighboringTileCoordsFromWorldCoord(Vector3 worldPos, bool includeDiagonals = false)
        {
            return GetNeighboringTileCoords(GetTileCoordFromWorldCoord(worldPos), includeDiagonals);
        }
        #endregion



        public void CalculateMap()
        {
            DataMap.Clear();

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

        public void GenerateSuperRandomMap()
        {
            DataMap.Clear();

            BoundsInt mapBounds = Map.cellBounds;

            Debug.Log("Map Boudns : " + mapBounds);

            for (int x = mapBounds.xMin; x < mapBounds.xMax; x++)
            {
                for (int y = mapBounds.yMin; y < mapBounds.yMax; y++)
                {
                    Vector3Int coordinates = new Vector3Int(x, y, 0);

                    Map.SetTile(coordinates, FeatureCollection.SoilFeatures[Random.Range(0, FeatureCollection.SoilFeatures.Length)].Tile);

                    TileBase tile = Map.GetTile(coordinates);
                    SoilTileData data = FeatureCollection.GetDataByTile(tile);

                    DataMap.Add(new Vector3Int(x, y, 0), data);
                }
            }
        }

        public void GenerateBetterRandomMap()
        {
            DataMap.Clear();

            BoundsInt mapBounds = Map.cellBounds;

            for (int x = mapBounds.xMin; x < mapBounds.xMax; x++)
            {
                for (int y = mapBounds.yMin; y < mapBounds.yMax; y++)
                {
                    Vector3Int coordinates = new Vector3Int(x, y, 0);

                    Map.SetTile(coordinates, FeatureCollection.GetRandomBiasedFeature().Tile);

                    TileBase tile = Map.GetTile(coordinates);
                    SoilTileData data = FeatureCollection.GetDataByTile(tile);

                    DataMap.Add(new Vector3Int(x, y, 0), data);
                }
            }
        }
        
        public SoilTileData GetDataByTileCoordinate(Vector3Int coordinate)
        {
            if (!DataMap.ContainsKey(coordinate))
            {
                return null;
            }
            return DataMap[coordinate];
        }
        // Version of above function that takes world coord instead of tile coord
        public SoilTileData GetDataByWorldCoordinate(Vector3 worldCoordinate)
        {
            return GetDataByTileCoordinate(GetTileCoordFromWorldCoord(worldCoordinate));
        }

        private void Start()
        {
            CalculateMap();

        }

#if UNITY_EDITOR
         
        [ContextMenu("LogTileAtCoordinate")]
        public void LogTileAtCoordinate()
        {
            SoilTileData data = GetDataByTileCoordinate(_testCoordinate);
            Debug.Log(data?.name);
            if (data != null)
            {
                Debug.Log($"Tile point value : {FeatureCollection.GetPointsFromData(data)}");
            }
        }

        [ContextMenu("LogTileAtWorldCoordinate")]
        public void LogTileAtWorldCoordinate()
        {
            SoilTileData data = GetDataByWorldCoordinate(_testWorldCoord);
            string tileName = data?.name;
            string msg = $"{tileName?.Remove(tileName.Length-15)} is surrounded by: ";

            List<NeighborCoord> neighbors = GetNeighboringTileCoordsFromWorldCoord(_testWorldCoord, UseDiagonals);
            for (int i = 0; i < neighbors.Count; i++)
            {
                tileName = GetDataByTileCoordinate(neighbors[i].TileCoordinate)?.name;
                msg += $"{tileName?.Remove(tileName.Length-15)} on the {neighbors[i].DirectionFromCaller.ToString()}, ";
            }

            Debug.Log(msg);
            //Debug.Log(data?.Features.Length);
        }

        [ContextMenu("Generate Super Random Map")]
        public void EditorGenerateSuperRandomMap()
        {
            GenerateSuperRandomMap();
        }

        [ContextMenu("Generate Super Random Map")]
        public void EditorGenerateBetterRandomMap()
        {
            GenerateBetterRandomMap();
        }

        Vector3Int _testWorldCoord_TileCoord;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_testWorldCoord, 0.25f);
        }

#endif

        /// <summary>
        /// When we get the neighbors of a tile lets us keep the neighboring tile coords together with the neighboring coords direction
        /// </summary>
        public struct NeighborCoord
        {
            public Vector3Int TileCoordinate;
            public NeighborDirections DirectionFromCaller;

            public NeighborCoord(Vector3Int tileCoord, NeighborDirections neighborDirection)
            {
                this.TileCoordinate = tileCoord;
                this.DirectionFromCaller = neighborDirection;
            }
        }
    }
}
