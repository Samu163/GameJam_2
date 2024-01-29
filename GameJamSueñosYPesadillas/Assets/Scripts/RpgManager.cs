using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RpgManager : MonoBehaviour
{
    public BoardController board;
    public PlayerController playerPrefabRef;
    public EnemyController enemyPrefabsRef;
    public List<EnemyController> enemies;
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
    public int activeEnemy = 0;
    public int indexOfAlly = 0;
    public bool hasInitHabilities = false;
    public bool isPlayerTurn = true;
    public bool selectCharacter = false;
    public bool selectAction = false;
    public string habilityName;
    bool isOnPlayerTarget;

    public enum state
    {
        SELECT_CHARACTER,
        SELECT_CHARACTER_FOR_CURE,
        SELECT_ACTION,
        SELECT_HABILITY,
        SELECT_ITEM,
        SELECT_ENEMY,
        NONE
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
            ally.Init(alliesInCombatConfig[i], CheckHabilityTarget);
            ally.InitHabilities();
            ally.ShowHabilities(false);
            allies.Add(ally);
        }
        for (int i = 0; i < enemyConfigs.Count; i++)
        {
            var enemy = Instantiate(enemyPrefabsRef, transform);
            enemy.transform.position = new Vector3(enemy.transform.position.x + 100, enemy.transform.position.y - 100 * i, enemy.transform.position.z);
            enemy.Init(enemyConfigs[i]);
            enemy.ShowSelectedIcon(false);
            enemies.Add(enemy);
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

    public void FindNextPlayerForCure(bool direction)
    {
        if (direction)
        {
            if (activePlayer <= 0)
            {
                activePlayer = allies.Count - 1;
            }
            else
            {
                activePlayer--;
            }
        }
        else
        {
            if (activePlayer >= allies.Count - 1)
            {
                activePlayer = 0;
            }
            else
            {
                activePlayer++;
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

    public void FindNextEnemy(bool direction)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (direction)
            {
                if (activePlayer <= 0)
                {
                    activeEnemy = enemies.Count - 1;
                }
                else
                {
                    activeEnemy--;
                }
            }
            else
            {
                if (activeEnemy >= enemies.Count - 1)
                {
                    activeEnemy = 0;
                }
                else
                {
                    activeEnemy++;
                }
            }

            if (enemies[activeEnemy].life > 0)
            {
                break;
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
                case state.SELECT_CHARACTER_FOR_CURE:
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        allies[activePlayer].ShowPlayerSelectedIcon(false);
                        FindNextPlayerForCure(true);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        allies[activePlayer].ShowPlayerSelectedIcon(false);

                    FindNextPlayerForCure(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        CheckHability(habilityName);
                        allies[activePlayer].hasAttacked = true;
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
                        activeHability = 0;
                        actionButtons[activeAction].ActiveOnClick();
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        actionButtons[activeAction].ShowSelectedIcon(false);

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
                       
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                    allies[activePlayer].buttonHabilities[activeHability].ShowSelectedImage(false);
                    allies[activePlayer].ShowHabilities(false);
                    activeHability = 0;

                    currentStep = state.SELECT_ACTION;
                    }
                    break;
                case state.SELECT_ITEM:                   
                    break;
                case state.SELECT_ENEMY:
                    enemies[activeEnemy].ShowSelectedIcon(true);

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        enemies[activeEnemy].ShowSelectedIcon(false);
                        FindNextEnemy(true);
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        enemies[activeEnemy].ShowSelectedIcon(false);

                        FindNextEnemy(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        enemies[activeEnemy].ShowSelectedIcon(false);
                        

                        CheckHability(habilityName);
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {

                        currentStep = state.SELECT_HABILITY;
                    }

                break;
                default:
                    break;
            }
       
        //Selecciona el objetivo 
    }
   

    public bool CheckAllPlayersHadAttacked()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (!allies[i].hasAttacked)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveEnemy(int index)
    {
        var enemy = enemies[index];
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    
    public void RemoveAlly(int index)
    {
        var ally = allies[index];
        allies.Remove(ally);
        Destroy(ally.gameObject);
    }

    public void PlayerEndingTurn()
    {

    }

    public void CheckBattleResults()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].life > 0)
            {
                break;
            }
            else if(i+1 == allies.Count)
            {
                //Derrota de los aliados 
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].life > 0)
            {
                break;
            }
            else if (i + 1 == enemies.Count)
            {
                //Derrota de los enemigos
            }
        }

    }


    public void EnemyTurn()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            var j = enemies[i].data.habilities.Count;
            CheckEnemyHabilityResult(enemies[i].data.habilities[GetRandomIndex(j)].habilityName);
        }
        SetPlayerTurn();
    }

    public void SetPlayerTurn()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            allies[i].hasAttacked = false;
        }
        activePlayer = 0;
        activeHability = 0;
        currentStep = state.SELECT_CHARACTER;

    }

    // Botón para usar Items
    public void itemButton()
    {
        var inventory = Instantiate(itemInventoryPrefabRef, transform);
        inventory.transform.position = new Vector3(100, 100, 0);
    }

    public int GetRandomIndex(int maxValue)
    {
        return Random.Range(0, maxValue);

    }

    public void CheckEnemyHabilityResult(string habilityName)
    {
        Debug.Log(habilityName);
        var j = GetRandomIndex(allies.Count);
        switch (habilityName)
        {
            case "Fuerte":
                //Añadir animacion de ataque
                Debug.Log("vaya reventada, tenia " + allies[j].life);
                allies[j].life -= 10;
                Debug.Log("Y le ha hecho 10 de daño al player:" + j + "por lo que ahora tiene" +allies[j].life);
                break;
            case "Malo":
                Debug.Log("No hace nada");
                break;
            default:
                break;
        }
        if (allies[j].life <= 0)
        {
            //destruir enemigo 
            RemoveAlly(activePlayer);
        }
        CheckBattleResults();
    }
    //Si afecta a todos los enemigos/aliados no hace falta poner traget 
    public void CheckHabilityTarget(string habilityName, bool targetEnemy, bool targetPlayer)
    {
        this.habilityName = habilityName;
        if (targetEnemy)
        {
            currentStep = state.SELECT_ENEMY;
        }
        else if (targetPlayer)
        {
            currentStep = state.SELECT_CHARACTER_FOR_CURE;
        }
        else
        {
            CheckHability(habilityName);
        }

    }


    public void CheckHability(string habilityName)
    {

        //active enemy es el enemigo a atacar y active player es el player al que se le aplica la accion (puede ser a si mismo) 
        switch (habilityName)
        {
            case "Punch":
                //Añadir animacion de ataque
                Debug.Log("Puñetazo");
                enemies[activeEnemy].life -= 5;
               
                Debug.Log(enemies[activeEnemy].life);

                break;

            case "Heal":
                allies[activePlayer].life += 10;
                Debug.Log(allies[activePlayer].life);
                break;

            case "Shot":
                Debug.Log("Disparo");
                break;
            case "Help":
                Debug.Log("Curación a Aliado");
                break;
            case "Shout":
                Debug.Log("Baja defensas enemigos");
                break;
            case "Hide Emotions":
                allies[activePlayer].defense += 10;
                Debug.Log(allies[activePlayer].defense);
                break;
            case "Let's Go":
                Debug.Log("Suma ataque a Aliado");
                break;
            case "Run":
                allies[activePlayer].attackPower += 20;
                allies[activePlayer].defense -= 10;
                Debug.Log("Suma Ataque y baja defensa");
                break;
            case "Blow Bottle":
                Debug.Log("Botellazo");
                break;
            case "Drink":
                allies[activePlayer].attackPower += 10;
                Debug.Log(allies[activePlayer].attackPower);
                break;
            default:
                break;
        }
        //Checkear si se tiene que destruir algo 
        if (enemies[activeEnemy].life <= 0)
        {
            //destruir enemigo 
            RemoveEnemy(activeEnemy);
        }
        if (allies[activePlayer].life <= 0)
        {
            //destruir enemigo 
            RemoveAlly(activePlayer);
        }
        allies[activePlayer].hasAttacked = true;
        CheckBattleResults();
        if (CheckAllPlayersHadAttacked())
        {
            allies[activePlayer].ShowPlayerSelectedIcon(false);
            EnemyTurn();
        }
        else
        {
            allies[activePlayer].ShowPlayerSelectedIcon(false);

            FindNextPlayer(true);
            currentStep = state.SELECT_CHARACTER;

        }
    }

    public void FightButton()
    {

        allies[activePlayer].ShowHabilities(true);
        currentStep = state.SELECT_HABILITY;
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
