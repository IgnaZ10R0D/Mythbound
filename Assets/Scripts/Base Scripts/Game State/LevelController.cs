using UnityEngine;
using System;

public class LevelController : MonoBehaviour
{
    [SerializeField] private string levelID;
    [SerializeField] private int killThreshold = 30;

    private bool triggered = false;

    public static event Action OnSomethingChanged;

    public void RegisterKill()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.RegisterEnemyKill(levelID);

        if (triggered)
            return;

        int kills = GameManager.Instance.GetKills(levelID);

        if (kills >= killThreshold)
        {
            triggered = true;

            GameManager.Instance.ActivateSomethingChanged(levelID);
            OnSomethingChanged?.Invoke();
        }
    }
}
