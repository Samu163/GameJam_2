using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class titleButton : MonoBehaviour
{


    public bool OnMe = false;
    public Button myButton;
    public GameObject Indicator;

    public int languageIndex = 0;
    public int speedIndex = 0;

    public TextMeshProUGUI myLanguage;
    public TextMeshProUGUI textSpeed;

    public TitleScreen title;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       


    }

    public void Press()
    {
       
            myButton.onClick.Invoke();
        
    }


    public void ChangeLanguage()
    {
        if(languageIndex == 0) { 
            languageIndex++;
            myLanguage.text = "Español";
          
        
        
        }
        else { languageIndex = 0; myLanguage.text = "English"; }

        title.language = myLanguage.text;
    }

    public void ChangeSpeed()
    {
        if (speedIndex == 0)
        {
            speedIndex++;
            textSpeed.text = "Medium";


        }
        else if (speedIndex == 1)
        {
            speedIndex++;
             textSpeed.text = "Fast"; 
        
        }
        else
        {
            speedIndex = 0;
             textSpeed.text = "Slow";
        }

        title.language = textSpeed.text;

    }

}
