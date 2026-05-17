using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ExplosionVFX : MonoBehaviour
{
    private ParticleSystem ps;
    private bool pendingReturn = false;

    public void Init()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        pendingReturn = false;
        ps.Play();
    }

    private void OnParticleSystemStopped()
    {
        pendingReturn = true;
    }

    private void Update()
    {
        if (pendingReturn)
        {
            pendingReturn = false;
            VFXPool.Instance.Return(this); // 👈 NO usa referencias internas
        }
    }
}
