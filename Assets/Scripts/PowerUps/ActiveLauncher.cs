using UnityEngine;

public class ActiveLauncher : PowerUp
{
    [SerializeField] private GameObject activePrefab;

    public override void UsePowerUp()
    {
        if (activePrefab == null) return;

        Player player = FindFirstObjectByType<Player>();
        Vector3 spawnPos = player != null ? player.transform.position : transform.position;
        GameObject instance = Instantiate(activePrefab, spawnPos, Quaternion.identity);

        if (instance.transform.localScale == Vector3.zero)
            instance.transform.localScale = Vector3.one;
    }
}




