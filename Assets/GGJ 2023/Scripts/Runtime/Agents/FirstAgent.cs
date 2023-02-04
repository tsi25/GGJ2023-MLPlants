using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace GGJRuntime
{
    public class FirstAgent : Agent
    {
        // =========== REGION FIVE GOLDEN CALLBACKS =========== 
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            base.CollectObservations(sensor);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            base.OnActionReceived(actions);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();
        }
        // =========== END REGION FIVE GOLDEN CALLBACKS =========== 


    }
}
