using UnityEngine;

public class BossDeath : MonoBehaviour
{
    void OnDestroy()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();

        foreach (var bullet in bullets)
        {
            if (bullet.gameObject.activeSelf)
                bullet.gameObject.SetActive(false);
        }
    }
}
