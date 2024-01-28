using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HabilityController : MonoBehaviour
{
    public TextMeshProUGUI habilityName;
    public Image image;
    public bool hasTarget;
    UnityAction<string> _onHabilityClick;

    public void Init(HabilityConfig hability, UnityAction<string> onHabilityClick)
    {
        habilityName.text = hability.name;
        image.sprite = hability.icon;
        hasTarget = hability.hasTarget;
        _onHabilityClick = onHabilityClick;
    }


    public void OnButtonClick()
    {
        _onHabilityClick.Invoke(habilityName.text);
    }



}
