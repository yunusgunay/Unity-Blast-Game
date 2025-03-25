using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelGridData {
    public ITEM_TYPE[,] GridData { get; protected set; }
    public List<GoalData> Goals { get; protected set; }
    public int Moves { get; protected set; }

    public LevelGridData(LevelInterface levelInfo) {
        // Counters for  obstacles used in goal data of a level.
        int numberOfBoxes = 0;
        int numberOfStones = 0;
        int numberOfVases = 0;

        // Initialize grid data array.
        GridData = new ITEM_TYPE[levelInfo.grid_height, levelInfo.grid_width];

        int gridIndex = 0;
        for (int i = levelInfo.grid_height - 1; i >= 0; --i) {
            for (int j = 0; j < levelInfo.grid_width; ++j) {
                string cell = levelInfo.grid[gridIndex++];
                GridData[i, j] = ParseGridCell(cell, ref numberOfBoxes, ref numberOfStones, ref numberOfVases);
            }
        }

        Goals = new List<GoalData>();
        if (numberOfBoxes > 0) {
            Goals.Add(new GoalData { GoalObstacle = ITEM_TYPE.Box, RequiredCount = numberOfBoxes });
        }
        if (numberOfStones > 0) {
            Goals.Add(new GoalData { GoalObstacle = ITEM_TYPE.Stone, RequiredCount = numberOfStones });
        }
        if (numberOfVases > 0) {
            Goals.Add(new GoalData { GoalObstacle = ITEM_TYPE.Vase02, RequiredCount = numberOfVases });
        }

        // Set moves.
        Moves = levelInfo.move_count;
    }

    private ITEM_TYPE ParseGridCell(string cell, ref int numberOfBoxes, ref int numberOfStones, ref int numberOfVases) {
        // Check for obstacles
        if (cell == "bo") {
            numberOfBoxes++;
            return ITEM_TYPE.Box;
        }
        if (cell == "s") {
            numberOfStones++;
            return ITEM_TYPE.Stone;
        }
        if (cell == "v") {
            numberOfVases++;
            return ITEM_TYPE.Vase02;
        }

        // Check for cubes
        if (cell == "b") {
            return ITEM_TYPE.BlueCube;
        }
        if (cell == "g") {
            return ITEM_TYPE.GreenCube;
        }
        if (cell == "r") {
            return ITEM_TYPE.RedCube;
        }
        if (cell == "y") {
            return ITEM_TYPE.YellowCube;
        }
        if (cell == "rand") {
            return GetRandomCubeItemType();
        }

        // Check for rockets
        if (cell == "hro") {
            return ITEM_TYPE.HorizontalRocket;
        }
        if (cell == "vro") {
            return ITEM_TYPE.VerticalRocket;
        }

        // Default
        return GetRandomCubeItemType();
    }

    public static ITEM_TYPE GetRandomCubeItemType() {
        return ((ITEM_TYPE[])Enum.GetValues(typeof(ITEM_TYPE)))[Random.Range(1, 5)];
    }

}
        