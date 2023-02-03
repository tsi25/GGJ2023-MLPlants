using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSoilFeature
{
    /// <summary>
    /// The user will determine the "score" of this feature by dragging tiles. i.e. Water is Good will assign some UserScore to the SoilFeature 'Water'
    /// </summary>
    public static float UserScore = 0f;

    /// <summary>
    /// The idea is that not every rocky tile is a dedicated rock tile. So a gravel-ish tile can have a Rock SoilFeature at 0.5 strength etc
    /// </summary>
    [Range(0f, 1f)]
    public float FeatureStrength = 1f;
}
