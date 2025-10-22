using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    public abstract class ICellAction : ScriptableObject
    {
        public abstract void Execute(Cell cell);
    }
}
