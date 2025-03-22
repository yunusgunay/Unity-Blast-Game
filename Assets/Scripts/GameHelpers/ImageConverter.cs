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

    public Sprite getImage(ItemType itemType) {
        switch(itemType) {
            // Cubes
            case ItemType.BlueCube: return BlueCube;
            case ItemType.GreenCube: return GreenCube;
            case ItemType.RedCube: return RedCube;   
            case ItemType.YellowCube: return YellowCube;
            
            // Obstacles
            case ItemType.Box: return Box;
            case ItemType.Stone: return Stone;
            case ItemType.Vase01: return Vase01;
            case ItemType.Vase02: return Vase02;
            
            // Rockets
            case ItemType.HorizontalRocket: return HorizontalRocket;
            case ItemType.VerticalRocket: return VerticalRocket;

            default: return null;
        }
    }
}
