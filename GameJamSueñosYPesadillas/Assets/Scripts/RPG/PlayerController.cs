using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public SpriteRenderer playerImage;
    public PlayerConfig config;
    public int life;
    public int attackPower;
    public int defense;
    public bool hasAttacked;
    public SpriteRenderer playerSelected;
    public HabilityController habilityPrefabRef;
    public List<HabilityController> buttonHabilities;
    UnityAction<string, bool, bool> _applyHability;
    public Animator AnimatorPlayer;
  

    public bool isFighting = false;

    public void Init(UnityAction<string, bool,bool> applyHability)
    {
       AnimatorPlayer = GetComponent<Animator>();
        _applyHability = applyHability;
        life = config.life;
        attackPower = config.attackPower;
        defense = config.defense;
        ShowPlayerSelectedIcon(false);

    }

    public void InitHabilities()
    {
        for (int i = 0; i < config.habilities.Count; i++)
        {
            var hability = Instantiate(habilityPrefabRef);
            hability.transform.position = new Vector3(transform.position.x + 150 + 100 * i, transform.position.y+100, transform.position.z);
            hability.Init(config.habilities[i], UseHability);
            buttonHabilities.Add(hability);
        }
    }

    public void ShowPlayerSelectedIcon(bool condition) 
    {
        playerSelected.gameObject.SetActive(condition);
    }


    public void DestroyHabilities()
    {
        for (int i = 0; i < buttonHabilities.Count; i++)
        {
            Destroy(buttonHabilities[i].gameObject);
        }
    }

    public void ShowHabilities(bool condition)
    {
        for (int i = 0; i < buttonHabilities.Count; i++)
        {
            buttonHabilities[i].gameObject.SetActive(condition);
        }
    }

    public void UseHability(string habilityName, bool hasTargetEnemy, bool hasTragetPlayer)
    {
        ShowHabilities(false);
        _applyHability.Invoke(habilityName, hasTargetEnemy, hasTragetPlayer);
        
    }
}
