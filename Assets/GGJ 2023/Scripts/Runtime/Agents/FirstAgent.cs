using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

namespace GGJRuntime
{
    public class FirstAgent : Agent
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

            BehaviorParameters parameters = GetComponent<BehaviorParameters>();
            if (parameters.BrainParameters.VectorObservationSize != _sensorPositions.Length)
            {
                Debug.LogWarning("Detected vector observation size does not match desired size! Adjusting....");
                parameters.BrainParameters.VectorObservationSize = _sensorPositions.Length;
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            foreach(Transform sensorPosition in _sensorPositions)
            {
                var data = _mapManager.GetDataByWorldCoordinate(sensorPosition.position);
                
                if (data == null)
                {
                    sensor.AddObservation(-1f);
                    continue;
                }

                Vector3Int worldCoordinate = _mapManager.GetTileCoordFromWorldCoord(sensorPosition.position);
                if (_visitedTiles.Contains(worldCoordinate))
                {
                    sensor.AddObservation(_revisitValue);
                    return;
                }

                sensor.AddObservation(_collection.GetPointsFromData(data));
            }
            
            base.CollectObservations(sensor);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            //check if we're dead because we have fallen off the map
            if (_mapManager.GetDataByWorldCoordinate(transform.position) == null)
            {
                SetReward(_currentScore - _failurePenality);
                EndEpisode();
            }

            //check if we're dead because we have crossed ourselves
            Vector3Int currentTilePosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);
            if (_visitedTiles.Contains(currentTilePosition))
            {
                if(currentTilePosition != _currentTilePosition)
                {
                    SetReward(_currentScore - _failurePenality);
                    EndEpisode();
                }

                return;
            }

            _visitedTiles.Add(currentTilePosition);
            _currentScore += _collection.GetPointsFromData(_mapManager.GetDataByTileCoordinate(currentTilePosition));
            _currentTilePosition = currentTilePosition;

            //check if the run is over because we've won
            if(_currentScore > _victoryThreshold)
            {
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
