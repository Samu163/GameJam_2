using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicTitleScreen : MonoBehaviour
{

    public AudioSource mainThemePlayer;

    // Start is called before the first frame update
    void Start()
    {
        mainThemePlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
