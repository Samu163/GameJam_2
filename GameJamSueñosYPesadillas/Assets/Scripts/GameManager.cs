using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<int> decisions;
    public List<int> finalDecisions;
    public int lastTextIndex;
    public int lastDecisionIndex;
    public int finalIndex; //si es menor de X es malo y si es mayor que Y es bueno, si no neutral
    public int RPGResult;
    public int day;

    public string language = "Spanish";
    public float textSpeed = 0.5f;

    // Start is called before the first frame update


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveNarrative(List<int> decisions, int lastIndexText, int indexDecisions)
    {
        day++;
        for (int i = 0; i < decisions.Count; i++)
        {
            this.decisions.Add(decisions[i]);

        }
        lastTextIndex = lastIndexText+1;
        lastDecisionIndex = indexDecisions;
    }
    //Si es 0 es derrota, si es 1 es victoria 
    public void SaveRPGResult(int result)
    {
        RPGResult = result;
    }
}
