using UnityEngine;

public class ImageScroll : MonoBehaviour
{
    [System.Serializable]
    public class ScrollLayer
    {
        public SpriteRenderer baseRenderer;
        public Vector2 scrollSpeed;
    }

    [SerializeField] private ScrollLayer[] _layers;

    private class RuntimeLayer
    {
        public SpriteRenderer a;
        public SpriteRenderer b;
        public Vector2 size;
        public Vector2 speed;
        public bool vertical;
        public Vector3 origin;
    }

    private RuntimeLayer[] _runtimeLayers;

    void Start()
    {
        _runtimeLayers = new RuntimeLayer[_layers.Length];

        for (int i = 0; i < _layers.Length; i++)
        {
            SpriteRenderer original = _layers[i].baseRenderer;

            Bounds bounds = original.bounds;
            Vector2 size = bounds.size;

            bool vertical = Mathf.Abs(_layers[i].scrollSpeed.y) >= Mathf.Abs(_layers[i].scrollSpeed.x);

            Vector3 offset = vertical
                ? new Vector3(0f, size.y, 0f)
                : new Vector3(size.x, 0f, 0f);

            SpriteRenderer duplicate = Instantiate(
                original,
                original.transform.position + offset,
                original.transform.rotation,
                original.transform.parent
            );

            _runtimeLayers[i] = new RuntimeLayer
            {
                a = original,
                b = duplicate,
                size = size,
                speed = _layers[i].scrollSpeed,
                vertical = vertical,
                origin = original.transform.position
            };
        }
    }

    void Update()
    {
        foreach (var layer in _runtimeLayers)
        {
            Vector3 delta = (Vector3)(layer.speed * Time.deltaTime);

            layer.a.transform.position += delta;
            layer.b.transform.position += delta;

            HandleLoop(layer);
        }
    }

    private void HandleLoop(RuntimeLayer layer)
    {
        float dirX = Mathf.Sign(layer.speed.x);
        float dirY = Mathf.Sign(layer.speed.y);

        HandleSprite(layer, layer.a, dirX, dirY);
        HandleSprite(layer, layer.b, dirX, dirY);
    }

    private void HandleSprite(RuntimeLayer layer, SpriteRenderer sr, float dirX, float dirY)
    {
        Vector3 offset = sr.transform.position - layer.origin;

        if (layer.vertical)
        {
            float limit = layer.size.y;

            if (dirY < 0 && offset.y <= -limit)
                sr.transform.position += Vector3.up * limit * 2f;
            else if (dirY > 0 && offset.y >= limit)
                sr.transform.position -= Vector3.up * limit * 2f;
        }
        else
        {
            float limit = layer.size.x;

            if (dirX < 0 && offset.x <= -limit)
                sr.transform.position += Vector3.right * limit * 2f;
            else if (dirX > 0 && offset.x >= limit)
                sr.transform.position -= Vector3.right * limit * 2f;
        }
    }
}







