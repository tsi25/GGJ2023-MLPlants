using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

namespace GGJRuntime
{
    public class TileAdjacencyAgent : Agent
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
        protected float _explorationCount = 25;
        [SerializeField]
        protected float _failurePenality = -10f;

        [SerializeField]
        protected float _agentTurnInput = 0f;
        [SerializeField]
        protected float _simulatedTurnInput = 0f;
        [SerializeField]
        protected bool _debug = false;

        protected Vector3 _cachedStartPosition = Vector3.zero;
        protected Vector3 _cachedStartRotation = Vector3.zero;

        protected Vector3Int _currentTilePosition = Vector3Int.zero;

        protected HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();

        public Vector3 MovementVector
        {
            get { return (_movementSpeed * Time.deltaTime) * transform.forward; }
        }

        // =========== REGION FIVE GOLDEN CALLBACKS =========== 
        public override void Initialize()
        {
            base.Initialize();

            _cachedStartPosition = transform.position;
            _cachedStartRotation = transform.rotation.eulerAngles;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);

            Vector3 movementVector = MovementVector;

            sensor.AddObservation(movementVector.x);
            sensor.AddObservation(movementVector.y);

            if(_debug)
            {
                Debug.Log($"movement x : {movementVector.x}");
                Debug.Log($"movement y : {movementVector.y}");
            }

            sensor.AddObservation(_agentTurnInput);

            Vector3Int currentPosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);

            var neighboringCoordinates = _mapManager.GetNeighboringTileCoords(currentPosition, true);

            foreach(var neighboringCoordinate in neighboringCoordinates)
            {
                var data = _mapManager.GetDataByTileCoordinate(neighboringCoordinate.TileCoordinate);

                if (data == null)
                {
                    sensor.AddObservation(_failurePenality);
                    if(_debug) Debug.Log($"{neighboringCoordinate.TileCoordinate} : {_failurePenality}");
                    continue;
                }

                sensor.AddObservation(_collection.GetPointsFromData(data));
                if (_debug) Debug.Log($"{neighboringCoordinate.TileCoordinate} : {_collection.GetPointsFromData(data)}");
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            base.OnActionReceived(actions);

            //check if we need to turn
            _agentTurnInput = actions.ContinuousActions[0];
            Vector3Int currentTilePosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);

            //if we have visited this tile before and we are currently on this tile, return
            if (_visitedTiles.Contains(currentTilePosition) && currentTilePosition != _currentTilePosition) return;

            _currentTilePosition = currentTilePosition;

            if (_visitedTiles.Count > _explorationCount)
            {
                EndEpisode();
                return;
            }

            //if we have already visited this tile, penalize the agent and return
            if (_visitedTiles.Contains(_currentTilePosition))
            {
                AddReward(_failurePenality);
                return;
            }

            //we are visiting a new tile, add it to the set of visited tiles
            _visitedTiles.Add(currentTilePosition);

            //if we are off the edge of the map, penalzie the agent
            if (_mapManager.GetDataByWorldCoordinate(transform.position) == null)
            {
                AddReward(_failurePenality);
            }
            //otherwise we are at a new tile and on the map, so add whatever that tile is worth
            else
            {
                float points = _collection.GetPointsFromData(_mapManager.GetDataByTileCoordinate(currentTilePosition));
                AddReward(points);
            }
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

            _mapManager.GenerateWFCMap();

            _visitedTiles.Clear();

            transform.position = _cachedStartPosition;
            transform.rotation = Quaternion.Euler(_cachedStartRotation);

            _agentTurnInput = 0f;
            _simulatedTurnInput = 0f;
            GetComponentInChildren<TrailRenderer>().Clear();
        }
        // =========== END REGION FIVE GOLDEN CALLBACKS =========== 

        protected virtual void Update()
        {
            transform.Translate(MovementVector, Space.World);
            transform.Rotate(new Vector3(0f, Mathf.Clamp(_agentTurnInput + _simulatedTurnInput, -1f, 1f) * _turnSpeed * Time.deltaTime, 0f));
        }
    }
}
