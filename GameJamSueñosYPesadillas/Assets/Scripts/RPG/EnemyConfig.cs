using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Configs/Enemy")]


public class EnemyConfig : ScriptableObject
{
    public int life;
    public int attack;
    public Sprite enemyIcon;
    public List<EnemyHabilityConfig> habilities;

}
