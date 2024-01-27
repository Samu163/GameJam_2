using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController: MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public string[] lines;
    public int index;
    public float textSpeed = 0.1f;

    public void AsignText(string[] lines)
    {
        this.lines = lines;
    }


    // Start is called before the first frame update
    void Start()
    {
        dialogue.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogue.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogue.text = lines[index];

            }
        }
    }

    public void StartDialogue()
    {
        index = 0;

        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (var letter in lines[index].ToCharArray())
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            dialogue.text = string.Empty;
            StartCoroutine(WriteLine());

        }
        else
        {
            //Acaban las lineas, se desactiva de momento 
            gameObject.SetActive(false);
        }
    }
}
