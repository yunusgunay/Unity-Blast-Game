using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

// MovesTracker monitors the player's remaining moves.
public class MovesTracker : Singleton<MovesTracker> {
    [SerializeField] private TextMeshProUGUI movesLabel;
    private int currentMoves;
    public Action OnNoMovesLeft;
    
    public void Init(int initialMoves) {
        currentMoves = initialMoves;
        UpdateLabel();
    }

    public async Task DecrementMoves() {
        currentMoves--;

        if (currentMoves <= 0) {
            currentMoves = 0;

            var input = GetComponent<TouchManager>();
            if (input != null) { input.enabled = false; } // discard inputs

            UpdateLabel();
            await Task.Delay(TimeSpan.FromSeconds(1));
            OnNoMovesLeft?.Invoke();
            return;
        }

        UpdateLabel();
    }

    void UpdateLabel() {
        if (movesLabel != null) {
            movesLabel.text = currentMoves.ToString();
        }
    }

}
