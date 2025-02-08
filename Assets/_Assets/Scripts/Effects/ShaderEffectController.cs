using UnityEngine;

public class ShaderEffectController
{
    private static MaterialPropertyBlock propertyBlock;
    private readonly SpriteRenderer spriteRenderer;
    private float effectTimer;
    private float fadeTimer;
    private bool isFading;
    private const string FADE_AMOUNT = "_FadeAmount";

    public ShaderEffectController(SpriteRenderer renderer)
    {
        spriteRenderer = renderer;
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

    public void StartFade(float initialValue = 0f)
    {
        fadeTimer = 0f;
        isFading = true;
        spriteRenderer.material.SetFloat(FADE_AMOUNT, initialValue);
    }

    public void UpdateFade(float deltaTime, float fadeDuration)
    {
        if (!isFading) return;

        fadeTimer += deltaTime;
        float progress = fadeTimer / fadeDuration;
        float fadeAmount = Mathf.Lerp(0f, 1f, progress);
        
        spriteRenderer.material.SetFloat(FADE_AMOUNT, fadeAmount);

        if (progress >= 1f)
        {
            isFading = false;
        }
    }

    public bool IsFadingComplete()
    {
        return !isFading;
    }
}