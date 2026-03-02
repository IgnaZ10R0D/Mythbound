using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementParams
{
    public float waitTime;
    public Vector2 offset;

    public float speed;
    public float duration;

    public float amplitude;
    public float frequency;

    public Vector2 direction;

    public bool smoothStop = false;
    public AnimationCurve stopCurve = AnimationCurve.Linear(0,1,1,0);
}
