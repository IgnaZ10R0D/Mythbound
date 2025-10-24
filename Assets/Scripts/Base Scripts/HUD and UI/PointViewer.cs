using UnityEngine;
using UnityEngine.UI;

public class PointViewer : MonoBehaviour
{
    public Text pointsText; 
    private PointManager pointManager;

    void Start()
    {
        pointManager = FindFirstObjectByType<PointManager>();
    }

    void Update()
    {
        if (pointManager != null && pointsText != null)
        {
            pointsText.text = "Power: " + pointManager.GetCurrentPoints().ToString();
        }
    }
}

