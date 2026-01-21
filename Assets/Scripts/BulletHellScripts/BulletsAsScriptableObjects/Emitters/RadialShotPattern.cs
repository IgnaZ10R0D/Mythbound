using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Bullets/Radial Shot Patterns")]
public class RadialShotPattern : ScriptableObject
{
    public int Repetitions;
    [Range(-180f, 180f)] public float AngleOffsetBetweenReps = 0f;
    public float StartWait = 0f;
    public float EndWait = 0f;
    public RadialShotSettings[] PatternSettings;
}
