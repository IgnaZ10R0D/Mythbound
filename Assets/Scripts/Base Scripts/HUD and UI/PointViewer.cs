using UnityEngine;
using UnityEngine.UI;

public class PointViewer : MonoBehaviour
{
    public Text pointsText;

    private PointManager pointManager;
    private PowerUpController powerUpController;

    void Start()
    {
        pointManager = FindFirstObjectByType<PointManager>();
        powerUpController = FindFirstObjectByType<PowerUpController>();

        if (pointManager != null)
        {
            pointManager.OnPointsChanged += UpdatePointsUI;
            UpdatePointsUI(pointManager.GetCurrentPoints()); // inicial
        }
    }

    void OnDestroy()
    {
        if (pointManager != null)
            pointManager.OnPointsChanged -= UpdatePointsUI;
    }

    void UpdatePointsUI(int currentPoints)
    {
        if (pointsText == null || powerUpController == null)
            return;

        int threshold = powerUpController.GetRelevantThreshold(currentPoints);

        pointsText.text = $"Power: {currentPoints}/{threshold}";
    }
}

