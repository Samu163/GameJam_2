using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HabilityController : MonoBehaviour
{
    public TextMeshProUGUI habilityName;
    public SpriteRenderer image;
    public SpriteRenderer selectedImage;
    public bool hasTargetEnemy;
    public bool hasTargetPlayer;
    UnityAction<string, bool,bool> _onHabilityClick;

    public void Init(HabilityConfig hability, UnityAction<string, bool,bool> onHabilityClick)
    {
        //habilityName.text = hability.name;
        image.sprite = hability.icon;
        hasTargetPlayer = hability.hasTarget;
        hasTargetEnemy = hability.hasTargetEnemy;
        _onHabilityClick = onHabilityClick;
        ShowSelectedImage(false);
    }

    public void ShowSelectedImage(bool condition)
    {
        selectedImage.gameObject.SetActive(condition);
    }

    public void OnButtonClick()
    {
        _onHabilityClick.Invoke(habilityName.text, hasTargetEnemy, hasTargetPlayer);
    }



}
