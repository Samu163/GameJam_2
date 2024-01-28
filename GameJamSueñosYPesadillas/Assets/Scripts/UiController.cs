using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{

    public GameObject playerActionsRef;

    public void SetPlayerActions(bool isActive)
    {
        playerActionsRef.SetActive(isActive);
    }
}
