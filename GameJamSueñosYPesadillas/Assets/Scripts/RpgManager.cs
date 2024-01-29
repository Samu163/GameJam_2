using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RpgManager : MonoBehaviour
{
    public BoardController board;
    public PlayerController playerPrefabRef;
    public EnemyController enemyPrefabsRef;
    public List<EnemyController> listOfEnemies;
    // public List<EnemyController> listOfEnemies;
    public List<EnemyConfig> enemyConfigs;
    public List<HabilityConfig> allHabilities;
    public List<PlayerConfig> alliesInCombatConfig;
    public List<PlayerController> allies;
    public List<ActionButton> actionButtons;
    public ItemManager itemInventoryPrefabRef;
    public state currentStep;
    public int activePlayer = 0;
    public int activeAction = 0;
    public int activeHability = 0;
    public int indexOfAlly = 0;
    public bool hasInitHabilities = false;
    public bool isPlayerTurn = true;
    public bool selectCharacter = false;
    public bool selectAction = false;

    public enum state
    {
        SELECT_CHARACTER,
        SELECT_ACTION,
        SELECT_HABILITY,
        SELECT_ITEM,
        SELECT_ENEMY
    };



    // Start is called before the first frame update
    void Start()
    {
        currentStep = state.SELECT_CHARACTER;
        //Aqui es cuando pasamos de escena 
        for (int i = 0; i < alliesInCombatConfig.Count; i++)
        {

            var ally = Instantiate(playerPrefabRef, transform);
            ally.transform.position = new Vector3(ally.transform.position.x - 100, ally.transform.position.y - 100 * i, ally.transform.position.z);
            ally.Init(alliesInCombatConfig[i], CheckHabilityResult);
            ally.InitHabilities();
            ally.ShowHabilities(false);
            allies.Add(ally);
        }
        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            var enemy = Instantiate(enemyPrefabsRef, transform);
            enemy.transform.position = new Vector3(enemy.transform.position.x + 100, enemy.transform.position.y - 100 * i, enemy.transform.position.z);
            enemy.Init(enemyConfigs[i]);
            listOfEnemies.Add(enemy);
        }

        for (int i = 0; i < actionButtons.Count; i++)
        {
            actionButtons[i].ShowSelectedIcon(false);
        }
    }
    //true es arriba false es abajo
    public void FindNextPlayer(bool direction)
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (direction)
            {
                if (activePlayer <= 0)
                {
                    activePlayer = allies.Count-1;
                }
                else
                {
                    activePlayer--;
                }
            }
            else
            {
                if (activePlayer >= allies.Count-1)
                {
                    activePlayer = 0;
                }
                else
                {
                    activePlayer++;
                }
            }

            if (!allies[activePlayer].hasAttacked)
            {
                break;
            }
        }
    }
    public void FindNextAction(bool direction)
    {
        if (direction)
        {
            if (activeAction <= 0)
            {
                activeAction = actionButtons.Count - 1;
            }
            else
            {
                activeAction--;
            }
        }
        else
        {
            if (activeAction >= actionButtons.Count - 1)
            {
                activeAction = 0;
            }
            else
            {
                activeAction++;
            }
        }
    }
     public void FindNextHability(bool direction)
    {
        if (direction)
        {
            if (activeHability <= 0)
            {
                activeHability = allies[activePlayer].buttonHabilities.Count - 1;
            }
            else
            {
                activeHability--;
            }
        }
        else
        {
            if (activeHability >= allies[activePlayer].buttonHabilities.Count - 1)
            {
                activeHability = 0;
            }
            else
            {
                activeHability++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            allies[activePlayer].ShowPlayerSelectedIcon(true);
            switch (currentStep)
            {
                //Selecciona el personaje 
                case state.SELECT_CHARACTER:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        allies[activePlayer].ShowPlayerSelectedIcon(false);
                        FindNextPlayer(true);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        allies[activePlayer].ShowPlayerSelectedIcon(false);

                        FindNextPlayer(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        currentStep = state.SELECT_ACTION;
                    }
                    break;
                //Selecciona la accion
                case state.SELECT_ACTION:
                    actionButtons[activeAction].ShowSelectedIcon(true);

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        actionButtons[activeAction].ShowSelectedIcon(false);
                        FindNextAction(true);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        actionButtons[activeAction].ShowSelectedIcon(false);
                        FindNextAction(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                    actionButtons[activeAction].ShowSelectedIcon(false);

                    actionButtons[activeAction].ActiveOnClick();
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentStep = state.SELECT_CHARACTER;
                    }
                    break;
                case state.SELECT_HABILITY:
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(true);
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                        FindNextHability(true);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                        FindNextHability(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                        allies[activePlayer].buttonHabilities[activeHability].OnButtonClick();
                        //Esto es provisional, en principio tendria que seleccionar al enemigo o al jugador 
                        allies[activePlayer].hasAttacked = true;
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        currentStep = state.SELECT_ACTION;
                    }
                    break;
                case state.SELECT_ITEM:                   
                    break;
                case state.SELECT_ENEMY:
                    
                    break;
                default:
                    break;
            }
       
        //Selecciona el objetivo 
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
    
    public void CheckHabilityResult(string habilityName, PlayerController playerAffected)
    {
        switch (habilityName)
        {
            case "Punch":
                //A�adir animacion de ataque
                Debug.Log("Pu�etazo");
                break;
            case "Shot":
                Debug.Log("Disparo");
                break;
            case "Help":
                Debug.Log("Curaci�n a Aliado");
                break;
            case "Shout":
                Debug.Log("Baja defensas enemigos");
                break;
            case "Hide Emotions":
                playerAffected.defense += 10;
                Debug.Log(playerAffected.defense);
                break;
            case "Let's Go":
                Debug.Log("Suma ataque a Aliado");
                break;
            case "Run":
                playerAffected.attackPower += 20;
                playerAffected.defense -= 10;
                Debug.Log("Suma Ataque y baja defensa");
                break;
            case "Blow Bottle":
                Debug.Log("Botellazo");
                break;
            case "Drink":
                playerAffected.attackPower += 10;
                Debug.Log(playerAffected.attackPower);
                break;
            default:
                break;
        }
        playerAffected.hasAttacked = true;

    }


    public void CheckHability()
    {

    }

    public void FightButton()
    {

        allies[activePlayer].ShowHabilities(true);
        currentStep = state.SELECT_HABILITY;
    }


    // Bot�n para Huir del combate
    public void fleeButton()
    {
        //allies[indexOfAlly].DestroyHabilities();
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
