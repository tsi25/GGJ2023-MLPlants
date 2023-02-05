using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    public class TileSetSwapper : MonoBehaviour
    {
        [SerializeField]
        private TilemapManager _mapManager = null;
        [SerializeField]
        private Tilemap[] _tilemaps = new Tilemap[0];

#if UNITY_EDITOR
        public int editorIndexOfTilemap = 0;

        [ContextMenu("ChangeToTilemapAtIndex")]
        public void EditorSwapTilemap()
        {
            SwapSourceTilemap(editorIndexOfTilemap);
        }
#endif

        public void SwapSourceTilemap(int indexOfTilemap)
        {
            _mapManager.SwapSourceTilemap(_tilemaps[indexOfTilemap]);
        }
    }
}
