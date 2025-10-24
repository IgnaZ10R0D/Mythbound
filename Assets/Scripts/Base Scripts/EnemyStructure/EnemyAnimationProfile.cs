using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Animation Profile")]
public class EnemyAnimationProfile : ScriptableObject
{
    public bool usesOnlyIdleAnimation = false;

    public string idleAnimation;
    public string movementAnimation;

    [Tooltip("Attack animation sequence (i.e: intro, loop, finish)")]
    public string[] attackSequence;
}

