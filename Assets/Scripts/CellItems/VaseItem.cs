
public class VaseItem : Item
{
    public void PrepareVaseItem(ItemBase itemBase)
    {
        itemBase.IsFallable = true;
        itemBase.Health = GetVaseHealth(itemBase.ItemType);
        itemBase.InterectWithExplode = true;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));
    }

    private int GetVaseHealth(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Vase01: return 1;
            case ItemType.Vase02: return 2;
            default: return 0;
        }
    }

    public override void TryExecute()
    {
        Health--;
        if (Health < 1)
        {
            ParticleManager.Instance.PlayParticle(this);
            base.TryExecute();
        }
        else
        {
            UpdateSprite(ImageConverter.Instance.Vase01); 
        }
    }
}
