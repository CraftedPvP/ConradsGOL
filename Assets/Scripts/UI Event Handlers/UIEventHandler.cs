using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    /// <summary>
    /// bridge between UI and game logic
    /// <code>
    /// [CreateAssetMenu(fileName = "Event Name", menuName = "Michael/UI Event Handler/Name")]
    /// </code>
    /// </summary>
    public abstract class UIEventHandler : ScriptableObject
    {
    }
}
