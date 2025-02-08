using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "Ninja Status", menuName = "Ninja/Status Config")]
public class NinjaStatusConfig : ScriptableObject
{
    [Header("Basic Status")]
   
    public float Health = 100f;
   
    public float MoveSpeed = 10f;
  
    public float JumpForce = 5f;

    [Header("Knockback Settings")]
    
    public float HorizontalKnockback = 5f;
   
    public float VerticalKnockback = 2.5f;
    
    public float KnockbackDuration = 0.5f;

    public float FallGravityMultiplier = 2f;

    [Header("Ground Settings")]
    [ValueDropdown("GetAllTags")] // Using Odin's ValueDropdown
    public string GroundTag = "Ground";

    private static IEnumerable GetAllTags()
    {
        return UnityEditorInternal.InternalEditorUtility.tags;
    }
}
