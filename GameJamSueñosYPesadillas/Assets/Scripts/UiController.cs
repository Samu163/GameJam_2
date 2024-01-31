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
    }

    public void InitBarraVida(float x, float y, string namePlayer)
    {
        var barra = Instantiate(barraVida, transform);
        barra.namePlayer.text = namePlayer;
        barra.transform.position = new Vector3(x, y, barra.transform.position.z);
        barraVidas.Add(barra);
    }

    public void ShowItemsBg(bool isActive)
    {
        itemsBg.gameObject.SetActive(isActive);
    }

    public void ShowTextDisplay(bool condition)
    {
        textDisplay.gameObject.SetActive(condition);
    }
}
