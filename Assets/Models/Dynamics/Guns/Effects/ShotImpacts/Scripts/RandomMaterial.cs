using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public Material[] Materials;

    void Awake()
    {
        var material = Materials[Random.Range(0, Materials.Length - 1)];
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }
}
