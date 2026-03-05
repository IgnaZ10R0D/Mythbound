using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBehaviour : ScriptableObject
{
    public abstract MovementInstance CreateInstance(Transform owner, MovementParams parameters);
}
