using UnityEngine;

public class ShaderEffectController
{
    private static MaterialPropertyBlock propertyBlock;
    private readonly SpriteRenderer spriteRenderer;
    private readonly Material materialInstance;
    private float effectTimer;

    public ShaderEffectController(SpriteRenderer renderer, Material existingMaterial = null)
    {
        if (renderer == null)
        {
            Debug.LogError("[ShaderEffectController] Null SpriteRenderer passed!");
            return;
        }
        
        spriteRenderer = renderer;

        if (existingMaterial != null)
        {
            materialInstance = existingMaterial;
            Debug.Log("[ShaderEffectController] Using existing material instance");
        }
        else
        {
            materialInstance = new Material(spriteRenderer.material);
            spriteRenderer.material = materialInstance;
            Debug.Log("[ShaderEffectController] Created new material instance");
        }
        
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
    }

    public void StopEffect()
    {
        UpdatePropertyBlock(0f, "_HitEffectBlend");
    }

    private void UpdatePropertyBlock(float intensity, string propertyName, Color? color = null)
    {
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(propertyName, intensity);
        
        if (color.HasValue)
        {
            propertyBlock.SetColor("_HitEffectColor", color.Value);
        }
        
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }

    public void UpdateFlashEffect(float deltaTime, float speed, float maxIntensity, Color color)
    {
        effectTimer += deltaTime;
        float flash = Mathf.Abs(Mathf.Sin(effectTimer * speed)) * maxIntensity;
        UpdatePropertyBlock(flash, "_HitEffectBlend", color);
    }

    public void StartEffect()
    {
        effectTimer = 0f;
        UpdatePropertyBlock(0f, "_HitEffectBlend");
    }

    ~ShaderEffectController()
    {
        if (materialInstance != null)
        {
            Object.Destroy(materialInstance);
        }
    }
}