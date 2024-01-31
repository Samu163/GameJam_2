using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReloadGame : MonoBehaviour
{

    public bool finished = false;
    private IEnumerator NextGame()
    {

        while (!finished)
        {
            yield return new WaitForSecondsRealtime(12);
            finished = true;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("NextGame");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && finished)
        {
            Destroy(GameManager.instance.gameObject);
            SceneManager.LoadScene("Title");
        }
    }
}
