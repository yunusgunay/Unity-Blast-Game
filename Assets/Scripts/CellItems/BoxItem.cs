
public class BoxItem: Item
{
    private const int HEALTH = 1;

    public void PrepareBoxItem(ItemBase itemBase)
    {
        itemBase.IsFallable = false;
        itemBase.Health = HEALTH;
        itemBase.InterectWithExplode = true;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));
    }

    public override void TryExecute()
    {
        ParticleManager.Instance.PlayParticle(this);
        base.TryExecute();
    }
}
