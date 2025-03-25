using UnityEngine;

// This class represents a single CUBE in a Cell.
public class CubeItem : Item {
    private MATCH_TYPE matchType;
    
    public void PrepareCubeItem(ItemBase itemBase, MATCH_TYPE matchType) {
        this.matchType = matchType;
        itemBase.IsClickable = true;
        
        PrepareItem(itemBase, GetSpriteForColor());
    }

    private Sprite GetSpriteForColor() {
        ImageConverter imageConverter = ImageConverter.Instance;
        
        if (matchType == MATCH_TYPE.Blue) {
            return imageConverter.BlueCube;
        }

        if (matchType == MATCH_TYPE.Green) {
            return imageConverter.GreenCube;
        }

        if (matchType == MATCH_TYPE.Red) {
            return imageConverter.RedCube;
        }

        if (matchType == MATCH_TYPE.Yellow) {
            return imageConverter.YellowCube;
        }

        return null;
    }

    public override MATCH_TYPE GetMatchType() {
        return matchType;
    }
    
    public override void UpdateToHintSprite(ITEM_TYPE itemType) {
        ImageConverter imageConverter = ImageConverter.Instance;

        switch (itemType) {
            case ITEM_TYPE.HorizontalRocket:
                UpdateRocketSprite(imageConverter);
                break;
            case ITEM_TYPE.VerticalRocket:
                UpdateRocketSprite(imageConverter);
                break;
            default:
                UpdateSprite(GetSpriteForColor());
                break;
        }
    }

    private void UpdateRocketSprite(ImageConverter imageConverter) {
        Sprite newSprite;
        switch (matchType) {
            case MATCH_TYPE.Green:
                newSprite = imageConverter.GreenCubeParticle;
                break;
            case MATCH_TYPE.Yellow:
                newSprite = imageConverter.YellowCubeParticle;
                break;
            case MATCH_TYPE.Blue:
                newSprite = imageConverter.BlueCubeParticle;
                break;
            case MATCH_TYPE.Red:
                newSprite = imageConverter.RedCubeParticle;
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
