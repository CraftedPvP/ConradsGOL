using UnityEngine;

namespace Michael
{
    /// <summary>
    /// A cell action that changes the future state of a cell based on its live neighbors.
    /// </summary>
    [CreateAssetMenu(fileName = "Change State", menuName = "Michael/Cell Actions/Change State")]
    public class ChangeStateCellAction : ICellAction
    {
        public override void Execute(Cell cell)
        {
            int liveNeighbors = cell.LiveNeighborCount;

            // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            if (!cell.IsAlive)
            {
                if (liveNeighbors == 3) cell.FutureIsAlive = true;
                return;
            }

            // Any live cell with fewer than two live neighbors dies, as if by underpopulation.
            if (liveNeighbors < 2) cell.FutureIsAlive = false;
            // Any live cell with two or three live neighbors lives on to the next generation.
            else if (liveNeighbors == 2 || liveNeighbors == 3) cell.FutureIsAlive = true;
            // Any live cell with more than three live neighbors dies, as if by overpopulation.
            else if (liveNeighbors > 3) cell.FutureIsAlive = false;
        }
    }
}
