using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public EnemyConfig data;
    public Image enemyIcon;
    public int life;
    public Image selectedIcon;
    public void Init(EnemyConfig config)
    {
        data = config;
        life = config.life;
        enemyIcon.sprite = config.enemyIcon;
    }

    public void ShowSelectedIcon()
    {
        selectedIcon.gameObject.SetActive(false);
    }


   



}
