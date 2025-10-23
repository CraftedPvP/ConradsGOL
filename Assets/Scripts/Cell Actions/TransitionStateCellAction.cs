using UnityEngine;

namespace Michael
{
    /// <summary>
    /// A cell action that transitions the cell to its future state.
    /// </summary>
    [CreateAssetMenu(fileName = "Transition State", menuName = "Michael/Cell Actions/Transition State")]
    public class TransitionStateCellAction : ICellAction
    {
        public override void Execute(Cell cell)
        {
            cell.TransitionState();
        }
    }
}
