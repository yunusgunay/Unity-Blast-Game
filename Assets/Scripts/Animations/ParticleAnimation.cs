using UnityEngine; 

// ParticleAnimation manages particle effects in the game.
public class ParticleAnimation : Singleton<ParticleAnimation> {
    [Header("CubeParticles")]
    public ParticleSystem CubeParticleBlue;
    public ParticleSystem CubeParticleRed;
    public ParticleSystem CubeParticleYellow;
    public ParticleSystem CubeParticleGreen;

    [Header("ObstacleParticles")]
    public ParticleSystem BoxParticle1;
    public ParticleSystem BoxParticle2;
    public ParticleSystem BoxParticle3;
    public ParticleSystem StoneParticle;
    public ParticleSystem VaseParticle;

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
            Vector3 spawnPos = item.transform.position + new Vector3(0, 0, -0.1f);
            Transform parent = item.Cell.gameGrid.particlesParent;
            ParticleSystem p1 = Instantiate(BoxParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(BoxParticle2, spawnPos, Quaternion.identity, parent);
            ParticleSystem p3 = Instantiate(BoxParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            p3.Play();
            return; 
        }

        else if (item.ItemType == ItemType.Stone) {
            particle = StoneParticle;
        }

        else {
            particle = VaseParticle;
        }

        Vector3 newton = item.transform.position + new Vector3(0, 0, -0.1f);
        ParticleSystem particleNew = Instantiate(particle, newton, Quaternion.identity, item.Cell.gameGrid.particlesParent);
        particleNew.Play();
    }

}
