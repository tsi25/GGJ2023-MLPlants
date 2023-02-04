using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJRuntime
{
    public class WfcTilemapTest : MonoBehaviour
    {
        [Tooltip("Tilemap to sample from.")]
        public Tilemap inputTilemap = null;
        [Tooltip("Tilemap to output generated maps to.")]
        public Tilemap outputTilemap = null;
        [Tooltip("Logic asset driving map generation.")]
        public WaveFunctionCollapseTilemapLogic logic = null;

        private WaveFunctionCollapseTilemapLogic RuntimeLogic { get; set; }
        /*public int width = 5;
        public int height = 5;
        public int maxIterations = 500;
        public int patternSize = 2;
        public bool equalWeights = false;

        private WfcCore core;
        private ValuesManager<TileBase> valuesManager;
        private PatternManager manager;
        private TileMapOutput output;

        private void SolveGrid()
        {
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


        private void SaveTilemap()
        {
            if(output.OutputImage == null) return;


        }*/


        private void SolveGrid()
        {
            RuntimeLogic.GenerateGrid();
        }


        private void Start()
        {
            RuntimeLogic = logic.CreateRuntimeInstance(inputTilemap, outputTilemap);

            SolveGrid();
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SolveGrid();
            }
        }
    }
}