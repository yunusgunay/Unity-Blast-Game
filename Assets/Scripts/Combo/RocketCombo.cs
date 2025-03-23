using System.Collections.Generic;

public class RocketCombo : ComboEffect
{
    public override void ApplyEffect(Cell tappedCell)
    {
        // Find all adjacent rocket cells
        List<Cell> rocketCells = new List<Cell>();
        rocketCells.Add(tappedCell);
        
        foreach (Cell neighbor in tappedCell.neighbours)
        {
            if (neighbor.item is RocketItem)
            {
                rocketCells.Add(neighbor);
            }
        }
        
        // For each rocket, first store their positions, then explode them
        // This prevents modifying the board while iterating
        foreach (Cell rocketCell in rocketCells)
        {
            // It's important to remove the item from the cell BEFORE calling TryExecute
            // to prevent infinite recursion
            Item rocketItem = rocketCell.item;
            rocketCell.item = null;
            
            // Now explode the rocket
            if (rocketItem != null)
                rocketItem.TryExecute();
        }
        
        // Create 3x3 explosion effect at the tapped cell location
        Create3x3Explosion(tappedCell);
    }

    public override List<Cell> GetAffectedCells(Cell cell)
    {
        throw new System.NotImplementedException();
    }

    private void Create3x3Explosion(Cell centerCell)
    {
        // Get all cells in a 3x3 grid centered on the tapped cell
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int targetX = centerCell.X + x;
                int targetY = centerCell.Y + y;
                
                // Get the cell if it exists
                GameBoard board = RocketManager.Instance.board;
                if (targetX >= 0 && targetX < board.Cols && 
                    targetY >= 0 && targetY < board.Rows)
                {
                    Cell targetCell = board.Cells[targetX, targetY];
                    if (targetCell != null && targetCell.item != null)
                    {
                        targetCell.item.TryExecute();
                    }
                }
            }
        }
    }
}
