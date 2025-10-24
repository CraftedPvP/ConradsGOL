using UnityEngine;

namespace Michael
{
    /// <summary>
    /// A cell action that checks for neighboring cells and updates the cell's neighbor list.
    /// </summary>
    [CreateAssetMenu(fileName = "Check Neighbors", menuName = "Michael/Cell Actions/Check Neighbors")]
    public class CheckNeighborsCellAction : ICellAction
    {
        public static Vector2[] NeighborDirections = new Vector2[]
        { // starts from top and goes clockwise
            Vector2.up,
            new Vector2(1, 1),
            Vector2.right,
            new Vector2(1, -1),
            Vector2.down,
            new Vector2(-1, 1),
            Vector2.left,
            new Vector2(-1, -1)
        };
        public override void Execute(Cell cell)
        {
            cell.Neighbors = CheckNeighbors(cell);
            // Debug.Log($"Cell at {cell.name} has {cell.LiveNeighborCount} live neighbors.", cell);
        }

        public Cell[] CheckNeighbors(Cell cell)
        {
            // create a raycast in set directions to check for neighbors
            Cell[] neighbors = new Cell[NeighborDirections.Length];
            for (int i = 0; i < NeighborDirections.Length; i++)
            {
                // put offset slightly before the edge of the cell
                Vector3 offset = NeighborDirections[i] * (GameManager.Instance.GameSettings.CellSize * .45f);
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    cell.transform.position + offset,
                    NeighborDirections[i],
                    // raycast padding to ensure we hit the diagonal neighbor
                    GameManager.Instance.GameSettings.CellSize * GameManager.Instance.GameSettings.RaycastPadding,
                    GameManager.Instance.GameSettings.CellLayerMask
                );
                if (hits.Length == 0)
                {
                    neighbors[i] = null;
                    continue;
                }

                for (int hitIndex = 0; hitIndex < hits.Length; hitIndex++)
                {
                    RaycastHit2D hit = hits[hitIndex];
                    // skip self
                    if (hit.collider == cell.Collider) continue;

                    Cell neighbor = hit.collider.GetComponent<Cell>();
                    neighbors[i] = neighbor;
                    break;
                }
            }
            return neighbors;
        }

        public override void PostExecute()
        {
        }
    }
}
