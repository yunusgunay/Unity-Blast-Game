using UnityEngine;

// The ImageConverter class stores all the item images in the game (Singleton).
public class ImageConverter : Singleton<ImageConverter> {
    [Header("Cubes")]
    public Sprite BlueCube;
    public Sprite BlueCubeParticle;
    public Sprite GreenCube;
    public Sprite GreenCubeParticle;  
    public Sprite RedCube;
    public Sprite RedCubeParticle;
    public Sprite YellowCube;
    public Sprite YellowCubeParticle;


    [Header("Obstacles")]
    public Sprite Box;
    public Sprite Stone;
    public Sprite Vase01;
    public Sprite Vase02;

    [Header("Rockets")]
    public Sprite HorizontalRocket;
    public Sprite VerticalRocket;

    public Sprite getImage(ITEM_TYPE itemType) {
        switch(itemType) {
            // Cubes
            case ITEM_TYPE.BlueCube: return BlueCube;
            case ITEM_TYPE.GreenCube: return GreenCube;
            case ITEM_TYPE.RedCube: return RedCube;   
            case ITEM_TYPE.YellowCube: return YellowCube;
            
            // Obstacles
            case ITEM_TYPE.Box: return Box;
            case ITEM_TYPE.Stone: return Stone;
            case ITEM_TYPE.Vase01: return Vase01;
            case ITEM_TYPE.Vase02: return Vase02;
            
            // Rockets
            case ITEM_TYPE.HorizontalRocket: return HorizontalRocket;
            case ITEM_TYPE.VerticalRocket: return VerticalRocket;

            default: return null;
        }
    }
}
