using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAnimationProfile animationProfile;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void SetAnimationProfile(EnemyAnimationProfile newProfile)
    {
        animationProfile = newProfile;
    }

    public void SetMoving(bool isMoving)
    {
        if (animationProfile != null && animationProfile.usesOnlyIdleAnimation) return;

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    public void SetAttacking(bool isAttacking)
    {
        if (animationProfile != null && animationProfile.usesOnlyIdleAnimation) return;

        if (animator != null)
        {
            animator.SetBool("isAttacking", isAttacking);
        }
    }
}





