using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName="Combat/GridObjectSO")]
public class GridObjectSO : ScriptableObject
{
    [Header("Stats")]
    public int   maxHealth       = 10;
    public int   chipThreshold   = 5;      // when do “chip” effects start?
    
    [Header("Behavior Flags")]
    public bool  isPushable      = true;   // can entities shove this?
    public bool  isBreakable     = true;   // does it shatter when HP ≤ 0?

    [Header("Visual Prefab")]
    public GameObject prefab;             // the 3D/sprite model to spawn

    [Header("Events")]
    public UnityEvent onHit;              // e.g. sparks, dust
    public UnityEvent onBreak;            // spawn debris, loot, etc.
}