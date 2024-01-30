using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig config;
    public int life;
    public int attack;
    public int defense;
    public int idEnemy;
    public SpriteRenderer selectedIcon;
    public void Init()
    {
        life = config.life;
        attack = config.attack;
        defense = config.defense;
        idEnemy = config.idEnemy;
        ShowSelectedIcon(false);
    }

    public void ShowSelectedIcon(bool condition)
    {
        selectedIcon.gameObject.SetActive(condition);
    }


   



}
