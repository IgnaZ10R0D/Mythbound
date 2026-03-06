using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Base Animations")]
    [SerializeField] private Sprite[] idle;
    [SerializeField] private Sprite[] move;

    [Header("Attack Animations")]
    [SerializeField] private AttackAnimation[] attacks;

    [Header("Attack + Move Animations")]
    [SerializeField] private AttackAnimation[] attackMoves;

    [SerializeField] private float fps = 12f;

    private float timer;
    private int frame;

    private Sprite[] currentAnimation;

    public enum State
    {
        Idle,
        Move,
        AttackStart,
        AttackLoop,
        AttackEnd
    }

    private State state;
    private AttackAnimation currentAttack;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayIdle();
    }

    void Update()
    {
        if (currentAnimation == null || currentAnimation.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f / fps)
        {
            timer = 0f;
            frame++;

            if (frame >= currentAnimation.Length)
            {
                HandleAnimationEnd();
                frame = Mathf.Clamp(frame, 0, currentAnimation.Length - 1);
            }

            if (spriteRenderer != null)
                spriteRenderer.sprite = currentAnimation[frame];
        }
    }

    void HandleAnimationEnd()
    {
        switch (state)
        {
            case State.AttackStart:
                PlayAttackLoop();
                break;

            case State.AttackEnd:
                PlayIdle();
                break;

            case State.AttackLoop:
                frame = 0;
                break;

            default:
                frame = 0;
                break;
        }
    }

    void Play(Sprite[] anim, State newState)
    {
        if (anim == null || anim.Length == 0)
        {
            anim = idle; 
            newState = State.Idle;
        }

        currentAnimation = anim;
        state = newState;

        frame = 0;
        timer = 0f;

        if (spriteRenderer != null && anim.Length > 0)
            spriteRenderer.sprite = anim[0];
    }

    public void PlayIdle()
    {
        Play(idle, State.Idle);
    }

    public void PlayMove()
    {
        if (move == null || move.Length == 0)
            PlayIdle();
        else
            Play(move, State.Move);
    }

    public void PlayAttack(int index = 0)
    {
        if (attacks == null || attacks.Length == 0)
        {
            PlayIdle();
            return;
        }

        index = Mathf.Clamp(index, 0, attacks.Length - 1);
        currentAttack = attacks[index];

        Play(currentAttack.start, State.AttackStart);
    }

    public void PlayAttackLoop()
    {
        if (currentAttack == null)
        {
            PlayIdle();
            return;
        }

        Play(currentAttack.loop, State.AttackLoop);
    }

    public void PlayAttackEnd()
    {
        if (currentAttack == null)
        {
            PlayIdle();
            return;
        }

        if (currentAttack.end != null && currentAttack.end.Length > 0)
            Play(currentAttack.end, State.AttackEnd);
        else
            PlayReverse(currentAttack.start);
    }
    public void PlayAttackMove(int index = 0)
    {
        if (attackMoves == null || attackMoves.Length == 0)
        {
            PlayAttack(index);
            return;
        }

        index = Mathf.Clamp(index, 0, attackMoves.Length - 1);
        currentAttack = attackMoves[index];

        Play(currentAttack.start, State.AttackStart);
    }

    void PlayReverse(Sprite[] source)
    {
        if (source == null || source.Length == 0)
        {
            PlayIdle();
            return;
        }

        Sprite[] reversed = new Sprite[source.Length];

        for (int i = 0; i < source.Length; i++)
            reversed[i] = source[source.Length - 1 - i];

        Play(reversed, State.AttackEnd);
    }
}






