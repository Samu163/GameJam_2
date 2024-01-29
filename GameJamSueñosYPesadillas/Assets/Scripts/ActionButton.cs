using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public Image selectedIcon;
    public Button button;


    public void ShowSelectedIcon(bool condition)
    {
        selectedIcon.gameObject.SetActive(condition);
    }

    public void ActiveOnClick()
    {
        button.onClick.Invoke();
    }
}
