// This class represents a single VASE in a Cell.
public class VaseItem : Item {
    public void PrepareVaseItem(ItemBase itemBase) {
        itemBase.IsFallable = true;
        itemBase.IsClickable = false;
        itemBase.CanExplode = true;

        if(itemBase.Type == ITEM_TYPE.Vase01) {
            itemBase.Health = 1;
        } else {
            itemBase.Health = 2; // ItemType.Vase02
        }

        Prepare(itemBase, ImageConverter.Instance.getImage(itemBase.Type));
    }

    public override void TryExecute() {
        Health = Health - 1;
        if (Health < 1) {
            ParticleAnimation.Instance.PlayParticle(this);
            base.TryExecute();
        } else {
            ParticleAnimation.Instance.PlayParticle(this);
            UpdateSprite(ImageConverter.Instance.Vase01); 
        }
    }

}
