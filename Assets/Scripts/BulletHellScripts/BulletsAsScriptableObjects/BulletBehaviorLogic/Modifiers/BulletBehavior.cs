using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BulletBehaviour : ScriptableObject
{
    public abstract BulletBehaviourInstance CreateInstance();
}
