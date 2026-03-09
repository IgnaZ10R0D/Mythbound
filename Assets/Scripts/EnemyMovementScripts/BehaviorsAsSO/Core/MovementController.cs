using UnityEngine;

public class MovementController : MonoBehaviour
{
    private MovementInstance currentMovement;

    private float timeFactor = 1f;

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged += OnTimeWarpChanged;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeWarpChanged -= OnTimeWarpChanged;
    }

    private void OnTimeWarpChanged(float factor)
    {
        timeFactor = factor;
    }

    public void PlayMovement(MovementBehaviour behaviour, MovementParams parameters)
    {
        if (currentMovement != null)
        {
            currentMovement.Stop();
        }

        currentMovement = behaviour.CreateInstance(transform, parameters);
        currentMovement.Start();
    }

    public void StopMovement()
    {
        if (currentMovement != null)
            currentMovement.Stop();

        currentMovement = null;
    }

    public void Update()
    {
        if (currentMovement == null) return;

        currentMovement.Tick(Time.deltaTime * timeFactor);

        if (currentMovement.IsFinished)
        {
            currentMovement = null;
        }
    }

    public bool IsBusy => currentMovement != null;
}