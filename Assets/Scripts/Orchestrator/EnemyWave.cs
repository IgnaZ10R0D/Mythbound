[System.Serializable]
public class EnemyWave
{
    public MovementBehaviour[] movements;
    public MovementParams[] movementParams;
    public bool shootInParallel;
    public RadialShotWeapon[] weapons;
}
[System.Serializable]
public class WaveWeapon
{
    public RadialShotWeapon weapon;
    public int waveIndex;           
}
[System.Serializable]
public class MovementEntry
{
    public MovementBehaviour behaviour;
    public MovementParams parameters;

    // Tiempo que espera antes de disparar armas (si shootInParallel = false)
    public float waitTime;
}