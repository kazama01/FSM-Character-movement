using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine;

public class fadeAmounController : MonoBehaviour
{
    [Header("Fade Settings")]
    [Range(-0.1f, 1f)]
    public float fadeAmount = 0f;
    [Range(0.1f, 5f)]
    public float fadeDuration = 2f;
    public bool startFadeOnEnable = true;

    private SpriteRenderer spriteRenderer;
    private float startValue;
    private float targetValue;
    private float fadeTimer;
    private bool isFading;
    private MaterialPropertyBlock propertyBlock;
    private const string FADE_PROPERTY = "_FadeAmount";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        
        if (spriteRenderer == null)
        {
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (startFadeOnEnable && spriteRenderer != null && propertyBlock != null)
        {
            StartFade(0f, fadeAmount);
        }
    }

    private void Start()
    {
        spriteRenderer.GetPropertyBlock(propertyBlock);
        startValue = propertyBlock.GetFloat(FADE_PROPERTY);
    }

    private void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(fadeTimer / fadeDuration);
        
        spriteRenderer.GetPropertyBlock(propertyBlock);
        float currentValue = propertyBlock.GetFloat(FADE_PROPERTY);
        float newValue = Mathf.Lerp(startValue, targetValue, progress);
        
        propertyBlock.SetFloat(FADE_PROPERTY, newValue);
        spriteRenderer.SetPropertyBlock(propertyBlock);
        

        if (progress >= 1f)
        {
            isFading = false;
        }
    }

    public void StartFade(float from, float to)
    {
        if (spriteRenderer == null || propertyBlock == null)
        {
            return;
        }

        startValue = from;
        targetValue = to;
        fadeTimer = 0f;
        isFading = true;
        
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(FADE_PROPERTY, from);
        spriteRenderer.SetPropertyBlock(propertyBlock);
        
    }
}
