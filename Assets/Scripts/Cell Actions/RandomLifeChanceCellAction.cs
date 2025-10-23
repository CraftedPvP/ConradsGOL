using UnityEngine;

namespace Michael
{
    /// <summary>
    /// A cell action that sets the future "alive" state of a cell based on a random chance.
    /// </summary>
    [CreateAssetMenu(fileName = "Random Life Chance", menuName = "Michael/Cell Actions/Random Life Chance")]
    public class RandomLifeChanceCellAction : ICellAction
    {
        [Tooltip("The lower the value, the less likely a cell is to be alive")]
        [SerializeField, Range(0f, 1f)]
        public float AliveChance = 0.5f;

        public override void Execute(Cell cell)
        {
            // Check if the cell should be alive
            cell.FutureIsAlive = Random.value < AliveChance;
            // Debug.Log($"Cell at {cell.name} set to {(cell.FutureIsAlive ? "Alive" : "Dead")}");
        }
    }
}
