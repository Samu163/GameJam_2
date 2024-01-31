using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEnemyConfig", menuName = "Configs/Enemy")]


public class EnemyConfig : ScriptableObject
{
    public int life;
    public int maxLife;
    public int attack;
    public int defense;
    public int idEnemy;
    public Sprite enemyIcon;
    public List<EnemyHabilityConfig> habilities;

}
