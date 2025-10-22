using UnityEngine;

namespace Michael
{
    [CreateAssetMenu(fileName = "Transition State", menuName = "Michael/Cell Actions/Transition State")]
    public class TransitionStateCellAction : ICellAction
    {
        public override void Execute(Cell cell)
        {
            cell.TweenBasedOnState();
        }
    }
}
