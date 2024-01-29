using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig data;
    public SpriteRenderer enemyIcon;
    public int life;
    public int attack;
    public int defense;
    public SpriteRenderer selectedIcon;
    public void Init(EnemyConfig config)
    {
        data = config;
        life = config.life;
        attack = config.attack;
        defense = config.defense;
        enemyIcon.sprite = config.enemyIcon;
    }

    public void ShowSelectedIcon(bool condition)
    {
        selectedIcon.gameObject.SetActive(condition);
    }


   



}
