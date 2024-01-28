using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image playerImage;
    public PlayerConfig config;
    public int life;
    public GameObject DispararButton;
    public HabilityController habilityPrefabRef;
    public List<HabilityController> buttonHabilities;
    UnityAction<string, PlayerController> _applyHability;


    public bool isFighting = false;

    public void Init(PlayerConfig config, UnityAction<string, PlayerController> applyHability)
    {
        this.config = config;
        _applyHability = applyHability;
        life = config.life;

    }

    public void ShowFightMenu()
    {
        isFighting = !isFighting;

        DispararButton.gameObject.SetActive(isFighting);
    }

    public void InitHabilities()
    {
        for (int i = 0; i < config.habilities.Count; i++)
        {
            var hability = Instantiate(habilityPrefabRef, transform);
            hability.transform.position = new Vector3(hability.transform.position.x + 100 + 100 * i, hability.transform.position.y, hability.transform.position.z);

            hability.Init(config.habilities[i], UseHability);
            buttonHabilities.Add(hability);
        }
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

    public void UseHability(string habilityName)
    {
        ShowHabilities(false);
        _applyHability.Invoke(habilityName, this);
        
    }
}
