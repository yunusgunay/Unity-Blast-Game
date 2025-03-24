using System;
using System.Threading.Tasks;

public class ComboManager : Singleton<ComboManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public async void TryExecute(Cell tappedCell)
    {
        // If the tapped cell does not contain a rocket, execute normally.
        if (!(tappedCell.item is RocketItem))
        {
            tappedCell.item.TryExecute();
            _ = MovesManager.Instance.DecreaseMovesAsync();
            return;
        }

        // Check if there's an adjacent rocket.
        Cell adjacentRocket = FindAdjacentRocket(tappedCell);
        if (adjacentRocket != null)
        {
            // Two adjacent rockets -> Execute combo.
            // Stop falling while combo animation plays.
            FallAndFillManager.Instance.StopFall();
            RocketCombo.ExecuteCombo(tappedCell);
            // Optionally disable touch for a short time.
            TouchManager tm = GetComponent<TouchManager>();
            if (tm != null)
                tm.enabled = false;
            await Task.Delay(TimeSpan.FromSeconds(1));
            if (tm != null)
                tm.enabled = true;
        }
        else
        {
            // Single rocket explosion.
            tappedCell.item.TryExecute();
        }

        _ = MovesManager.Instance.DecreaseMovesAsync();
    }

    private Cell FindAdjacentRocket(Cell tappedCell)
    {
        foreach (Cell neighbor in tappedCell.neighbours)
        {
            if (neighbor.item is RocketItem)
            {
                return neighbor;
            }
        }
        return null;
    }
}
