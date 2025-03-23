using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ComboManager : Singleton<ComboManager>
{
    private Dictionary<ComboType, ComboEffect> comboEffects;
    private List<Cell> matchedCells; // Potentially used to store rocket adjacency.
    
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        comboEffects = new Dictionary<ComboType, ComboEffect>
        {
            {ComboType.Rocket, ScriptableObject.CreateInstance<RocketCombo>()},
        };
    }

    public async void TryExecute(Cell tappedCell)
    {
        ComboType comboType = GetComboType(tappedCell);

        if(comboType == ComboType.None)
        {
            tappedCell.item.TryExecute();
        }

        else if (comboEffects.TryGetValue(comboType, out var effect))
        {
            // Stop fall and fill while combo is active
            FallAndFillManager.Instance.StopFall();

            effect.ApplyEffect(tappedCell);

            // Disable the inputs during animation.
            TouchManager tm = this.GetComponent<TouchManager>();
            if(tm != null) tm.enabled = false;

            // Animation delay
            await Task.Delay(TimeSpan.FromSeconds(1));

            if(tm != null) tm.enabled = true;
        }
        
        _ = MovesManager.Instance.DecreaseMovesAsync();
    }
    
    public ComboType GetComboType(Cell tappedCell)
    {
        if(!(tappedCell.item is RocketItem)) 
            return ComboType.None;

        // Check neighbors for another rocket
        bool foundAnotherRocket = false;
        foreach(Cell neighbor in tappedCell.neighbours)
        {
            if(neighbor.item is RocketItem)
            {
                // We found at least two adjacent rockets
                foundAnotherRocket = true;
                return ComboType.Rocket;
            }
        }

        return foundAnotherRocket ? ComboType.Rocket : ComboType.None;   
    }

}
