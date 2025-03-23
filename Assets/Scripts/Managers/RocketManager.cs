using UnityEngine;
using System;

public class RocketManager : Singleton<RocketManager>
{
    public GameBoard board;

    [Header("Parted Rocket Prefabs")]
    public GameObject partedLeftPrefab;
    public GameObject partedRightPrefab; 
    public GameObject partedTopPrefab; 
    public GameObject partedBottomPrefab;

    private int activePartedRockets = 0;
    public event Action OnAllPartedRocketsFinished;

    public void PartedRocketSpawned()
    {
        activePartedRockets++;
    }

    public void PartedRocketFinished()
    {
        activePartedRockets--;
        if (activePartedRockets <= 0)
        {
            activePartedRockets = 0;
            OnAllPartedRocketsFinished?.Invoke();
        }
    }
}
