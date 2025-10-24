using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootDelay = 0.1f;
    private float shootTimer = 0f;
    public float offsetX = 1f;
    public float offsetY = 1f;
    private Vector3 referencePosition;
    private PlayerSounds playerSounds;
    private AudioSource audioSource;

    void Start()
    {
        referencePosition = transform.localPosition;
        playerSounds = GetComponent<PlayerSounds>();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(InputManager.Instance.GetKey("FocusFireMode")))
        {
            transform.localPosition = new Vector3(referencePosition.x + offsetX, referencePosition.y + offsetY, referencePosition.z);
        }
        else
        {
            transform.localPosition = referencePosition;
        }
    }


    private void HandleShooting()
    {
        if (Input.GetKey(InputManager.Instance.GetKey("Shoot")))
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootDelay)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f;
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (playerSounds != null)
        {
            playerSounds.PlaySound("BasicAttack");
        }
    }
}


