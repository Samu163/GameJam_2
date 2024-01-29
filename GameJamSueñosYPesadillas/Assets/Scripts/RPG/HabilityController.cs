using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HabilityController : MonoBehaviour
{
    public TextMeshProUGUI habilityName;
    public Button button;
    public Image image;
    public Image selectedImage;
    public bool hasTargetEnemy;
    public bool hasTarget;
    UnityAction<string> _onHabilityClick;

    public void Init(HabilityConfig hability, UnityAction<string> onHabilityClick)
    {
        habilityName.text = hability.name;
        image.sprite = hability.icon;
        hasTarget = hability.hasTarget;
        _onHabilityClick = onHabilityClick;
        ShowSelectedImage(false);
    }

    public void ShowSelectedImage(bool condition)
    {
        selectedImage.gameObject.SetActive(condition);
    }

    public void OnButtonClick()
    {
        _onHabilityClick.Invoke(habilityName.text);
    }



}
