using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public Button StartBt;
    public Button Language;



    // Start is called before the first frame update
    void Start()
    {
        StartBt.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void StartGame()
    {
        SceneManager.LoadScene("Narrative");


    }


}
