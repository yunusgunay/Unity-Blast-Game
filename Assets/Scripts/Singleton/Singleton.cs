using UnityEngine;

// A generic base class that enforces a single instance of type "Type" in the scene.
public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour {
    private static Type _cachedInstance;
    
    public static Type Instance {
        get {
            if (_cachedInstance == null) { 
                _cachedInstance = FindAnyObjectByType<Type>(); 
            }
            return _cachedInstance;
        }
    }

    protected virtual void Awake() {
        if (Instance != this) {
            Destroy(gameObject);
        }
    }

}
