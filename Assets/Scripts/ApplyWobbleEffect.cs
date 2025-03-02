using UnityEngine;

public class ApplyWobbleEffect : MonoBehaviour
{
    public Material wobbleMaterial; // Reference to the material with the PSX Wobble Shader
    private Material originalMaterial; // To store the original material
    private MaterialPropertyBlock propertyBlock; // To modify properties without replacing the material

    void Start()
    {
        // Get the MeshRenderer component
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Save the original material
            originalMaterial = meshRenderer.material;

            // Initialize MaterialPropertyBlock
            propertyBlock = new MaterialPropertyBlock();

            // Apply the original material first
            meshRenderer.material = originalMaterial;
        }
    }

    void Update()
    {
        // Modify the wobble effect in the material dynamically
        // Get the MeshRenderer and set the PropertyBlock for the effect
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null && wobbleMaterial != null)
        {
            // Set properties of the wobble shader (you can add more if needed)
            propertyBlock.SetFloat("_WobbleStrength", Mathf.Sin(Time.time) * 0.1f); // Example of wobble effect
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
