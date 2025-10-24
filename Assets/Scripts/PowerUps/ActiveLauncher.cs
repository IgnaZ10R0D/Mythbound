using UnityEngine;

public class ActiveLauncher : PowerUp
{
    [SerializeField] private GameObject activePrefab;

    public override void UsePowerUp()
    {
        if (activePrefab != null)
        {
            GameObject instance = Instantiate(activePrefab, transform.position, Quaternion.identity);

            if (instance.transform.localScale == Vector3.zero)
                instance.transform.localScale = Vector3.one;
        }
    }
}



