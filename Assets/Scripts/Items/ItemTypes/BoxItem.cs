// This class represents a single BOX in a Cell.
public class BoxItem: Item {
    public void PrepareBoxItem(ItemBase itemBase) {  
        itemBase.IsFallable = false;
        itemBase.IsClickable = false;
        itemBase.CanExplode = true;        
        itemBase.Health = 1;
        
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.Type));
    }

    public override void TryExecute() {
        ParticleAnimation.Instance.PlayParticle(this);
        base.TryExecute();
    }

}
