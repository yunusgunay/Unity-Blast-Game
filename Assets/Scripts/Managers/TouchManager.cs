using UnityEngine;

// TouchManager reads user clicks/taps, sends them to the cell for handling.
public class TouchManager : MonoBehaviour {
    [SerializeField] private new Camera camera;
    
    void Update() {
        #if UNITY_EDITOR 
            // Check Editor Input
            if (Input.GetMouseButtonUp(0)) {
                ProcessTap(Input.mousePosition);
            }
        
        #else 
            // Check Touch Input
            if (Input.touchCount > 0) {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    ProcessTap(touch.position);
                }
            }
        
        #endif
    }

    void ProcessTap(Vector3 screenPos) {
        Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
        BoxCollider2D hit = Physics2D.OverlapPoint(worldPos) as BoxCollider2D;

        if (hit != null) {
            Cell tappedCell = hit.GetComponent<Cell>();
            if (tappedCell != null) {
                tappedCell.OnCellTapped();
            }
        }
    }

    void DisableTouch() {
        enabled = false;
    }

    void OnEnable() {
        MovesTracker.Instance.OnNoMovesLeft += DisableTouch;
        GoalManager.Instance.OnGoalsCompleted += DisableTouch;
    }

    void OnDisable() {
        MovesTracker.Instance.OnNoMovesLeft -= DisableTouch;
        GoalManager.Instance.OnGoalsCompleted -= DisableTouch;
    }

}
