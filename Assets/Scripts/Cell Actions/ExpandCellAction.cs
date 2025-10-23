using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    /// <summary>
    /// This expands to neighboring empty / dead <see cref="Cell"/>s.
    /// </summary>
    [CreateAssetMenu(fileName = "Expand", menuName = "Michael/Cell Actions/Expand")]
    public class ExpandCellAction : ICellAction
    {
        public static HashSet<Vector3> CellsThatNeedSpawning { get; private set; } = new HashSet<Vector3>();
        // cache the list object so we don't create new ones every execution
        Dictionary<int, Vector2> emptyNeighbors = new Dictionary<int, Vector2>();
        public override void Execute(Cell cell)
        {
            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                Cell neighbor = cell.Neighbors[i];
                if (!neighbor || !neighbor.IsAlive)
                {
                    Vector3 pos = cell.transform.position;
                    Vector3 dir = CheckNeighborsCellAction.NeighborDirections[i] * GameManager.Instance.GameSettings.CellSize;
                    pos.x += dir.x;
                    pos.y += dir.y;
                    emptyNeighbors.Add(i, pos);
                }
            }
            CheckIfCanExpand();
        }
        /// <summary>
        /// loop through each empty neighbor and check if it meets the requirements to expand into
        /// </summary>
        void CheckIfCanExpand()
        {
            foreach (var kvp in emptyNeighbors)
            {
                int directionIndex = kvp.Key;
                Vector2 emptyCellPos = kvp.Value;

                // check requirements for expansion
                if (CheckIfMetRequirements(directionIndex, emptyCellPos)){
                    CellsThatNeedSpawning.Add(emptyCellPos);
                    // Map.Instance.SpawnCellAtPosition(emptyCellPos);
                    Debug.Log("Going to expand into empty cell at " + emptyCellPos);
                }
            }
            emptyNeighbors.Clear();
        }
        /// <summary>
        /// for each empty cell, check if it meets the requirements to expand into
        /// </summary>
        /// <param name="directionIndex"></param>
        /// <param name="emptyCellPos"></param>
        /// <returns>true, if can expand into the empty cell</returns>
        bool CheckIfMetRequirements(int directionIndex, Vector3 emptyCellPos)
        {
            int minRequirementForExpansion = 3;
            int liveNeighborCount = 0;

            // create a raycast in set directions to check for neighbors
            for (int i = 0;
                i < CheckNeighborsCellAction.NeighborDirections.Length &&
                liveNeighborCount < minRequirementForExpansion; i++)
            {
                Vector3 offset = CheckNeighborsCellAction.NeighborDirections[i] * (GameManager.Instance.GameSettings.CellSize / 2);
                RaycastHit2D hit = Physics2D.Raycast(
                    emptyCellPos + offset,
                    CheckNeighborsCellAction.NeighborDirections[i],
                    // raycast padding to ensure we hit the diagonal neighbor
                    GameManager.Instance.GameSettings.CellSize * GameManager.Instance.GameSettings.RaycastPadding,
                    GameManager.Instance.GameSettings.CellLayerMask
                );
                if (!hit.collider) continue;
                Cell neighbor = hit.collider.GetComponent<Cell>();
                if (neighbor.IsAlive) liveNeighborCount++;
            }
            // Debug.Log("live count for empty cell at " + emptyCellPos + " is " + liveNeighborCount);
            return liveNeighborCount >= minRequirementForExpansion;
        }

        public override void PostExecute()
        {
            SpawnNeededCells();
        }
        void SpawnNeededCells()
        {
            foreach (var pos in CellsThatNeedSpawning)
                Map.Instance.SpawnCellAtPosition(pos);
            CellsThatNeedSpawning.Clear();
        }
    }
}
