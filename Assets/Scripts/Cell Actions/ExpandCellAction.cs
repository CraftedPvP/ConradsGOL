using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    /// <summary>
    /// This expands to neighboring empty / dead <see cref="Cell"/>s if it exactly matches 3 live neighbors.
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
                if (neighbor) continue;
                Vector3 pos = cell.transform.position;
                Vector3 dir = CheckNeighborsCellAction.NeighborDirections[i] * GameManager.Instance.GameSettings.CellSize;
                pos.x += dir.x;
                pos.y += dir.y;
                emptyNeighbors.Add(i, pos);
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

                // skip if we've already marked this cell for spawning
                if (CellsThatNeedSpawning.Contains(emptyCellPos)) continue;
                
                // check requirements for expansion
                if (CheckIfMetRequirements(directionIndex, emptyCellPos)){
                    CellsThatNeedSpawning.Add(emptyCellPos);
                    // Map.Instance.SpawnCellAtPosition(emptyCellPos);
                    // Debug.Log("Going to expand into empty cell at " + emptyCellPos);
                }
            }
            emptyNeighbors.Clear();
        }
        /// <summary>
        /// for each empty cell, check if it meets the requirements to expand into
        /// </summary>
        /// <param name="directionIndex">used to fetch the direction if needed</param>
        /// <param name="emptyCellPos">world position of the empty cell</param>
        /// <returns>true, if can expand into the empty cell</returns>
        bool CheckIfMetRequirements(int directionIndex, Vector3 emptyCellPos)
        {
            int exactRequirementForExpansion = 3;
            int liveNeighborCount = 0;
            RaycastHit2D[] raycastHits = new RaycastHit2D[1];

            // in the loop below, we want to check if the empty cell has exactly N live neighbors
            // if it goes beyond N neighbors, we can exit early as it no longer meets the requirement

            // create a raycast in set directions to check for neighbors
            for (int i = 0; i < CheckNeighborsCellAction.NeighborDirections.Length; i++)
            {
                Vector3 offset = CheckNeighborsCellAction.NeighborDirections[i] * (GameManager.Instance.GameSettings.CellSize / 2);
                int hits = Physics2D.RaycastNonAlloc(
                    emptyCellPos + offset,
                    CheckNeighborsCellAction.NeighborDirections[i],
                    raycastHits,
                    // raycast padding to ensure we hit the diagonal neighbor
                    GameManager.Instance.GameSettings.CellSize * GameManager.Instance.GameSettings.RaycastPadding,
                    GameManager.Instance.GameSettings.CellLayerMask
                );
                if (hits > 0) {
                    liveNeighborCount++;
                 
                    // we can put the condition in the for loop but we kept it here for clarity
                    if (liveNeighborCount > exactRequirementForExpansion) break;
                }
            }
            
            // Debug.Log("live count for empty cell at " + emptyCellPos + " is " + liveNeighborCount);
            return liveNeighborCount == exactRequirementForExpansion;
        }

        public override void PostExecute()
        {
            SpawnNeededCells();
        }
        void SpawnNeededCells()
        {
            Map.Instance.AddCellsInPositions(CellsThatNeedSpawning);
            CellsThatNeedSpawning.Clear();
        }
    }
}
