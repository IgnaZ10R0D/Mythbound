using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletConfig", menuName = "Bullet/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [Header("Standard Bullet Behaviors")]
    public float duration = 5f;
    public float movementSpeed = 3f;
    public float delayBeforeTargeting = 1.5f; 
    public float followSeconds = 2f;
    
    public enum PillarExpandDirection { Up, Down, Both }
    public PillarExpandDirection expandDirection = PillarExpandDirection.Down;
    public float expandSpeed = 5f;


    [Header("Visual")]
    public Sprite bulletSprite;
}

