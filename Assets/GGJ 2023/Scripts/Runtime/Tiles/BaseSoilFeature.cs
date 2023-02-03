using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName =nameof(BaseSoilFeature), menuName = "GGJ/Data/"+nameof(BaseSoilFeature))]
    [System.Serializable]
    public class BaseSoilFeature : ScriptableObject
    {
        /// <summary>
        /// The type associated with this tile data
        /// </summary>
        [field: SerializeField, Tooltip("The type associated with this tile data")]
        public SoilType SoilType { get; private set; } = SoilType.None;
        /// <summary>
        /// The idea is that not every rocky tile is a dedicated rock tile. So a gravel-ish tile can have a Rock SoilFeature at 0.5 strength etc
        /// </summary>
        [field: SerializeField, Range(0f, 1f), Tooltip("The idea is that not every rocky tile is a dedicated rock tile. So a gravel-ish tile can have a Rock SoilFeature at 0.5 strength etc")]
        public float FeatureStrength { get; private set; } = 1f;
    }
}

