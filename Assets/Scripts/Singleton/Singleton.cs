using UnityEngine;

public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour
{
    private static Type instance;
    public static Type Instance {
        get {
            if ( instance == null ) { instance = FindAnyObjectByType(typeof(Type)) as Type; }
            return instance;
        }
    }

    protected virtual void Awake() {
        if ( Instance != this ) {
            Destroy(gameObject);
        }
    }
}
