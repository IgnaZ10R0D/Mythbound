using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RadialShotSettings
{
    [Header("Radial Shot Settings")]
    public int NumberOfBullets = 5;
    public float BulletSpeed = 10f;
    public float CooldownAfterShot;
    [Header("Offsets")] 
    [Range(-1f, 1f)] public float PhaseOffset = 0f;
    [Range(-180f,180f)] public float AngleOffset = 0f;
    [Header("Radial Mask")] 
    public bool RadialMask;
    [Range(0f, 360f)] public float MaskAngle = 360f;
    [Header("Visual Elements")] 
    public Sprite BulletSprite;
    public Vector3 BulletScale;
    [Header("Bullet Behaviours")]
    public BulletBehaviour BehaviorProfile;
    public BulletActivationCondition ConditionProfile;
}
