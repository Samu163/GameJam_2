using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHability : MonoBehaviour
{
    //Los players va tener toda la informacion el RPGManager
    //Cuando se presione este boton Se dispare un UnityAction 
    public HabilityConfig hability;



    public void Init(HabilityConfig hability)
    {
        this.hability = hability;
    }

    public void OnButtonClick()
    {

    }
}
