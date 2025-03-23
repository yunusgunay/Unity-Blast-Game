// This class represents a single BOX in a Cell.
public class BoxItem: Item {
    public void PrepareBoxItem(ItemBase itemBase) {
        itemBase.IsFallable = true;
        itemBase.Health = 1;
        itemBase.InterectWithExplode = true;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));
    }

    public override void TryExecute() {
        ParticleAnimation.Instance.PlayParticle(this);
        base.TryExecute();
    }
}
