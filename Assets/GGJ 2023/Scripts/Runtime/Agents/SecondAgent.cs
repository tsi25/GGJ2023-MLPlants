using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

namespace GGJRuntime
{
    public class SecondAgent : Agent
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
        [SerializeField]
        protected float _revisitValue = -0.1f;
        [SerializeField]
        protected float _victoryThreshold = 10f;
        [SerializeField]
        protected float _failurePenality = 100f;
        public bool _debug = false;

        //TODO this casn be more performant by deriving it mathematically
        [SerializeField]
        protected Transform[] _sensorPositions = new Transform[0];
        [SerializeField]
        protected float _agentTurnInput = 0f;
        [SerializeField]
        protected float _simulatedTurnInput = 0f;

        protected float _currentScore = 0f;
        protected Vector3 _cachedStartPosition = Vector3.zero;
        protected Vector3 _cachedStartRotation = Vector3.zero;
        protected Vector3Int _currentTilePosition = Vector3Int.zero;
        protected HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();


        // =========== REGION FIVE GOLDEN CALLBACKS =========== 
        public override void Initialize()
        {
            base.Initialize();

            _cachedStartPosition = transform.position;
            _cachedStartRotation = transform.rotation.eulerAngles;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // Knowledge of own facing direction
            sensor.AddObservation(transform.rotation);

            // tile we're on and neighboring tiles
            var data = _mapManager.GetDataByWorldCoordinate(transform.position);
            if (data == null)
                sensor.AddObservation(-1f);
            else
                sensor.AddObservation(_collection.GetPointsFromData(data));

            foreach (var neighbor in _mapManager.GetNeighboringTileCoordsFromWorldCoord(transform.position, includeDiagonals: true))
            {
                data = _mapManager.GetDataByTileCoordinate(neighbor.TileCoordinate);
                if (data == null)
                    sensor.AddObservation(-1f);
                else
                    sensor.AddObservation(_collection.GetPointsFromData(data));
            }

            // Knowledge of depth of tiles in each direction
            //foreach (int tileDepth in _mapManager.GetRemainingDepthInEachAxis(transform.position))
            //{
            //    sensor.AddObservation(tileDepth);
            //}

            // Fan of sensors determining tiles in front of agent
            //foreach (Transform sensorPosition in _sensorPositions)
            //{
            //    var data = _mapManager.GetDataByWorldCoordinate(sensorPosition.position);

            //    if (data == null)
            //    {
            //        sensor.AddObservation(-1f);
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
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //check if we're dead because we have fallen off the map
            if (_mapManager.GetDataByWorldCoordinate(transform.position) == null)
            {
                if (_debug) { Debug.Log("Ran off map!"); }
                SetReward(_currentScore - _failurePenality);
                EndEpisode();
            }

            //check if we're dead because we have crossed ourselves
            Vector3Int currentTilePosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);
            if (_visitedTiles.Contains(currentTilePosition))
            {
                if (currentTilePosition != _currentTilePosition)
                {
                    if (_debug) { Debug.Log("Crossed roots!"); }
                    SetReward(_currentScore - _failurePenality);
                    EndEpisode();
                }

                return;
            }

            _visitedTiles.Add(currentTilePosition);

            float tileScore = _collection.GetPointsFromData(_mapManager.GetDataByTileCoordinate(currentTilePosition));
            _currentScore += tileScore;
            _currentTilePosition = currentTilePosition;

            //check if the run is over because we've won
            if (_currentScore > _victoryThreshold)
            {
                if (_debug) { Debug.Log("Victory!");}
                SetReward(1f);
                EndEpisode();
            }

            //check if we need to turn
            _agentTurnInput = actions.ContinuousActions[0];

            base.OnActionReceived(actions);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;

            if (Input.GetKey(KeyCode.LeftArrow)) _simulatedTurnInput -= 0.1f;
            if (Input.GetKey(KeyCode.RightArrow)) _simulatedTurnInput += 0.1f;

            _simulatedTurnInput = Mathf.Clamp(_simulatedTurnInput, -1f, 1f);
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();

            _mapManager.GenerateBetterRandomMap();

            _visitedTiles.Clear();

            transform.position = _cachedStartPosition;
            transform.rotation = Quaternion.Euler(_cachedStartRotation);

            _agentTurnInput = 0f;
            _simulatedTurnInput = 0f;

            _currentScore = 0f;
            GetComponentInChildren<TrailRenderer>().Clear();
        }
        // =========== END REGION FIVE GOLDEN CALLBACKS =========== 

        protected virtual void Update()
        {
            transform.Translate((_movementSpeed * Time.deltaTime) * transform.forward, Space.World);
            transform.Rotate(new Vector3(0f, Mathf.Clamp(_agentTurnInput + _simulatedTurnInput, -1f, 1f) * _turnSpeed * Time.deltaTime, 0f));
        }

        [ContextMenu("Clear Trail Renderer")]
        private void EditorClearTrailRenderer()
        {
            GetComponentInChildren<TrailRenderer>().Clear();
        }
    }
}
