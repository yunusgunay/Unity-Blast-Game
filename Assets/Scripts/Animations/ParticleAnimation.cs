using UnityEngine; 

// ParticleAnimation manages particle effects in the game.
public class ParticleAnimation : Singleton<ParticleAnimation> {
    [Header("CubeParticles")]
    public ParticleSystem CubeParticleBlue;
    public ParticleSystem CubeParticleRed;
    public ParticleSystem CubeParticleYellow;
    public ParticleSystem CubeParticleGreen;

    [Header("ObstacleParticles")]
    public ParticleSystem BoxParticle;
    public ParticleSystem StoneParticle;
    public ParticleSystem VaseParticle;

    public ParticleSystem ComboHintParticle;

    public void PlayParticle(Item item) {
        ParticleSystem particle;
        
        if (item.ItemType == ItemType.GreenCube) {
            particle = CubeParticleGreen;
        }

        else if (item.ItemType == ItemType.BlueCube) {
            particle = CubeParticleBlue;
        }
        
        else if (item.ItemType == ItemType.RedCube) {
            particle = CubeParticleRed;
        }
        
        else if (item.ItemType == ItemType.YellowCube) {
            particle = CubeParticleYellow;
        }

        else if (item.ItemType == ItemType.Box) {
            particle = BoxParticle;
        }

        else if (item.ItemType == ItemType.Stone) {
            particle = StoneParticle;
        }

        else {
            particle = VaseParticle;
        }

        Vector3 newton = item.transform.position + new Vector3(0, 0, -0.1f);
        var particleNew = Instantiate(particle, newton, Quaternion.identity, item.Cell.gameGrid.particlesParent);
        particleNew.Play();
    }

    public ParticleSystem PlayComboParticleOnItem(Item item) {
        var particle = Instantiate(ComboHintParticle, item.transform.position, Quaternion.identity, item.transform);
        particle.Play(true);
        return particle;
    }

    public void ParticleDestroyer(Item item) {
        if (item.Particle != null) {
            Destroy(item.Particle.gameObject);
        }
    }

}
