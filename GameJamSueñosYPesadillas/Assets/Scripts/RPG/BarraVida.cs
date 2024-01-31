using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barra;

    public Text namePlayer;
    
   public void loadLife(float life, float maxLife)
    {
        barra.fillAmount = life / maxLife;
    }
}
