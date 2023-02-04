using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

namespace GGJRuntime
{
    public class OmniscientAndConstrainedAgent : Agent
    {
        [Header("Managers & Scriptables")]
        [SerializeField]
        protected TilemapManager _mapManager = null;
        [SerializeField]
        protected SoilFeatureCollection _collection = null;

        [Header("Agent Config")]
        [SerializeField]
        protected float _movementSpeed = 1f;
        [SerializeField]
        protected float _turnSpeed = 1f;
        //[SerializeField]
        //protected float _revisitValue = -0.1f;
        //[SerializeField]
        //protected float _deathTileThreshold = -5f;
        [SerializeField]
        protected float _victoryThreshold = 10f;
        [SerializeField]
        protected float _failureThreshold = -10f;
        [SerializeField]
        protected float _explorationCount = 25;

        [SerializeField]
        protected float _agentTurnInput = 0f;
        [SerializeField]
        protected float _simulatedTurnInput = 0f;

        public bool _debug = false;

        [SerializeField]
        protected int _numSteps = 0;
        protected float _currentScore = 0f;
        protected Vector3 _cachedStartPosition = Vector3.zero;
        protected Vector3 _cachedStartRotation = Vector3.zero;
        protected Vector3 _currentPosition = Vector3.zero;
        protected Vector3 _currentTilePosition;
        protected HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();

        private Vector3 _movementVector = Vector3.zero;
        // map bounds to not go out of bounds and not worry about it
        private float _minXBound;
        private float _maxXBound;
        private float _minYBound;
        private float _maxYBound;

        // =========== REGION FIVE GOLDEN CALLBACKS =========== 
        public override void Initialize()
        {
            _cachedStartPosition = transform.position;
            _cachedStartRotation = transform.rotation.eulerAngles;
        }

        public override void OnEpisodeBegin()
        {
            _mapManager.GenerateBetterRandomMap();

            _minXBound = _mapManager.Map.cellBounds.xMin + 0.01f;
            _maxXBound = _mapManager.Map.cellBounds.xMax - 0.01f;
            _minYBound = _mapManager.Map.cellBounds.yMin + 0.01f;
            _maxYBound = _mapManager.Map.cellBounds.yMax - 0.01f;

            _visitedTiles.Clear();

            transform.position = _cachedStartPosition;
            transform.rotation = Quaternion.Euler(_cachedStartRotation);
            _currentPosition = _cachedStartPosition;

            _agentTurnInput = 0f;
            _simulatedTurnInput = 0f;

            _numSteps = 0;
            _currentScore = 0f;
            GetComponentInChildren<TrailRenderer>().Clear();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // Agent can know its movement vector
            sensor.AddObservation(_movementVector.x);
            sensor.AddObservation(_movementVector.y);

            float distance;
            float score;
            SoilTileData tileData;

            // Agent knows the score of the tile it is on and getting rewarded for
            tileData = _mapManager.GetDataByWorldCoordinate(transform.position);
            sensor.AddObservation(_collection.GetPointsFromData(tileData));

            // Agent knows distance to each of its neighbors
            List<TilemapManager.NeighborCoord> neighbors = _mapManager.GetNeighboringTileCoordsFromWorldCoord(transform.position, includeDiagonals: true);
            for (int i = 0; i < neighbors.Count; i++)
            {
                tileData = _mapManager.GetDataByTileCoordinate(neighbors[i].TileCoordinate);

                if (tileData == null) //neighbor is off the map
                    score = 0;// _collection.VeryBadModifier; //Trying 0 out so that it can distinguish between no-tile and some-tile
                else
                    score = _collection.GetPointsFromData(tileData);

                distance = Vector3.Distance(_mapManager.Map.CellToWorld(neighbors[i].TileCoordinate), transform.position);

                if (_debug)
                {
                    Debug.Log($"To the {neighbors[i].DirectionFromCaller.ToString()}, tile is WORTH: {score} and DISTANCE: {distance}");
                }
                sensor.AddObservation(score);
                sensor.AddObservation(distance);
            }
            
            #region TAYLOR_REF
            //sensor.AddObservation(MovementVector.x);
            //sensor.AddObservation(MovementVector.y);
            //sensor.AddObservation(MovementVector.z);

            //foreach (Transform sensorPosition in _sensorPositions)
            //{

            //    var data = _mapManager.GetDataByWorldCoordinate(sensorPosition.position);

            //    if (data == null)
            //    {
            //        sensor.AddObservation(_deathTileThreshold);
            //        continue;
            //    }

            //    Vector3Int worldCoordinate = _mapManager.GetTileCoordFromWorldCoord(sensorPosition.position);
            //    if (_visitedTiles.Contains(worldCoordinate))
            //    {
            //        sensor.AddObservation(_revisitValue);
            //        continue;
            //    }

            //    sensor.AddObservation(_collection.GetPointsFromData(data));
            //}

            //base.CollectObservations(sensor);
            #endregion
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //check if we need to turn
            _agentTurnInput = actions.ContinuousActions[0];

            // Handle moving into a new tile
            Vector3Int currentTilePosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);

            // You get the points for the tile your on for as long as you're on it
            float points = _collection.GetPointsFromData(_mapManager.GetDataByTileCoordinate(currentTilePosition));
            AddReward(points);
            _currentScore += points;

            if (currentTilePosition == _currentTilePosition) // haven't moved into new tile yet
                return;

            _currentTilePosition = currentTilePosition;

            // Stepping into a new tile this frame
            if (!_visitedTiles.Contains(currentTilePosition))
                _visitedTiles.Add(currentTilePosition);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;

            if (Input.GetKey(KeyCode.LeftArrow)) _simulatedTurnInput -= 0.1f;
            if (Input.GetKey(KeyCode.RightArrow)) _simulatedTurnInput += 0.1f;

            _simulatedTurnInput = Mathf.Clamp(_simulatedTurnInput, -1f, 1f);
        }

        // =========== END REGION FIVE GOLDEN CALLBACKS =========== 

        protected virtual void Update()
        {
            transform.Rotate(new Vector3(0f, Mathf.Clamp(_agentTurnInput + _simulatedTurnInput, -1f, 1f) * _turnSpeed * Time.deltaTime, 0f));

            // Movement clamped to the grid. Grid has to be rectangular
            Vector3 newLocalPos = transform.localPosition;
            newLocalPos += _movementSpeed * Time.deltaTime * transform.forward;
            newLocalPos.x = Mathf.Clamp(newLocalPos.x, _minXBound, _maxXBound);
            newLocalPos.y = Mathf.Clamp(newLocalPos.y, _minYBound, _maxYBound);
            _movementVector = newLocalPos - transform.localPosition;
            transform.localPosition = newLocalPos;
        }

        [ContextMenu("Clear Trail Renderer")]
        private void EditorClearTrailRenderer()
        {
            GetComponentInChildren<TrailRenderer>().Clear();
        }
    }
}
