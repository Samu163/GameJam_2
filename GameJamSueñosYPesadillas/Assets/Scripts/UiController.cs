using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Image itemsBg;
    public TextRPGInfo textDisplay;

    public GameObject lifebar;
    public BarraVida barraVida;
    public List<BarraVida> barraVidas;

    public void Start()
    {
        lifebar = barraVida.gameObject;
    }

    public void Init()
    {
        ShowItemsBg(false);
        ShowTextDisplay(false);
        HideAllLifeBars();
    }

    public void HideAllLifeBars()
    {
        for (int i = 0; i < barraVidas.Count; i++)
        {
            barraVidas[i].gameObject.SetActive(false);
        }
    }

    public void ShowLifeBar(int index)
    {
        barraVidas[index].gameObject.SetActive(true);
    } 
    public void HideLifeBar(int index)
    {
        barraVidas[index].gameObject.SetActive(false);
    } 


    public void InitBarraVida(string namePlayer, int i)
    {
        
        barraVidas[i].namePlayer.text = namePlayer;
        ShowLifeBar(i);
        
    }

    public void ShowItemsBg(bool isActive)
    {
        itemsBg.gameObject.SetActive(isActive);
    }

    public void ShowTextDisplay(bool condition)
    {
        textDisplay.gameObject.SetActive(condition);
    }

    public void RemoveBarraVida(int index)
    {
        var barra = barraVidas[index];
        barraVidas.Remove(barra);
        Destroy(barra.gameObject);
    }
}
