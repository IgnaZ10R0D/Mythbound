using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private PointManager pointManager;

    private Dictionary<System.Type, GameObject> activePowerUps = new Dictionary<System.Type, GameObject>();

    void Start()
    {
        pointManager = FindFirstObjectByType<PointManager>();
    }

    public void AddAbility<T>(GameObject prefab) where T : PowerUp
    {
        if (this == null || gameObject == null) return;

        System.Type type = typeof(T);

        if (activePowerUps.ContainsKey(type))
        {
            return;
        }

        if (prefab != null)
        {
            if (transform == null) return;

            GameObject instance = Instantiate(prefab, transform);
            instance.transform.localPosition = Vector3.zero;

            if (instance.GetComponent<T>() == null)
            {
                instance.AddComponent<T>();
            }

            activePowerUps[type] = instance;
        }
    }

    public void RemoveAbility<T>() where T : PowerUp
    {
        System.Type type = typeof(T);

        if (activePowerUps.TryGetValue(type, out GameObject instance))
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            activePowerUps.Remove(type);
        }
    }

    public void CheckAbility<T>(int pointThreshold, GameObject prefab) where T : PowerUp
    {
        if (pointManager != null)
        {
            int currentPoints = pointManager.GetCurrentPoints();

            if (currentPoints >= pointThreshold)
            {
                AddAbility<T>(prefab);
            }
            else
            {
                RemoveAbility<T>();
            }
        }
    }

    public void UseAbility<T>() where T : PowerUp
    {
        System.Type type = typeof(T);

        if (activePowerUps.TryGetValue(type, out GameObject instance))
        {
            if (instance != null)
            {
                T ability = instance.GetComponent<T>();
                if (ability != null)
                {
                    ability.UsePowerUp();
                }
            }
        }
    }

    private void OnDestroy()
    {
        activePowerUps.Clear();
    }
}

