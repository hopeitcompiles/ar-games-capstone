using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour
{
    Renderer renderer;
    Material material;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        gameObject.AddComponent<MeshCollider>();

    }
    public void MakeItGlow(bool state)
    {
        renderer.material = state ? MaterialProvider.Instance.GlowMaterial : material;
    }
}
