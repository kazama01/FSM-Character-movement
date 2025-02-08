using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeAmounController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float fadeAmount = 0f;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            spriteRenderer.material.SetFloat("_FadeAmount", fadeAmount);
        }
    }
}
