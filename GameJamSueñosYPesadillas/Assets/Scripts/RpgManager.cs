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
    public int indexOfAlly = 0;
    public bool hasInitHabilities = false;



    // Start is called before the first frame update
    void Start()
    {
        //Aqui es cuando pasamos de escena 
        for (int i = 0; i < alliesInCombatConfig.Count; i++)
        {
            var ally = Instantiate(playerPrefabRef, transform);
            ally.transform.position = new Vector3(ally.transform.position.x - 100, ally.transform.position.y - 100 * i, ally.transform.position.z);
            ally.Init(alliesInCombatConfig[i],CheckHabilityResult);
            ally.InitHabilities();
            ally.ShowHabilities(false);
            allies.Add(ally);
        }
    }

    // Update is called once per frame
    void Update()
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
    
    public void CheckHabilityResult(string habilityName, PlayerController playerAffected)
    {
        switch (habilityName)
        {
            case "fuego":
                //Añadir animacion de ataque
                playerAffected.life += 10;
                Debug.Log(playerAffected.life);
                break;
            case "hielo":
                playerAffected.life -= 10;
                Debug.Log(playerAffected.life);
                break;
            default:
                break;
        }
        indexOfAlly++;
    }

    public void FightButton()
    {
        if (allies.Count <= indexOfAlly)
        {
            indexOfAlly = 0;
        }

        allies[indexOfAlly].ShowHabilities(true);
    }


    // Botón para Huir del combate
    public void fleeButton()
    {
        //allies[indexOfAlly].DestroyHabilities();
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
