using UnityEngine; 

// ParticleAnimation manages particle effects in the game.
public class ParticleAnimation : MonoBehaviour {
    public static ParticleAnimation Instance { get; private set; }

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
    public ParticleSystem VaseParticle1; // for Vase02
    public ParticleSystem VaseParticle2; // for Vase01
    public ParticleSystem VaseParticle3; // for Vase02

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayParticle(Item item) {
        ParticleSystem particle;
        Vector3 spawnPos = item.transform.position + new Vector3(0, 0, -0.1f);
        Transform parent = item.Cell.parentBoard.particlesParent;
        
        // Blue Cube Particles
        if (item.ItemType == ITEM_TYPE.BlueCube) {
            particle = Instantiate(CubeParticleBlue, spawnPos, Quaternion.identity, parent);
            particle.Play();
            return;
        }       
       
        // Red Cube Particles
        if (item.ItemType == ITEM_TYPE.RedCube) {
            particle = Instantiate(CubeParticleRed, spawnPos, Quaternion.identity, parent);
            particle.Play();
            return;
        }
        
        // Yellow Cube Particles
        if (item.ItemType == ITEM_TYPE.YellowCube) {
            particle = Instantiate(CubeParticleYellow, spawnPos, Quaternion.identity, parent);
            particle.Play();
            return;
        }

        // Green Cube Particles
        if (item.ItemType == ITEM_TYPE.GreenCube) {
            particle = Instantiate(CubeParticleGreen, spawnPos, Quaternion.identity, parent);
            particle.Play();
            return;
        }

        // Box Obstacle Particles
        if (item.ItemType == ITEM_TYPE.Box) {           
            ParticleSystem p1 = Instantiate(BoxParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(BoxParticle2, spawnPos, Quaternion.identity, parent);
            ParticleSystem p3 = Instantiate(BoxParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            p3.Play();
            return; 
        }

        // Stone Obstacle Particles
        if (item.ItemType == ITEM_TYPE.Stone) {
            ParticleSystem p1 = Instantiate(StoneParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(StoneParticle2, spawnPos, Quaternion.identity, parent);
            ParticleSystem p3 = Instantiate(StoneParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            p3.Play();
            return;
        }

        // Vase01 Obstacle Particles
        if (item.ItemType == ITEM_TYPE.Vase01) {
            particle = Instantiate(VaseParticle2, spawnPos, Quaternion.identity, parent);
            particle.Play();
            return;
        }

        // Vase02 Obstacle Particles
        if (item.ItemType == ITEM_TYPE.Vase02) {
            ParticleSystem p1 = Instantiate(VaseParticle1, spawnPos, Quaternion.identity, parent);
            ParticleSystem p2 = Instantiate(VaseParticle3, spawnPos, Quaternion.identity, parent);
            p1.Play();
            p2.Play();
            return;
        }
    }

}
