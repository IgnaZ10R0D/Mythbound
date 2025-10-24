using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed = 10f; 
    [SerializeField] float lifetime = 2f; 
    private float timer = 0f;

    void Update()
    {
        
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        
        timer += Time.deltaTime;

        
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
}

