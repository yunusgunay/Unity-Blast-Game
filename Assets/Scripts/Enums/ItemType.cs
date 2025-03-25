using UnityEngine;

public enum ITEM_TYPE {
    None,
    
    [Header("Cube Types")]
    BlueCube,
    GreenCube,
    RedCube,
    YellowCube,

    [Header("Obstacle Types")]
    Box,
    Stone,
    Vase01,
    Vase02,

    [Header("Rocket Types")]
    HorizontalRocket,
    VerticalRocket
}
