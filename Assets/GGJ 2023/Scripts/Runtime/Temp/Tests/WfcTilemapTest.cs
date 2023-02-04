using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Text;

namespace GGJRuntime
{
    public class WfcTilemapTest : MonoBehaviour
    {
        public Tilemap inputTilemap = null;
        public Tilemap outputTilemap = null;
        public int width = 5;
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
            //ValuesManager<TileBase> valuesManager = new ValuesManager<TileBase>(grid);
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


        }


        private void Start()
        {
            SolveGrid();

            /*for(int row=0; row < grid.Length; row++)
            {
                for(int col=0; col < grid[0].Length; col++)
                {
                    Debug.Log($"Row: {row}\nCol: {col}\nTile: {grid[row][col].Value.name}");
                }
            }*/

            /*StringBuilder output = new StringBuilder();
            List<string> outputs = new List<string>();

            for(int row=-1; row <= grid.Length; row++)
            {
                output.Clear();

                for(int col = -1; col <= grid[0].Length; col++)
                {
                    output.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
                }

                outputs.Add(output.ToString());
            }

            outputs.Reverse();

            outputs.ForEach(s => Debug.Log(s));*/
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