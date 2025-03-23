// This class represents a single STONE in a Cell.
public class StoneItem : Item {
    public void PrepareStoneItem(ItemBase itemBase) {
        itemBase.IsFallable = false;
        itemBase.Health = 1;
        itemBase.InterectWithExplode = false;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));      
    }

    public override void TryExecute() {
        ParticleAnimation.Instance.PlayParticle(this);
        base.TryExecute();
    }
}
