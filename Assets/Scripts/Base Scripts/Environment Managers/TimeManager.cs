using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    
    [SerializeField] private float timeSlow = 1f;
    public float TimeSlow
    {
        get => timeSlow;
        private set
        {
            timeSlow = Mathf.Clamp(value, 0f, 2f);
            OnTimeWarpChanged?.Invoke(timeSlow);
        }
    }

    public event Action<float> OnTimeWarpChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetTimeSlow(float factor)
    {
        TimeSlow = factor;
    }

    public void ResetTimeSlow()
    {
        TimeSlow = 1f;
    }
}


