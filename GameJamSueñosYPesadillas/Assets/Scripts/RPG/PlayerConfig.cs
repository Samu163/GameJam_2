using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "Configs/Player")]

public class PlayerConfig : ScriptableObject
{
    public string name;
    public int life;
    public int maxLife;
    public int attackPower;
    public int defense;
    public List<HabilityConfig> habilities;
}
