using UnityEngine;

public enum ItemType {
    None,
    
    [Header("Cubes")]
    BlueCube,
    GreenCube,
    RedCube,
    YellowCube,

    [Header("Obstacles")]
    Box,
    Stone,
    Vase01,
    Vase02,

    [Header("Rockets")]
    HorizontalRocket,
    VerticalRocket
}