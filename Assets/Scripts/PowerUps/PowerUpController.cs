using UnityEngine;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpData[] powerUps;

    private Player player;
    private PowerUpUIController uiController;
    private PointManager pointManager;

    private List<ActiveLauncher> unlockedActivePowerUps = new List<ActiveLauncher>();
    private int currentActiveIndex = -1;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        uiController = FindFirstObjectByType<PowerUpUIController>();
        pointManager = FindFirstObjectByType<PointManager>();

        if (uiController != null)
            uiController.InitializeIcons(powerUps);

        if (pointManager != null)
        {
            pointManager.OnPointsChanged += HandlePointsChanged;
            HandlePointsChanged(pointManager.GetCurrentPoints());
        }
    }

    private void OnDestroy()
    {
        if (pointManager != null)
            pointManager.OnPointsChanged -= HandlePointsChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.GetKey("CycleSpell")) && unlockedActivePowerUps.Count > 1)
        {
            currentActiveIndex = (currentActiveIndex + 1) % unlockedActivePowerUps.Count;
            uiController?.HighlightSelectedPowerUp(unlockedActivePowerUps[currentActiveIndex]);
        }

        if (Input.GetKeyDown(InputManager.Instance.GetKey("ActiveSpellCard")) && unlockedActivePowerUps.Count > 0 && currentActiveIndex >= 0)
        {
            ActiveLauncher selected = unlockedActivePowerUps[currentActiveIndex];
            PowerUpData data = null;

            foreach (var powerUpData in powerUps)
            {
                if (powerUpData.powerUp == selected)
                {
                    data = powerUpData;
                    break;
                }
            }

            if (data != null && pointManager != null && pointManager.GetCurrentPoints() >= data.thresholdPoints)
            {
                selected.UsePowerUp();
                pointManager.SubtractPoints(data.thresholdPoints);

                RevalidateSelection();
            }
        }
    }

    private void HandlePointsChanged(int currentPoints)
    {
        if (pointManager == null) return;

        RevalidateSelection();
        uiController?.UpdatePowerUpIcons(currentPoints);
    }

    private void RevalidateSelection()
    {
        int currentPoints = pointManager != null ? pointManager.GetCurrentPoints() : 0;
        unlockedActivePowerUps.Clear();

        foreach (var powerUpData in powerUps)
        {
            var type = powerUpData.powerUp.GetType();
            var method = player.GetType().GetMethod("CheckAbility").MakeGenericMethod(type);
            method.Invoke(player, new object[] { powerUpData.thresholdPoints, powerUpData.powerUp.gameObject });

            if (powerUpData.powerUp is ActiveLauncher activeLauncher && currentPoints >= powerUpData.thresholdPoints)
            {
                unlockedActivePowerUps.Add(activeLauncher);
            }
        }

        if (unlockedActivePowerUps.Count == 0)
        {
            currentActiveIndex = -1;
            uiController?.ClearSelection();
        }
        else if (currentActiveIndex < 0 || currentActiveIndex >= unlockedActivePowerUps.Count)
        {
            currentActiveIndex = 0;
            uiController?.HighlightSelectedPowerUp(unlockedActivePowerUps[currentActiveIndex]);
        }
        else
        {
            uiController?.HighlightSelectedPowerUp(unlockedActivePowerUps[currentActiveIndex]);
        }
    }

    public void UsePowerUp<T>() where T : PowerUp
    {
        foreach (var powerUpData in powerUps)
        {
            if (powerUpData.powerUp is T)
            {
                int threshold = powerUpData.thresholdPoints;
                if (pointManager != null && pointManager.GetCurrentPoints() >= threshold)
                {
                    player.UseAbility<T>();
                    pointManager.SubtractPoints(threshold);
                    RevalidateSelection();
                }
            }
        }
    }
}


