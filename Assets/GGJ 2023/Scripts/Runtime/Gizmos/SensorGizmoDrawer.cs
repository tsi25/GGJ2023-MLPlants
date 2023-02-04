using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    public class SensorGizmoDrawer : MonoBehaviour
    {
        public bool _showGizmos = true;
        [SerializeField]
        private Transform AgentTF;
        [SerializeField]
        private Transform[] Sensors;

        private Color[] colors = new Color[] { Color.white, Color.red, Color.yellow, Color.green, Color.blue, Color.cyan, Color.magenta, Color.black, Color.gray };

        private void OnDrawGizmos()
        {
            if (!_showGizmos)
                return;
            if (AgentTF == null || Sensors.Length < 1)
                return;

            for (int i = 0; i < Sensors.Length; i++)
            {
                Gizmos.color = colors[0];
                Gizmos.DrawLine(AgentTF.position, Sensors[i].position);
            }
        }

        [ContextMenu("PullSensorsFrom \"Sensors\"")]
        public void EditorPullSensorsFromAgent()
        {
            Transform sensors = transform.Find("Sensors");
            if (sensors == null)
            {
                Debug.LogWarning("Could not find Sensors!");
                return;
            }

            List<Transform> children = new List<Transform>();
            for (int i = 0; i < sensors.childCount; i++)
            {
                children.Add(sensors.GetChild(i));
            }
            Sensors = children.ToArray();

            AgentTF = transform;
        }
    }
}