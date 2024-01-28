using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "Configs/Player")]

public class PlayerConfig : ScriptableObject
{
    public int life;
    public List<HabilityConfig> habilities;
}
