using UnityEngine;

public class ImageScroll : MonoBehaviour
{
    [SerializeField] private float _speed = 30f;
    [SerializeField] private Renderer _bgRenderer;
    
    void Update()
    {
        _bgRenderer.material.mainTextureOffset += new Vector2(0, _speed * Time.deltaTime);
    }
}




