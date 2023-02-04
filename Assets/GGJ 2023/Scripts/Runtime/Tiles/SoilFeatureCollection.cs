using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    [CreateAssetMenu(fileName = nameof(SoilFeatureCollection), menuName =("GGJ/Collections/"+nameof(SoilFeatureCollection)))]
    public class SoilFeatureCollection : ScriptableObject
    {
        //TODO instead of set in the inspector like this, this needs to be user driven
        [field: SerializeField]
        public SoilType[] VeryGoodTileTypes { get; private set; } = new SoilType[0];
        [field: SerializeField]
        public SoilType[] GoodTileTypes { get; private set; } = new SoilType[0];

        [field: SerializeField]
        public SoilType[] BadTileTypes { get; private set; } = new SoilType[0];
        [field: SerializeField]
        public SoilType[] VeryBadTileTypes { get; private set; } = new SoilType[0];

        [field: SerializeField]
        public float VeryGoodModifier { get; private set; } = 1f;
        [field: SerializeField]
        public float GoodModifier { get; private set; } = 0.5f;

        [field: SerializeField]
        public float VeryBadModifier { get; private set; } = -1f;
        [field: SerializeField]
        public float BadModifier { get; private set; } = -0.5f;


        /// <summary>
        /// Static array of soil features used to track valid tiles
        /// </summary>
        [field: SerializeField, Tooltip("Static array of soil features used to track valid tiles")]
        public SoilTileData[] SoilFeatures { get; private set; } = new SoilTileData[0];

        /// <summary>
        /// Takes a tile and looks up the data associated with that tile
        /// </summary>
        /// <param name="tile">The tile to look up data for</param>
        /// <returns>The data associated with the given tile</returns>
        public SoilTileData GetDataByTile(TileBase tile)
        {
            // TODO prolly a cool way to optimize this with a dictionary but it involves some initialization i dont want to think about atm
            return SoilFeatures.FirstOrDefault(sf => sf.Tile == tile);
        }
        
        /// <summary>
        /// Takes a tile and looks up the point value associate with that tile based on current settings
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public float GetPointsFromTile(TileBase tile)
        {
            return GetPointsFromData(GetDataByTile(tile));
        }

        /// <summary>
        /// Takes a tile data and looks up the point value associate with that tile based on current settings
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public float GetPointsFromData(SoilTileData data)
        {
            float tileScore = 0f;

            //loop through each tile feature and update the tile score to reflect the given score of the tile
            //result must be a normalized value between -1.0 and 1.0
            
            for (int i = 0; i < data.Features.Length; i++)
            {
                if (VeryGoodTileTypes.Contains(data.Features[i].SoilType))
                {
                    tileScore += data.Features[i].FeatureStrength * VeryGoodModifier;
                    continue;
                }

                if (GoodTileTypes.Contains(data.Features[i].SoilType))
                {
                    tileScore += data.Features[i].FeatureStrength * GoodModifier;
                    continue;
                }

                if (BadTileTypes.Contains(data.Features[i].SoilType))
                {
                    tileScore += data.Features[i].FeatureStrength * BadModifier;
                    continue;
                }

                if (VeryBadTileTypes.Contains(data.Features[i].SoilType))
                {
                    tileScore += data.Features[i].FeatureStrength * VeryBadModifier;
                    continue;
                }
            }

            return tileScore;
        }

        public SoilTileData GetRandomBiasedFeature()
        {
            float maxValue = 0f;

            foreach(SoilTileData feature in SoilFeatures)
            {
                maxValue += feature.SelectionBias;
            }

            float selection = Random.Range(0f, maxValue);

            foreach (SoilTileData feature in SoilFeatures)
            {
                selection -= feature.SelectionBias;
                if (selection <= 0f) return feature;
            }

            return SoilFeatures[SoilFeatures.Length - 1];
        }
    }
}
