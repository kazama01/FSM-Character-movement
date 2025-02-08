using UnityEngine;

[CreateAssetMenu(fileName = "NinjaStateConfig", menuName = "Ninja/State Config")]
public class NinjaStateConfig : ScriptableObject
{
    [Header("Idle Settings")]
    public AnimationClip idleAnimation;
    public float idleAnimationSpeed = 1f;

    [Header("Run Settings")]
    public AnimationClip runAnimation;
    public float runAnimationSpeed = 1f;

    [Header("Jump Settings")]
    public AnimationClip jumpAnimation;
    public float jumpAnimationSpeed = 1f;

    [Header("Attack Settings")]
    public AnimationClip attackAnimation;
    public float attackDuration = 0.5f;
    public float attackAnimationSpeed = 1f;

    [Header("Hurt Settings")]
    public AnimationClip hurtAnimation;
    public float hurtDuration = 0.5f;
    public float hurtAnimationSpeed = 1f;

    [Header("Hit Effect Settings")]
    [Tooltip("Material property name for hit effect intensity")]
    public string hitEffectProperty = "_HitEffectBlend";
    [Range(0f, 1f)]
    public float maxHitEffect = 0.3f;
    [Range(1f, 50f)]
    public float hitFlashSpeed = 20f;
    [ColorUsage(true, true)]
    [Tooltip("Color for the _HitEffectColor shader property")]
    public Color hitEffectColor;

    [Header("Death Settings")]
    public AnimationClip deathAnimation;
    public float deathAnimationSpeed = 1f;

    [Header("Death Effect Settings")]
    public float fadeDuration = 2f;  // Controls how long it takes to fade out

}