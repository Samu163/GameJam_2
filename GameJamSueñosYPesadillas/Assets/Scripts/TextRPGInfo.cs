using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRPGInfo : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public string textToRead;
    public float textSpeed = 0.1f;
    public bool isActive = false;


    public void Init(string text)
    {
        textToRead = text;
        SetTextEmpty();
        isActive = true;
        StartDialogue();
    }
    public void SetTextEmpty()
    {
        displayText.text = string.Empty;
    }

    public void SetFalse()
    {
        isActive = false;
    }

    public void Update()
    {
        if (isActive)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (var letter in textToRead.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

}
