// This class represents a single STONE in a Cell.
public class StoneItem : Item {
    public void PrepareStoneItem(ItemBase itemBase) {
        itemBase.IsFallable = false;
        itemBase.IsClickable = false;
        itemBase.CanExplode = false;
        itemBase.Health = 1;
        
        PrepareItem(itemBase, ImageConverter.Instance.getImage(itemBase.Type));      
    }

    public override void TryExecute() {
        ParticleAnimation.Instance.PlayParticle(this);
        base.TryExecute();
    }
}
