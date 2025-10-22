using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Random Life Chance", menuName = "Michael/Cell Actions/Random Life Chance")]
    public class RandomLifeChanceCellAction : ICellAction
    {
        [Tooltip("The lower the value, the less likely a cell is to be alive")]
        [SerializeField, Range(0f, 1f)]
        float aliveChance = 0.5f;

        public override void Execute(Cell cell)
        {
            // Check if the cell should be alive
            cell.FutureIsAlive = Random.value < aliveChance;
            // Debug.Log($"Cell at {cell.name} set to {(cell.FutureIsAlive ? "Alive" : "Dead")}");
        }
    }
}
