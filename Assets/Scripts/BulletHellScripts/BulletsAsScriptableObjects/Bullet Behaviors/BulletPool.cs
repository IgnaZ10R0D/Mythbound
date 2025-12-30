using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;

    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("No Bullet Pool Instance found");
            }
            return _instance;
        }
    }
    
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int initialPoolSize = 10;
    private List<Bullet> _bulletPool = new List<Bullet>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        
        AddBulletsToPool(initialPoolSize);
    }

    private void AddBulletsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletPool.Add(bullet);
            bullet.transform.parent = transform;
        }
    }

    public Bullet RequestBullet()
    {
        for (int i = 0; i < _bulletPool.Count; i++)
        {
            if (!_bulletPool[i].gameObject.activeSelf)
            {
                _bulletPool[i].gameObject.SetActive(true);
                return _bulletPool[i];
            }
        }
        AddBulletsToPool(initialPoolSize);
        _bulletPool[_bulletPool.Count - 1].gameObject.SetActive(false);
        return _bulletPool[_bulletPool.Count - 1];
    }
}
