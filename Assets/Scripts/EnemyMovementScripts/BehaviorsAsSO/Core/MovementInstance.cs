using UnityEngine;

public abstract class MovementInstance
{
    protected Transform owner;

    protected float elapsedTime;
    protected bool isFinished;

    protected float duration; 

    public bool IsFinished => isFinished;

    protected MovementInstance(Transform owner, float duration = 0f)
    {
        this.owner = owner;
        this.duration = duration;
    }

    public virtual void Start()
    {
        elapsedTime = 0f;
        isFinished = false;
    }

    public virtual void Tick(float deltaTime)
    {
        if (isFinished)
            return;

        elapsedTime += deltaTime;
        if (duration > 0f && elapsedTime >= duration)
        {
            OnDurationCompleted();
        }
    }

    protected virtual void OnDurationCompleted()
    {
        isFinished = true;
    }

    public virtual void Stop()
    {
        isFinished = true;
    }

    protected float GetNormalizedTime()
    {
        if (duration <= 0f)
            return 0f;

        return Mathf.Clamp01(elapsedTime / duration);
    }
}
