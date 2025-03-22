
public class StoneItem : Item
{
    private const int HEALTH = 1;

    public void PrepareStoneItem(ItemBase itemBase)
    {
        itemBase.IsFallable = false;
        itemBase.Health = HEALTH;
        itemBase.InterectWithExplode = false;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));
       
    }

    public override void TryExecute()
    {
        ParticleManager.Instance.PlayParticle(this);
        base.TryExecute();
    }
}
