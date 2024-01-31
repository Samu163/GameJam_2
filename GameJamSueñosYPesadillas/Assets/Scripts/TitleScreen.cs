using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public Button StartBt;
    public Button Language;

    public GameObject cursor;

    public List<titleButton> myButtons;
    public int index;
    public int activeOption = 0;

    public string language = "Spanish";
    public string speed = "slow";

    // Start is called before the first frame update
    void Start()
    {
        StartBt.onClick.AddListener(StartGame);
        myButtons[0].Indicator.gameObject.SetActive(true);
        myButtons[1].Indicator.gameObject.SetActive(false);
        myButtons[2].Indicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activeOption++;
            if (activeOption < 0) { activeOption = 2; }
            if (activeOption > 2) { activeOption = 0; }


            myButtons[0].Indicator.gameObject.SetActive(false);
            myButtons[1].Indicator.gameObject.SetActive(false);
            myButtons[2].Indicator.gameObject.SetActive(false);

            myButtons[activeOption].Indicator.gameObject.SetActive(true);


            //enemies[activeEnemy].ShowSelectedIcon(false);
            //FindNextEnemy(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            activeOption--;
            if (activeOption < 0) { activeOption = 2; }
            if (activeOption > 2) { activeOption = 0; }


            myButtons[0].Indicator.gameObject.SetActive(false);
            myButtons[1].Indicator.gameObject.SetActive(false);
            myButtons[2].Indicator.gameObject.SetActive(false);

            myButtons[activeOption].Indicator.gameObject.SetActive(true);

            //enemies[activeEnemy].ShowSelectedIcon(false);

            //FindNextEnemy(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            myButtons[activeOption].Press();
        }

      
    }


    void StartGame()
    {
        
        GameManager.instance.language = language;

        if (speed == "slow")
        {
            GameManager.instance.textSpeed = 0.1f;
        }
        else if (speed == "medium")
        {
            GameManager.instance.textSpeed = 0.3f;
        }
        else if (speed == "fast")
        {
            GameManager.instance.textSpeed = 0.5f;
        }

        SceneManager.LoadScene("Narrative");

    }


}
