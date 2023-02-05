using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    /// <summary>
    /// Logic driving WFC generation.
    /// </summary>
    [CreateAssetMenu(fileName="WFC Logic", menuName="GGJ/WFC/Logic")]
    public class WaveFunctionCollapseTilemapLogic : ScriptableObject
    {
        [Tooltip("Width of the output map.")]
        public int width = 5;
        [Tooltip("Height of the output map.")]
        public int height = 5;
        [Tooltip("Maximum number of times to iterate when collisions occur.")]
        public int maxIterations = 500;
        [Tooltip("Size of the sample pattern.\n\nNote: THIS CAN BE ONLY 1 for now. Highlander rules until I figure out some edge cases.")]
        public int patternSize = 1;
        [Tooltip("A little hazy, but I think if false this will try to reflect tile counts from the sample tilemap. So more frequent tiles will be more frequently output.")]
        public bool equalWeights = false;
        public bool debugWfcIteration = true;

        private Tilemap inputTilemap = null;
        private Tilemap outputTilemap = null;
        private WfcCore core;
        private ValuesManager<TileBase> valuesManager;
        private PatternManager manager;
        private TileMapOutput output;

        /// <summary>
        /// Create a runtime instance of the logic asset using current values and the passed tilemaps.
        /// </summary>
        /// <param name="inputTilemap">Tilemap to sample from.</param>
        /// <param name="outputTilemap">Tilemap to output generated maps to.</param>
        /// <returns>Instance of this logic initialized for runtime use.</returns>
        public WaveFunctionCollapseTilemapLogic CreateRuntimeInstance(Tilemap inputTilemap, Tilemap outputTilemap)
        {
            WaveFunctionCollapseTilemapLogic runtimeInstance = ScriptableObject.CreateInstance<WaveFunctionCollapseTilemapLogic>();

            string json = JsonUtility.ToJson(this);
            JsonUtility.FromJsonOverwrite(json, runtimeInstance);

            runtimeInstance.inputTilemap = inputTilemap;
            runtimeInstance.outputTilemap = outputTilemap;

            return runtimeInstance;
        }

        /// <summary>
        /// Generate a grid and ouput tiles to the associated output tilemap.
        /// </summary>
        public void GenerateGrid()
        {
            WfcCore.DebugIterations = debugWfcIteration;

            InputReader reader = new InputReader(inputTilemap);
            IValue<TileBase>[][] grid = reader.ReadInputToGrid();
            valuesManager = new ValuesManager<TileBase>(grid);
            manager = new PatternManager(patternSize);

            manager.ProcessGrid(valuesManager, equalWeights);

            core = new WfcCore(width, height, maxIterations, manager);

            CreateTilemap();
        }


        private void CreateTilemap()
        {
            output = new TileMapOutput(valuesManager, outputTilemap);

            int[][] result = core.CreateOutputGrid();

            output.CreateOutput(manager, result, width, height);
        }
    }
}