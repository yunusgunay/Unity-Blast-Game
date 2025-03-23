// This class represents a single VASE in a Cell.
public class VaseItem : Item {
    public void PrepareVaseItem(ItemBase itemBase) {
        itemBase.IsFallable = true;
        
        if(itemBase.ItemType == ItemType.Vase01) {
            itemBase.Health = 1;
        } else {
            itemBase.Health = 2; // ItemType.Vase02
        }

        itemBase.InterectWithExplode = true;
        itemBase.Clickable = false;
        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.ItemType));
    }

    public override void TryExecute() {
        Health = Health - 1;
        if (Health < 1) {
            ParticleAnimation.Instance.PlayParticle(this);
            base.TryExecute();
        } else {
            UpdateSprite(ImageConverter.Instance.Vase01); 
        }
    }
}
