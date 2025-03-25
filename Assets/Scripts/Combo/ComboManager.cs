using System;
using System.Threading.Tasks;
using UnityEngine;

// ComboManager manages the combo system of the game.
public class ComboManager : MonoBehaviour {
    public static ComboManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public async void TryExecute(Cell tappedCell) {
        // If the tapped cell doesn't contain a rocket, execute its action normally.
        if (!(tappedCell.item is RocketItem)) {
            tappedCell.item.TryExecute();
            _ = MovesManager.Instance.DecreaseMovesAsync();
            return;
        }

        // If the tapped cell does contain a rocket, check for an adjacent rocket.
        Cell adjacentRocket = GetAdjacentRocket(tappedCell);
        if (adjacentRocket != null) {     
            FallAndFillManager.Instance.StopFall(); // pause falling during the combo animation
            RocketCombo.ExecuteCombo(tappedCell);   // execute Rocket-to-Rocket Combo
            
            TouchManager touchManager = GetComponent<TouchManager>();
            if (touchManager != null) { touchManager.enabled = false; }
            await Task.Delay(TimeSpan.FromSeconds(1));
            if (touchManager != null) { touchManager.enabled = true; }
        } 
        else {
            tappedCell.item.TryExecute(); // single rocket explosion
        }

        _ = MovesManager.Instance.DecreaseMovesAsync();
    }

    private Cell GetAdjacentRocket(Cell tappedCell) {
        foreach (Cell neighbor in tappedCell.adjacentCells) {
            if (neighbor.item is RocketItem) { 
                return neighbor; 
            }
        }
        return null;
    }

}
