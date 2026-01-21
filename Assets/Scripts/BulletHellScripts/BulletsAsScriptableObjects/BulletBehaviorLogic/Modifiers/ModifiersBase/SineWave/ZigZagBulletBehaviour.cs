using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Behaviours/ZigZag")]
public class ZigZagBulletBehaviour : BulletBehaviour
{
    public float amplitude = 1f;     // Qué tan fuerte oscila
    public float frequency = 5f;     // Qué tan rápido oscila

    public override BulletBehaviourInstance CreateInstance()
    {
        return new ZigZagBulletBehaviourInstance(this);
    }
}

