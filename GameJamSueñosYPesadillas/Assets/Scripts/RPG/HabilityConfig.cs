using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewhabilityConfig", menuName = "Configs/hability")]


public class HabilityConfig : ScriptableObject
{
    public string name;
    public Sprite icon;
    public bool hasTarget;
    public bool hasTargetEnemy;
}
