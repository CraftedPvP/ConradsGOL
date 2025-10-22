using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Check Neighbors", menuName = "Michael/Cell Actions/Check Neighbors")]
    public class CheckNeighborsCellAction : ICellAction
    {
        [SerializeField] LayerMask cellLayerMask;
        Vector2[] NeighborDirections = new Vector2[]
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
            Debug.Log($"Cell at {cell.transform.position} has {cell.LiveNeighborCount} live neighbors.");
        }

        public Cell[] CheckNeighbors(Cell cell)
        {
            // create a raycast in set directions to check for neighbors
            Cell[] neighbors = new Cell[8];
            for (int i = 0; i < NeighborDirections.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(
                    cell.transform.position,
                    NeighborDirections[i],
                    // 1.8f is padding to ensure we hit the diagonal neighbor
                    GameManager.Instance.GameSettings.CellSize * 1.8f,
                    cellLayerMask
                );
                if (hit.collider == null) continue;

                Cell neighbor = hit.collider.GetComponent<Cell>();
                if (neighbor != null) neighbors[i] = neighbor;
            }
            return neighbors;
        }
    }
}
