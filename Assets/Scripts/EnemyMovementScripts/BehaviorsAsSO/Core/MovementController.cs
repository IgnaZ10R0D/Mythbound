using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private MovementInstance currentMovement;

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
        currentMovement.Tick(Time.deltaTime);

        if (currentMovement.IsFinished)
        {
            currentMovement = null;
        }
    }
    public bool IsBusy => currentMovement != null;
}
