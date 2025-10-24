using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    /// <code>
    /// [CreateAssetMenu(fileName = "Action Name", menuName = "Michael/Cell Action/Name")]
    /// </code>
    public abstract class ICellAction : ScriptableObject
    {
        /// <summary>
        /// called for each living cell in the map
        /// </summary>
        /// <param name="cell">cell being processed</param>
        public abstract void Execute(Cell cell);
        /// <summary>
        /// called after all cells have been processed
        /// </summary>
        public abstract void PostExecute();
    }
}
