using UnityEngine;
using System;

public class PointManager : MonoBehaviour
{
    public event Action<int> OnPointsChanged;

    [SerializeField] private int startingPoints = 0; 
    private int _points;
    public int Points => _points;

    private void Awake()
    {
        _points = startingPoints;
        SafeInvoke(_points);
    }

    public void AddPoints(int amount)
    {
        _points += amount;
        SafeInvoke(_points);
    }

    public void SubtractPoints(int amount)
    {
        _points = Mathf.Max(0, _points - amount);
        SafeInvoke(_points);
    }

    public void ResetPoints()
    {
        _points = startingPoints; 
        SafeInvoke(_points);
    }

    public int GetCurrentPoints()
    {
        return _points;
    }
    private void SafeInvoke(int points)
    {
        if (OnPointsChanged == null) return;

        foreach (var d in OnPointsChanged.GetInvocationList())
        {
            try
            {
                (d as Action<int>)?.Invoke(points);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"PointManager: Listener inválido removido ({e.Message})");
                OnPointsChanged -= (Action<int>)d;
            }
        }
    }
}

