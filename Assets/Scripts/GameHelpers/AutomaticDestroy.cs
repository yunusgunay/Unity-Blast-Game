using UnityEngine;

public class AutomaticDestroy : MonoBehaviour {
    private void Start() {
        Destroy(gameObject, 1f);
    }
}
