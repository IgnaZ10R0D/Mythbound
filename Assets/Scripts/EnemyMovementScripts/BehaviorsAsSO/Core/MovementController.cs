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
            Debug.Log("[MovementController] Forzando stop de movimiento anterior");
            currentMovement.Stop();
        }

        currentMovement = behaviour.CreateInstance(transform, parameters);
        Debug.Log($"[MovementController] Iniciando nuevo movimiento: {behaviour.GetType().Name}");
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

        Debug.Log($"[MovementController] Tick - IsFinished: {currentMovement.IsFinished} | Type: {currentMovement.GetType().Name}");

        currentMovement.Tick(Time.deltaTime);

        if (currentMovement.IsFinished)
        {
            Debug.Log($"[MovementController] MOVIMIENTO TERMINADO → liberando");
            currentMovement = null;
        }
    }
    public bool IsBusy => currentMovement != null;
}
