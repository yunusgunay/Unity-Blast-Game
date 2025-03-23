using UnityEngine;

/// <summary>
/// 
/// CubeItem is a class that represents a cube item in the game. It inherits from the Item class.
/// 
/// </summary>
public class CubeItem : Item
{
    private MatchType matchType;
    
    public void PrepareCubeItem(ItemBase itemBase,MatchType matchType)
    {
        this.matchType = matchType;
        itemBase.Clickable = true;
        Prepare(itemBase, GetSpritesForMatchType());
    }
    private Sprite GetSpritesForMatchType()
    {
        var imageLibrary = ImageConverter.Instance;
        switch(matchType)
        {
            case MatchType.Green:
                return imageLibrary.GreenCube;
            case MatchType.Yellow:
                return imageLibrary.YellowCube;
            case MatchType.Blue:
                return imageLibrary.BlueCube;
            case MatchType.Red:
                return imageLibrary.RedCube;
        }
        return null;
    }
    public override MatchType GetMatchType()
    {
        return matchType;
    }
    public override void HintUpdateToSprite(ItemType itemType)
    {
        var imageLibrary = ImageConverter.Instance;

        switch(itemType)
        {
            case ItemType.HorizontalRocket:
                UpdateColorfulBombSprite(imageLibrary);
                break;
            case ItemType.VerticalRocket:
                UpdateColorfulBombSprite(imageLibrary);
                break;
            default:
                UpdateSprite(GetSpritesForMatchType());
                break;
        }
    }
    private void UpdateColorfulBombSprite(ImageConverter imageLibrary)
    {
        Sprite newSprite;
        switch (matchType)
        {
            case MatchType.Green:
                newSprite = imageLibrary.GreenCubeParticle;
                break;
            case MatchType.Yellow:
                newSprite = imageLibrary.YellowCubeParticle;
                break;
            case MatchType.Blue:
                newSprite = imageLibrary.BlueCubeParticle;
                break;
            case MatchType.Red:
                newSprite = imageLibrary.RedCubeParticle;
                break;
            default:
                return;
        }
        UpdateSprite(newSprite);
    }
    
    public override void TryExecute()
    {
        ParticleManager.Instance.PlayParticle(this);
        base.TryExecute();
    }
}