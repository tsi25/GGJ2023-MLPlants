using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJRuntime
{
    public class TerrainSelector : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _terrainDropdown = null;
        [SerializeField]
        private TileSetSwapper _tilemapSwapper = null;

        private const string GRID_TAG = "grid";

        private void OnTerrainSelectionChanged(int selection)
        {
            _tilemapSwapper.SwapSourceTilemap(selection);
        }

        private void Start()
        {
            _terrainDropdown.onValueChanged.AddListener(OnTerrainSelectionChanged);

            //TODO this is obviously gross af but I'm out of time so here goes nothing lol. Avert your eyes, Taylor
            _tilemapSwapper = GameObject.FindGameObjectWithTag(GRID_TAG).GetComponent<TileSetSwapper>();
        }
    }
}
