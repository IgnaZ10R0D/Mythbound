using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10f;

    [SerializeField] private KeyCode shiftKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode altShiftKey = KeyCode.RightShift;
    [SerializeField] private PointManager pointManager;

    void Start()
    {
        pointManager = FindFirstObjectByType<PointManager>();
    }

    public float CalculateDamage()
    {
        if (Input.GetKey(shiftKey) || Input.GetKey(altShiftKey))
        {
            return baseDamage + (pointManager.Points * 0.03f);
        }
        else
        {
            return baseDamage;
        }
    }

    public void SetBaseDamage(float newBaseDamage)
    {
        baseDamage = newBaseDamage;
    }

    
}

