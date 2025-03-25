using UnityEngine;

// This class represents a single CUBE in a Cell.
public class CubeItem : Item {
    private MATCH_TYPE matchType;
    
    public void PrepareCubeItem(ItemBase itemBase, MATCH_TYPE matchType) {
        this.matchType = matchType;
        itemBase.IsClickable = true;
        
        Prepare(itemBase, GetSpriteForColor());
    }

    private Sprite GetSpriteForColor() {
        ImageConverter imageLibrary = ImageConverter.Instance;
        
        if (matchType == MATCH_TYPE.Blue) {
            return imageLibrary.BlueCube;
        }

        if (matchType == MATCH_TYPE.Green) {
            return imageLibrary.GreenCube;
        }

        if (matchType == MATCH_TYPE.Red) {
            return imageLibrary.RedCube;
        }

        if (matchType == MATCH_TYPE.Yellow) {
            return imageLibrary.YellowCube;
        }

        return null;
    }

    public override MATCH_TYPE GetMatchType() {
        return matchType;
    }
    
    public override void UpdateToHintSprite(ITEM_TYPE itemType) {
        ImageConverter imageLibrary = ImageConverter.Instance;

        switch (itemType) {
            case ITEM_TYPE.HorizontalRocket:
                UpdateRocketSprite(imageLibrary);
                break;
            case ITEM_TYPE.VerticalRocket:
                UpdateRocketSprite(imageLibrary);
                break;
            default:
                UpdateSprite(GetSpriteForColor());
                break;
        }
    }

    private void UpdateRocketSprite(ImageConverter imageLibrary) {
        Sprite newSprite;
        switch (matchType) {
            case MATCH_TYPE.Green:
                newSprite = imageLibrary.GreenCubeParticle;
                break;
            case MATCH_TYPE.Yellow:
                newSprite = imageLibrary.YellowCubeParticle;
                break;
            case MATCH_TYPE.Blue:
                newSprite = imageLibrary.BlueCubeParticle;
                break;
            case MATCH_TYPE.Red:
                newSprite = imageLibrary.RedCubeParticle;
                break;
            default:
                return;
        }
    
        UpdateSprite(newSprite);
    }
    
    public override void TryExecute() {
        ParticleAnimation.Instance.PlayParticle(this);
        base.TryExecute();
    }

}
