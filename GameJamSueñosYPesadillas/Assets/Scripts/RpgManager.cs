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

    // Botón para Luchar
    public void fightButton()
    {

    }

    // Botón para usar Items
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
                //Añadir animacion de ataque
                break;
            case "hielo":
                break;
            default:
                break;
        }
    }


    // Botón para Huir del combate
    public void fleeButton()
    {

    }

    // Acción cuando pulsamos botón de Punch
    public void punchButton()
    {

    }

    // Acción cuando pulsamos botón de Shot
    public void shotButton()
    {

    }

    // Acción cuando pulsamos botón de Help
    public void helpButton()
    {

    }

    // Acción cuando pulsamos botón de Shout
    public void shoutButton()
    {

    }

    // Acción cuando pulsamos botón de Hide Emotions
    public void hideEmotionsButton()
    {

    }

    // Acción cuando pulsamos botón de Run
    public void runButton()
    {

    }

    // Acción cuando pulsamos botón de Let's Go
    public void letsGoButton()
    {

    }

    // Acción cuando pulsamos botón de Drink
    public void drinkButton()
    {

    }

    // Acción cuando pulsamos botón de Blow Bottle
    public void blowBottleButton()
    {

    }
}
