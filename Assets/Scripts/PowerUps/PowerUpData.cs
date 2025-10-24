using UnityEngine;

[System.Serializable]
public class PowerUpData
{
    public PowerUp powerUp;         
    public int thresholdPoints;     

    [Header("UI")]
    public Sprite icon;
    public string displayName;
}


