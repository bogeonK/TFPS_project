using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTilingController : MonoBehaviour
{
    // Ÿ�ϸ� ������ ������ ����
    public float tileX = 1.0f;
    public float tileY = 1.0f;

    void Start()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // ť���� ũ�⿡ ���� Ÿ�ϸ� ����
            Vector3 scale = transform.localScale;
            meshRenderer.material.mainTextureScale = new Vector2(scale.x / tileX, scale.y / tileY);
        }
    }
}