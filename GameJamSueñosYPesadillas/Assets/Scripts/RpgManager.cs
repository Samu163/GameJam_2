using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgManager : MonoBehaviour
{
    public BoardController board;
    public PlayerController playerPrefabRef;
    public EnemyController enemyPrefabsRef;
    public List<EnemyController> listOfEnemies;
    public List<EnemyConfig> enemyConfigs;
    public List<HabilityConfig> allHabilities;
    public List<PlayerConfig> alliesInCombatConfig;
    public List<PlayerController> allies;
    public ItemManager itemInventoryPrefabRef;



    // Start is called before the first frame update
    void Start()
    {
        //Aqui es cuando pasamos de escena 
        for (int i = 0; i < alliesInCombatConfig.Count; i++)
        {
            var ally = Instantiate(playerPrefabRef, transform);
            ally.transform.position = new Vector3(ally.transform.position.x - 300, ally.transform.position.y - 50 * i, ally.transform.position.z);
            ally.Init(alliesInCombatConfig[i]);
            allies.Add(ally);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Bot�n para Luchar
    public void fightButton()
    {

    }

    // Bot�n para usar Items
    public void itemButton()
    {
        var inventory = Instantiate(itemInventoryPrefabRef, transform);
        inventory.transform.position = new Vector3(100, 100, 0);

    }

    public void GetRandomIndex(int maxValue)
    {

    }

    public void CheckEnemyHabilityResult(string habilityName)
    {
        switch (habilityName)
        {
            case "fuego":
                //A�adir animacion de ataque
                break;
            case "hielo":
                break;
            default:
                break;
        }
    }


    // Bot�n para Huir del combate
    public void fleeButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Punch
    public void punchButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Shot
    public void shotButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Help
    public void helpButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Shout
    public void shoutButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Hide Emotions
    public void hideEmotionsButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Run
    public void runButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Let's Go
    public void letsGoButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Drink
    public void drinkButton()
    {

    }

    // Acci�n cuando pulsamos bot�n de Blow Bottle
    public void blowBottleButton()
    {

    }
}
