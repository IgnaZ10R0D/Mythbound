using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator animator;

    static readonly int Moving = Animator.StringToHash("Moving");
    static readonly int Attack = Animator.StringToHash("Attack");
    static readonly int AttackType = Animator.StringToHash("AttackType");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoving(bool value)
    {
        animator.SetBool(Moving, value);
    }

    public void StartAttack(int attackType)
    {
        animator.SetInteger(AttackType, attackType);
        animator.SetBool(Attack, true);
    }

    public void StopAttack()
    {
        animator.SetBool(Attack, false);
    }
}





