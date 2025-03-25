using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelData
{
    public ITEM_TYPE[,] GridData { get; protected set; }
    public List<LevelGoal> Goals { get; protected set; }
    public int Moves { get; protected set; }

    public LevelData(LevelInterface levelInfo)
    {
        // Count obstacles for goal data
        int numberOfBoxes = 0;
        int numberOfStones = 0;
        int numberOfVases = 0;

        // Set the grid data
        GridData = new ITEM_TYPE[levelInfo.grid_height, levelInfo.grid_width];

        int gridIndex = 0;
        for (int i = levelInfo.grid_height - 1; i >= 0; --i)
            for (int j = 0; j < levelInfo.grid_width; ++j)
            {
                switch (levelInfo.grid[gridIndex++])
                {
                    // Obstacles
                    case "bo": // Box
                        GridData[i, j] = ITEM_TYPE.Box;
                        ++numberOfBoxes;
                        break;
                    case "s": // Stone
                        GridData[i, j] = ITEM_TYPE.Stone;
                        ++numberOfStones;
                        break;
                    case "v": // Vase
                        GridData[i, j] = ITEM_TYPE.Vase02;
                        ++numberOfVases;
                        break;
                    
                    // Cubes
                    case "b": // Blue
                        GridData[i, j] = ITEM_TYPE.BlueCube;
                        break;
                    case "g": // Green
                        GridData[i, j] = ITEM_TYPE.GreenCube;
                        break;
                    case "r": // Red
                        GridData[i, j] = ITEM_TYPE.RedCube;
                        break;
                    case "y": // Yellow
                        GridData[i, j] = ITEM_TYPE.YellowCube;
                        break;
                    
                    // Random
                    case "rand": 
                        GridData[i, j] = ((ITEM_TYPE[]) Enum.GetValues(typeof(ITEM_TYPE)))[Random.Range(1, 5)];
                        break;

                    // Rockets
                    case "hro": // Horizontal Rocket
                        GridData[i, j] = ITEM_TYPE.HorizontalRocket;
                        break;
                    case "vro": // Vertical Rocket
                        GridData[i, j] = ITEM_TYPE.VerticalRocket;
                        break;
                    
                    default:
                        GridData[i, j] = ((ITEM_TYPE[])Enum.GetValues(typeof(ITEM_TYPE)))[Random.Range(1, 5)];
                        break;
                }
            }

        // Set the goals data
        Goals = new List<LevelGoal>();
        if (numberOfBoxes != 0) Goals.Add(new LevelGoal { ItemType = ITEM_TYPE.Box, Count = numberOfBoxes });
        if (numberOfStones != 0) Goals.Add(new LevelGoal { ItemType = ITEM_TYPE.Stone, Count = numberOfStones });
        if (numberOfVases != 0) Goals.Add(new LevelGoal { ItemType = ITEM_TYPE.Vase02, Count = numberOfVases });

        // Set moves
        Moves = levelInfo.move_count;
    }

    public static ITEM_TYPE GetRandomCubeItemType()
    {
        return ((ITEM_TYPE[])Enum.GetValues(typeof(ITEM_TYPE)))[Random.Range(1, 5)]; // 1,5 represents number of blocks
    }

}
