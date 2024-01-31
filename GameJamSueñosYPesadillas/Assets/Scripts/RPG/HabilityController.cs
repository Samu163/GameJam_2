using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HabilityController : MonoBehaviour
{
    public string habilityName;
    public SpriteRenderer image;
    public SpriteRenderer selectedImage;
    public bool hasTargetEnemy;
    public bool hasTargetPlayer;
    public string description;
    UnityAction<string, bool,bool> _onHabilityClick;

    public void Init(HabilityConfig hability, UnityAction<string, bool,bool> onHabilityClick)
    {
        habilityName = hability.name;
        image.sprite = hability.icon;
        hasTargetPlayer = hability.hasTarget;
        hasTargetEnemy = hability.hasTargetEnemy;
        _onHabilityClick = onHabilityClick;
        if(GameManager.instance.language != null)
        {
            if (GameManager.instance.language == "Español")
            {
                description = hability.descriptionEspañol;

            }
            else
            {
                description = hability.descriptionIngles;

            }
        }
        else
        {
            description = hability.descriptionIngles;
        }

        ShowSelectedImage(false);
    }

    public void ShowSelectedImage(bool condition)
    {
        selectedImage.gameObject.SetActive(condition);
    }

    public void OnButtonClick()
    {
        _onHabilityClick.Invoke(habilityName, hasTargetEnemy, hasTargetPlayer);
    }



}
