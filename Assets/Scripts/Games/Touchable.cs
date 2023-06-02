using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    Renderer _renderer;
    Material material;
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        material = _renderer.material;
        gameObject.AddComponent<MeshCollider>();

    }
    public void MakeItGlow(bool state)
    {
        _renderer.material = state ? MaterialProvider.Instance.GlowMaterial : material;
    }
}
