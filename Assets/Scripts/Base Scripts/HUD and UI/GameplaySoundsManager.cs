using UnityEngine;

public class GameplaySoundsManager : MonoBehaviour
{
    public static GameplaySoundsManager Instance { get; private set; }

    [Header("Referencia al SoundPool")]
    [SerializeField] private SoundPool soundPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string key)
    {
        soundPool.Play(key);
    }
}

