using UnityEngine;
using System;

public class RocketManager : MonoBehaviour {
    public static RocketManager Instance { get; private set; }

    public GameBoard board;

    [Header("Parted Rocket Prefabs")]
    public GameObject partedLeftPrefab;
    public GameObject partedRightPrefab; 
    public GameObject partedTopPrefab; 
    public GameObject partedBottomPrefab;
    public GameObject smokePrefab;

    private int activePartedRockets = 0;
    public event Action OnAllPartedRocketsFinished;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PartedRocketSpawned() {
        activePartedRockets++;
    }

    public void PartedRocketFinished() {
        activePartedRockets--;
        if (activePartedRockets <= 0) {
            activePartedRockets = 0;
            OnAllPartedRocketsFinished?.Invoke();
        }
    }

}
