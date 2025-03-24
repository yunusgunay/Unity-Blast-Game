using UnityEngine; 

// ParticleAnimation manages particle effects in the game.
public class ParticleAnimation : Singleton<ParticleAnimation> {
    [Header("Cube Particles")]
    public ParticleSystem CubeParticleBlue;
    public ParticleSystem CubeParticleRed;
    public ParticleSystem CubeParticleYellow;
    public ParticleSystem CubeParticleGreen;

    [Header("Box Particles")]
    public ParticleSystem BoxParticle1;
    public ParticleSystem BoxParticle2;
    public ParticleSystem BoxParticle3;

    [Header("Stone Particles")]
    public ParticleSystem StoneParticle1;
    public ParticleSystem StoneParticle2;
    public ParticleSystem StoneParticle3;
    
    [Header("Vase Particles")]
    public ParticleSystem VaseParticle1;
    public ParticleSystem VaseParticle2;
    public ParticleSystem VaseParticle3;

    public void PlayParticle(Item item) {
        ParticleSystem particle;
        Vector3 spawnPos = item.transform.position + new Vector3(0, 0, -0.1f);
        Transform parent = item.Cell.gameGrid.particlesParent;
        
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
            ParticleSystem p1 = Instantiate(BoxParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(BoxParticle2, spawnPos, Quaternion.identity, parent);
            ParticleSystem p3 = Instantiate(BoxParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            p3.Play();
            return; 
        }

        else if (item.ItemType == ItemType.Stone) {
            ParticleSystem p1 = Instantiate(StoneParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(StoneParticle2, spawnPos, Quaternion.identity, parent);
            ParticleSystem p3 = Instantiate(StoneParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            p3.Play();
            return;
        }

        else if (item.ItemType == ItemType.Vase01) {
            particle = VaseParticle2; // Vase01: Loses only its 1 component
        }

        else { // Vase02: Loses rest of the 2 components
            ParticleSystem p1 = Instantiate(VaseParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(VaseParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            return;
        }

        ParticleSystem particleNew = Instantiate(particle, spawnPos, Quaternion.identity, parent);
        particleNew.Play();
    }

}
