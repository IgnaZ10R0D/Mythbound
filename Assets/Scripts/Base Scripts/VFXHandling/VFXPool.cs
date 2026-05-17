using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    public static VFXPool Instance { get; private set; }

    private Dictionary<ParticleSystem, List<ExplosionVFX>> pool = new();
    private Dictionary<ExplosionVFX, ParticleSystem> reverseLookup = new();

    private void Awake()
    {
        Instance = this;
    }

    public ExplosionVFX Get(ParticleSystem prefab)
    {
        if (!pool.TryGetValue(prefab, out var list))
        {
            list = new List<ExplosionVFX>();
            pool[prefab] = list;
        }

        foreach (var v in list)
        {
            if (!v.gameObject.activeInHierarchy)
            {
                v.gameObject.SetActive(true);
                return v;
            }
        }

        var ps = Instantiate(prefab, transform);
        ps.gameObject.SetActive(true);

        var vfx = ps.gameObject.AddComponent<ExplosionVFX>();
        vfx.Init(); // 👈 ya no recibe nada

        list.Add(vfx);
        reverseLookup[vfx] = prefab;

        return vfx;
    }

    public void Return(ExplosionVFX vfx)
    {
        vfx.gameObject.SetActive(false);
    }

    public ParticleSystem GetPrefab(ExplosionVFX vfx)
    {
        return reverseLookup[vfx];
    }
}
