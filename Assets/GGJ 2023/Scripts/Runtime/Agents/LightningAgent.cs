using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;

namespace GGJRuntime
{
    public class LightningAgent : Agent
    {
        public Action OnGrowthHalted = delegate { };

        [Header("Managers & Scriptables")]
        [SerializeField]
        protected TilemapManager _mapManager = null;
        [SerializeField]
        protected SoilFeatureCollection _collection = null;

        [Header("Agent Config")]
        [SerializeField]
        protected bool _generateMapOnEpisodeStart = true;
        [SerializeField]
        protected bool _growImmediately = false;
        [SerializeField]
        protected float _movementSpeed = 1f;
        [SerializeField]
        protected float _turnSpeed = 1f;

        [SerializeField]
        protected float _explorationCount = 25;
        [SerializeField]
        protected int _tileDecay = 100;
        [SerializeField]
        protected float _failurePenality = -10f;
        [SerializeField]
        protected float _maxGrowthDuration = 60f;

        [Header("Window to the Soul")]
        [SerializeField]
        protected float _agentXComponent = 0f;
        [SerializeField]
        protected float _agentYComponent = 0f;
        [SerializeField]
        protected float _simulatedXComponent = 0f;
        [SerializeField]
        protected float _simulatedYComponent = 0f;
        //[SerializeField]
        //protected float _agentTurnInput = 0f;
        [SerializeField]
        protected float _simulatedTurnInput = 0f;
        [SerializeField]
        protected bool _debug = false;
        [SerializeField]
        protected bool _training = false;

        // For pausing the root before user wants it to grow
        protected bool _isGrowing = false;
        protected float _growthDuration = 0f;
        // So we can guarantee we don't start in lava
        protected Vector3Int FirstTileCoord = new Vector3Int(7, 14, 0);

        protected Vector3 _cachedStartPosition = Vector3.zero;
        protected Vector3 _cachedStartRotation = Vector3.zero;

        protected Vector3Int _currentTilePosition = Vector3Int.zero;

        protected int _visitedTileCount = 0;
        
        protected int _tileAge = 1;

        protected HashSet<Vector3Int> _visitedTiles = new HashSet<Vector3Int>();

        public delegate void OnTileHit(Vector3Int tileCoord, LightningAgent agent);
        public event OnTileHit TileHitEvent;

        public bool IsGrowing
        {
            get => _isGrowing;
            set
            {
                _isGrowing = value;
                if(!_isGrowing)
                {
                    OnGrowthHalted?.Invoke();
                }
                if (_isGrowing)
                {
                    _growthDuration = 0f;
                }
            }
        }

        public Vector3 MovementVector
        {
            get { return (_movementSpeed * Time.deltaTime) * transform.forward; }
        }

        [ContextMenu("Start Growing")]
        public void StartGrowing()
        {
            IsGrowing = true;
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

            //Vector3 movementVector = MovementVector;
            sensor.AddObservation(_agentXComponent);
            sensor.AddObservation(_agentYComponent);
            //sensor.AddObservation(_agentTurnInput);

            Vector3Int currentPosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);

            List<Vector3Int> coordinates = new List<Vector3Int>();
            coordinates.Add(currentPosition); //knows about its own tile or not
            var neighboringCoordinates = _mapManager.GetNeighboringTileCoords(currentPosition, true);
            foreach (var neighboringCoordinate in neighboringCoordinates)
            {
                coordinates.Add(neighboringCoordinate.TileCoordinate);
            }

            foreach (Vector3Int coordinate in coordinates)
            {
                var data = _mapManager.GetDataByTileCoordinate(coordinate);

                //if the tile is off the map, observe that its penalized
                if (data == null)
                {
                    sensor.AddObservation(_collection.VeryBadModifier);
                    continue;
                }

                //if the tile has been visited, observe that its penalized
                if (_visitedTiles.Contains(coordinate))
                {
                    sensor.AddObservation(_collection.VeryBadModifier);
                    continue;
                }

                //otherwise observe the point value of the tile
                sensor.AddObservation(_collection.GetPointsFromData(data));
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            base.OnActionReceived(actions);

            if (!IsGrowing) return;

            //check if we need to turn
            _agentXComponent = actions.ContinuousActions[0];
            _agentYComponent = actions.ContinuousActions[1];
            Vector3Int currentTilePosition = _mapManager.GetTileCoordFromWorldCoord(transform.position);

            //if we have visited this tile before and we are currently on this tile
            if (_visitedTiles.Contains(currentTilePosition) && currentTilePosition == _currentTilePosition)
            {
                //if we arent training, check if we have stayed too long on the same tile and end the growth if so
                if(!_training)
                {
                    _tileAge++;
                    if (_tileAge == _tileDecay) IsGrowing = false;
                }
                return;
            }

            //just entered a new tile
            _tileAge = 0;
            _currentTilePosition = currentTilePosition;
            _visitedTileCount++;
            TileHitEvent?.Invoke(_currentTilePosition, this);

            //if we have done the allotted amount of exploring we can end the episode and check our score
            if (_visitedTileCount > _explorationCount)
            {
                //EndEpisode();
                return;
            }

            //if we have already visited this tile, penalize the agent and return
            if (_visitedTiles.Contains(_currentTilePosition))
            {
                if (_debug) Debug.Log("adding failure because the current tile has been visited already!");
                AddReward(_failurePenality);
                //EndEpisode();
                return;
            }

            //we are visiting a new tile, add it to the set of visited tiles
            _visitedTiles.Add(currentTilePosition);

            //if we are off the edge of the map, penalize the agent
            if (_mapManager.GetDataByWorldCoordinate(transform.position) == null)
            {
                if (_debug)
                {
                    Debug.Log("adding failure because we have fallen off the map!");
                }

                AddReward(_failurePenality);
                //EndEpisode();
            }
            //otherwise we are at a new tile and on the map, so add whatever that tile is worth
            else
            {
                float points = _collection.GetPointsFromData(_mapManager.GetDataByTileCoordinate(currentTilePosition));
                if (_debug) Debug.Log($"adding {points} points for hitting a valid new tile!");
                AddReward(points);

                if (points == _collection.VeryBadModifier)
                {
                    AddReward(_failurePenality);
                    //EndEpisode();
                }
                if (points == _collection.VeryGoodModifier)
                {
                    AddReward(100f);
                }
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;

            //if (Input.GetKey(KeyCode.LeftArrow)) _simulatedTurnInput -= 0.1f;
            //if (Input.GetKey(KeyCode.RightArrow)) _simulatedTurnInput += 0.1f;
            _simulatedTurnInput = Mathf.Clamp(_simulatedTurnInput, -1f, 1f);

            if (Input.GetKey(KeyCode.LeftArrow)) _simulatedXComponent -= 0.1f;
            if (Input.GetKey(KeyCode.RightArrow)) _simulatedXComponent += 0.1f;
            if (Input.GetKey(KeyCode.UpArrow)) _simulatedYComponent += 0.1f;
            if (Input.GetKey(KeyCode.DownArrow)) _simulatedYComponent -= 0.1f;
            _simulatedXComponent = Mathf.Clamp(_simulatedXComponent, -1f, 1f);
            _simulatedYComponent = Mathf.Clamp(_simulatedYComponent, -1f, 1f);
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();

            // TODO : This is a gross way to prevent a magma tile in the starting position, would be better to modify WFC
            if (_generateMapOnEpisodeStart)
            {
                bool happyMap = false;
                while (!happyMap)
                {
                    _mapManager.GenerateWFCMap();

                    happyMap = true;

                    foreach (var tileFeature in _mapManager.GetDataByTileCoordinate(FirstTileCoord).Features)
                    {
                        if (tileFeature.SoilType == SoilType.Magma)
                        {
                            happyMap = false;
                            if (_debug) { Debug.Log("Impossible map detected, re-rolling!"); }
                        }
                    }
                }
            }

            _visitedTiles.Clear();

            transform.position = _cachedStartPosition;
            transform.rotation = Quaternion.Euler(_cachedStartRotation);

            _agentXComponent = 0f;
            _agentYComponent = 0f;
            //_agentTurnInput = 0f;
            _simulatedTurnInput = 0f;

            _visitedTileCount = 0;
            _tileAge = 0;

            GetComponentInChildren<TrailRenderer>().Clear();

            IsGrowing = _growImmediately;
        }
        // =========== END REGION FIVE GOLDEN CALLBACKS =========== 

        protected virtual void Update()
        {
            if (!IsGrowing) { return; }
            //transform.Translate(MovementVector, Space.World);
            Vector3 agentInput = new Vector3(_agentXComponent, _agentYComponent, 0f);
            Vector3 simulatedInput = new Vector3(_simulatedXComponent, _simulatedYComponent, 0f);
            transform.position += _movementSpeed * Time.deltaTime * agentInput.normalized + _movementSpeed * Time.deltaTime * simulatedInput;
            //transform.Rotate(new Vector3(0f, Mathf.Clamp(_agentTurnInput + _simulatedTurnInput, -1f, 1f) * _turnSpeed * Time.deltaTime, 0f));

            _growthDuration += Time.deltaTime;
            if (_growthDuration >= _maxGrowthDuration)
            {
                IsGrowing = false;
            }
        }
    }
}
